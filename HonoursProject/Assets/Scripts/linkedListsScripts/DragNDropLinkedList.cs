using System;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LinkedListsScripts
{
	public class DragNDropLinkedList : MonoBehaviour
	{
		//gameobjects from scene
		private TextAsset _text;	
		public GameObject prefabButton;
		public RectTransform parentPanel;
		public RectTransform panelTrue;
		public RectTransform panelFalse;
		public RectTransform panelFeedback;
		public RectTransform badgePanel;
		public TextAsset text;

		private bool _done;    				//check if game over
		private bool _incorrectAnswer;		//total incorrect answers

		private int _consecutiveCorrect = 0;	//total correct answers in a row
		private int _buttonsSubmitted = 0;		//total statements correctly submitted
		private TextMeshProUGUI _feedBackTxt;	//ref to feedback text
		private TextMeshProUGUI _badgeTxt;		//ref to badge text

		public void SubmitAnswers() {
			var dict = new Dictionary<string, string>();	//create dictionary to hold statements and values
			var panelList = new List<RectTransform> {panelTrue, panelFalse};		//list to hold panels

			var panelIndex = 0;								//index to hold which panel is being checked
			var correctValueForPanel = true;				//bool to check if statement is correct for panel

			
			foreach (var k in PlayerPrefs.GetString("keys", "").Split(','))	//for each key in dictionary, add to dictionary with value from PlayerPrefs
			{
				dict.Add(k, PlayerPrefs.GetString(k, ""));
			}
        
			foreach (var panel in panelList)	//for each panel
			{
				foreach (Transform child in panel)		//for each child in panel
				{
					if (panelIndex > 0)					//define correct answer for panel
					{
						correctValueForPanel = false;
					}

					if (!child.GetComponent<Button>()) { continue;		//if child is not a button, skip
}
					var tempButton = child.GetComponent<Button>();		//get button
					var tempText = tempButton.GetComponentInChildren<TextMeshProUGUI>().text; 	    //get text from button
					var tempValue = Convert.ToBoolean(dict[tempText]);	//convert value from dictionary to bool
          
					if (tempValue == correctValueForPanel) { 	//if value is correct
						_consecutiveCorrect++;						//increment correct answers in a row
						_buttonsSubmitted++;						//increment total statements submitted
						Destroy(tempButton.gameObject);				//destroy button to denote correct answer
					} else {									//if wrong
						_consecutiveCorrect = 0;					//reset correct answers in a row
						if (!_incorrectAnswer)					//if no incorrect answers yet
						{											//display feedback and set incorrect answer to true
							_incorrectAnswer = true;
							panelFeedback.gameObject.SetActive(true);															
							var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();				
							feedBackTxt.text = "That's not quite it, at least one of your answers are incorrect. Answer incorrectly again and you will be sent back to the content.";	
							Invoke(nameof(ClearFeedback), 3);
							return;
						}
						else 									//if incorrect answer already
						{											//display feedback and send back to content
							panelFeedback.gameObject.SetActive(true);															
							var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();				
							feedBackTxt.text = "That's not quite it, at least one of your answers are incorrect. You will now be sent back to the content.";	
							StartCoroutine(LoadScene("LinkedListsLessonUI"));
							PlayerPrefs.SetInt("imgCountLinkedLists", 6);
							PlayerPrefs.SetString("LinkedListsPosition", "listContent2");
							PlayerPrefs.DeleteKey("keys");
							Invoke(nameof(LoadSceneNoDelay), 3);
						}

					}
				}
				panelIndex++;	//increment panel index, to go to next panel
			}
		}

		private static System.Collections.IEnumerator LoadScene(string sceneName)	//load scene after 2 seconds
		{
			yield return new WaitForSeconds(2);
			SceneManager.LoadScene(sceneName);
		}

		public void LoadSceneNoDelay()	//load scene immediately, delete data stored in PlayerPrefs within this scene
		{
			PlayerPrefs.SetInt("imgCountLinkedLists", 8);
			PlayerPrefs.SetInt("objPositionLinkedLists", 2);
			PlayerPrefs.SetString("LinkedListsPosition", "listContent3");
			PlayerPrefs.DeleteKey("keys");
			SceneManager.LoadScene("LinkedListsLessonUI");
		}
	
		
		public void ClearFeedback()	//clear text of panel and hide panel
		{
			panelFeedback.gameObject.SetActive(false);
			var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
			feedBackTxt.text = "";
		}

		public void NextScene() {	//load levelSelect scene
			SceneManager.LoadScene("LevelSelect");
		}
		
		private void Update()
		{
			if (panelFeedback.gameObject.activeSelf) return; //if feedback panel is not active
			if (_buttonsSubmitted !=7) return;				 //check if all statements have been submitted
			foreach (var k in PlayerPrefs.GetString("keys", "").Split(','))	//delete data stored in PlayerPrefs for this scene
			{
				PlayerPrefs.DeleteKey(k);
			}

			panelFeedback.gameObject.SetActive(true);			//display feedback
			_feedBackTxt.text = "Well Done! You successfully sorted the statements into True and False. That's the end of this lesson!";	
			
			PlayerPrefs.SetInt("LinkedListsCompleted", 1); 		//set unit as completed
			
			if (PlayerPrefs.HasKey("username")) {				//if user is logged in, set badge as completed
				FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge08").SetValueAsync(true);	
				badgePanel.gameObject.SetActive(true);
			} else {
				PlayerPrefs.DeleteAll();				
				SceneManager.LoadScene("sign-login");	
			}
			
			Invoke(nameof(NextScene), 5);		 //load levelSelect scene after 5 seconds																
		}

		private void Start()
		{
			_badgeTxt = badgePanel.GetComponentInChildren<TextMeshProUGUI>();				//get text from badge panel
			_feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();			//get text from feedback panel
			var dict = new Dictionary<string, string>();
			var keys = "";

			_feedBackTxt.text = "Drag and drop the statements to the correct side depending on whether they are true or false.";				//change text to denote start of scene
			Invoke(nameof(ClearFeedback), 3);																								//clear feedback panel after 3 seconds
		
			_text = text;			//load text file from resources folder

			if (_text == null) {	//ensure file exists
				print("File not found");
				return;
			}
			
			var lines = _text.text.Split('\n');	//split text file into lines
			foreach (var line in lines) {		//for each line in text file
				var split = line.Split(',');	//split line into key and value
				dict.Add(split[0], split[1]);		//add key and value to dictionary
			}
        
			foreach (var entry in dict)	//for each key in dictionary
			{
				keys += entry.Key + ",";						//add key to string
				PlayerPrefs.SetString(entry.Key, entry.Value);	//store key and value in playerprefs
			}

			PlayerPrefs.SetString("keys", keys);				//store keys in playerprefs
			PlayerPrefs.Save();									//save playerprefs
            
			//for each key in dictionary, dynamically create a button
			foreach (var entry in dict)
			{
				var statementButton = Instantiate(prefabButton, parentPanel, false);
				statementButton.transform.localScale = new Vector3(1, 1, 1);
				var tempButton = statementButton.GetComponent<Button>();
				tempButton.transform.localPosition = new Vector3(UnityEngine.Random.Range(-200, 200), UnityEngine.Random.Range(-200, 200), 0);
				var tempText = statementButton.GetComponentInChildren<TextMeshProUGUI>();
				tempText.text = entry.Key;
				try {
					tempButton.GetComponentInChildren<Text>().text = entry.Value;				
				}
				catch (Exception e) {
					print(e);
				}
				tempButton.gameObject.SetActive(true);
			}
		}
	}
}
