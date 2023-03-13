using System;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace sortingScripts
{
	public class DragNDropSorting : MonoBehaviour
	{
		//gameobjects from scene
		private TextAsset _text;	
		public GameObject prefabButton;
		public RectTransform parentPanel;
		public RectTransform panelTrue;
		public RectTransform panelFalse;
		public RectTransform panelFeedback;
		public TextAsset text;

		private bool _done;    				//check if game over
		private int _consecutiveCorrect = 0;	//total correct answers in a row
		private int _buttonsSubmitted = 0;	//total statements correctly submitted

		public void SubmitAnswers() {
			var dict = new Dictionary<string, string>();	//create dictionary to hold statements and values
			var panelList = new List<RectTransform>();	//list to hold panels

			var panelIndex = 0;	//index to hold which panel is being checked
			var correctValueForPanel = true;	

			panelList.Add(panelTrue);
			panelList.Add(panelFalse);

			foreach (var k in PlayerPrefs.GetString("keys", "").Split(','))	//for each key in dictionary, add to dictionary with value from playerprefs
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

					if (!child.GetComponent<Button>()) continue; //if child is a button
					var tempButton = child.GetComponent<Button>();	//get button
					var tempText = tempButton.GetComponentInChildren<TextMeshProUGUI>().text; 	    //get text from button
					var tempValue = Convert.ToBoolean(dict[tempText]);	//convert value from dictionary to bool
          
					if (tempValue == correctValueForPanel) { 	//if value is correct
						_consecutiveCorrect++;					//increment correct answers in a row
						_buttonsSubmitted++;						//increment total statements submitted
						Destroy(tempButton.gameObject);			//destroy button to denote correct answer
					} else {																					//if wrong
						_consecutiveCorrect = 0;																				//reset correct answers in a row
						panelFeedback.gameObject.SetActive(true);															//show feedback panel to denote wrong answer	
						var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();				//get text from feedback panel
						feedBackTxt.text = "That's not quite it, at least one of your answers are incorrect. Try again.";	//change text to denote wrong answer
						Invoke(nameof(ClearFeedback), 3);																//clear feedback panel after 3 seconds
					}
				}
				panelIndex++;	//increment panel index, to go to next panel
			}
		}

		private void CheckIfBadgeMet()
		{
			//check if badge in this scene has been met
			if (_done) return;  //check if already met
			if (_consecutiveCorrect < 6) return; //if the user has got 5 correct answers in a row
			if (PlayerPrefs.HasKey("username")) {	//ensure user is logged in
				FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge06").SetValueAsync(true);	//store badge in firebase
				panelFeedback.gameObject.SetActive(true);																	  //set feedback panel to active
				var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();						  //get text from feedback panel
				feedBackTxt.text = "Congratulations! You aced this, you have unlocked a badge."; //change text to denote badge unlocked
			} else {
				PlayerPrefs.DeleteAll();				//delete all playerprefs
				SceneManager.LoadScene("sign-login");	//send user to login screen
			}
			_done = true;	//badge has been met
			Invoke(nameof(ClearFeedback), 3);	//clear feedback panel after 3 seconds
		}

		public void ClearFeedback()	//clear text of panel and hide panel
		{
			panelFeedback.gameObject.SetActive(false);
			var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
			feedBackTxt.text = "";
		}

		public void NextScene() {	//load next scene
			SceneManager.LoadScene("MainMenu");
		}

		private void Update()
		{
			if (!_done) {	//if badge hasen't been met check
				CheckIfBadgeMet();
			}

			if (panelFeedback.gameObject.activeSelf) return; //if feedback panel is not active
			if (_buttonsSubmitted < 6) return; //check if all statements have been submitted
			foreach (var k in PlayerPrefs.GetString("keys", "").Split(','))	//delete data stored in playerprefs pertaining to this scene
			{
				PlayerPrefs.DeleteKey(k);
			}

			panelFeedback.gameObject.SetActive(true);														//set feedback panel to active
			var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();			//get text from feedback panel
			feedBackTxt.text = "Well Done! You successfully sorted the statements into true and false. That's all the content for Big O Notation";	//change text to denote completion of scene
			Invoke(nameof(NextScene), 5);																			//load next scene after 3 seconds
		}

		private void Start()
		{
			var dict = new Dictionary<string, string>();
			var keys = "";

			var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();									//get text from feedback panel
			feedBackTxt.text = "Drag and drop the statements to the correct side depending on whether they are true or false.";		//change text to denote start of scene
			Invoke(nameof(ClearFeedback), 3);																								//clear feedback panel after 3 seconds
		
			_text = text;	//load text file from resources folder

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
