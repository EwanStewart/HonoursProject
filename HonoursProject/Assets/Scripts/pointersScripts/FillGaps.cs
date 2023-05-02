using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Database;
namespace pointersScripts
{
	public class FillGaps : MonoBehaviour
	{    
		private TextAsset _text;
		public GameObject prefabButton;
    
		public RectTransform parentPanel;
		public RectTransform panelText;
		public GameObject badgePanel;
		
		public GameObject textObj;
		
		private bool _done = false;
		private bool _waiting = false;
		private readonly List<List<string>> _list = new();
		public RectTransform panelFeedback;

		public void ClearFeedback()	//clear feedback on screen, if correct move to next question else reset
		{
			panelFeedback.gameObject.SetActive(false);
			var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
			if (feedBackTxt.text == "That's not quite it, at least one of your answers are incorrect. Try again.")
			{
				ClearAnswers();
			}
			else
			{
				var currentIndex = PlayerPrefs.GetInt("currentIndexFill");
				currentIndex++;
				PlayerPrefs.SetInt("currentIndexFill", currentIndex);
				ClearAnswers();
			}
			feedBackTxt.text = "";
			_waiting = false;
		}

		public void ClearAnswers()
		{ 
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void SubmitAnswers() {
			var correctAnswers = 0;
 
			foreach (Transform child in panelText) { //check if all buttons have been filled
				if (child.GetComponent<Button>() && child.GetComponentInChildren<TextMeshProUGUI>().text == "" && child.gameObject.activeSelf) {
					return;
				}
			}

			var buttons = new List<Button>();

			foreach (Transform child in panelText) //store all buttons in list
			{
				if (!child.GetComponent<Button>()) continue;
				if (!child.gameObject.activeSelf) continue;
				var tempButton = child.GetComponent<Button>();
				var tempText = tempButton.GetComponentInChildren<TextMeshProUGUI>().text;
				buttons.Add(tempButton);
			}
		
			buttons.Sort((x, y) => string.Compare(x.name, y.name, StringComparison.Ordinal)); //sort buttons by name
			buttons.Reverse(); //reverse list to get correct order

			for (var i = 0; i < buttons.Count; i++) { //get number of correct answers
				if (buttons[i].GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("key" + i)) {
					correctAnswers++;
				}
			}

			panelFeedback.gameObject.SetActive(true);															
			var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();	

			if (correctAnswers == buttons.Count) { //check if all answers are correct
				feedBackTxt.text = "That's correct!";
				_waiting = true;
				for (var i = 0; i < buttons.Count; i++) { //clear key cache
					PlayerPrefs.DeleteKey("key" + i);
				}
			} else {
				feedBackTxt.text = "That's not quite it, at least one of your answers are incorrect. Try again.";	//change text to denote wrong answer
			}
			Invoke(nameof(ClearFeedback), 3);
		}

		private void FillText(IReadOnlyList<string> line)
		{
		
			var z = panelText.GetChild(panelText.childCount - 1).GetComponent<TextMeshProUGUI>();
			z.text = "";
			var i = 0;

			foreach (var character in line[0]) //fill text with question
			{
	
				z = panelText.GetChild(panelText.childCount - 1).GetComponent<TextMeshProUGUI>();
				var rect = z.rectTransform.localPosition;
				var width = z.preferredWidth;
				var height = z.preferredHeight;
	        
				if (character == '_') { //create button if character is '_'
					var goButton = (GameObject)Instantiate(prefabButton, panelText, false);
					goButton.transform.localPosition = new Vector3(rect.x+width, rect.y, 0);
					goButton.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
					goButton.GetComponentInChildren<TextMeshProUGUI>().text = "";
					goButton.gameObject.SetActive(true);
					goButton.tag = "question";
					goButton.name = "button" + i;
					PlayerPrefs.SetString("key" + i, line[i + 1]);
	            
					var goText = (GameObject)Instantiate(textObj, panelText, false); //create new text after button
					var tempText = goText.GetComponent<TextMeshProUGUI>();
					tempText.text = "";
					tempText.transform.localPosition = new Vector3(rect.x, rect.y-(height*2), 0);
					tempText.gameObject.SetActive(true);
					tempText.name = "text" + i;
					i++;
				} else {
					z.text += character;
				}
			}
        
        
			foreach (Transform child in panelText)
			{
				if (!child.GetComponent<TextMeshProUGUI>()) continue;
				var tempText = child.GetComponent<TextMeshProUGUI>();
				var rect = tempText.rectTransform.localPosition;
				var width = tempText.preferredWidth;
				var height = tempText.preferredHeight;
				tempText.transform.localPosition = new Vector3(rect.x + width/2, rect.y, 0);
			}

			foreach (Transform child in panelText)
			{
				if (!child.GetComponent<Button>()) continue;
				var tempButton = child.GetComponent<Button>();
				var tempText = child.GetSiblingIndex() - 1;
				var tempText2 = panelText.GetChild(tempText).GetComponent<TextMeshProUGUI>();
				var rect = tempText2.rectTransform.localPosition;
				var width = tempText2.preferredWidth;
				var height = tempText2.preferredHeight;
				tempButton.transform.localPosition = new Vector3(rect.x + width, rect.y, 0);
	        
			}
		}

		private void CreateButtonsForLine(List<string> line)
		{
			FillText(line);
			line.RemoveAt(0); //remove question from list
			var coordinates = new List<Vector2>();

			foreach (var s in line) //create buttons for answers
			{
				float x; float y;
				var goButton = (GameObject)Instantiate(prefabButton, parentPanel, false);
				goButton.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
				var tempButton = goButton.GetComponent<Button>();
				var rect = parentPanel.rect;
				if (coordinates.Count == 0) //set coordinates for first button
				{
					x = rect.xMin + 300;
					y = rect.yMax - 100;
					coordinates.Add(new Vector2(x, y));
				}
				else //set coordinates for other buttons relative to previous button
				{
					var last = coordinates[^1];
					x = last.x + 500;
					y = last.y;
					if (x > rect.xMax - 300)
					{
						x = rect.xMin + 300;
						y = last.y - 100;
					}
					coordinates.Add(new Vector2(x, y));
				}

				tempButton.transform.localPosition = new Vector3(x, y, 0);
				tempButton.GetComponentInChildren<TextMeshProUGUI>().text = s; //set text for button
				tempButton.tag = "answer";										//set tag for button
				var tempText = goButton.GetComponentInChildren<TextMeshProUGUI>();
				tempText.text = s;
				tempButton.gameObject.SetActive(true);
			}
		}

		private void Update()
		{
			if (!_waiting && _done) // if not waiting for feedback to clear and game over
			{
				var feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
				var badgeText = badgePanel.GetComponentInChildren<TextMeshProUGUI>();

				feedBackTxt.text = "That's all for pointers!";
				badgeText.text = "'It's Over!' unlocked!";
				PlayerPrefs.SetInt("PointersCompleted", 1); //set level completed

				var z = panelText.GetChild(panelText.childCount - 1).GetComponent<TextMeshProUGUI>();
				z.text = "";
			
				
				panelFeedback.gameObject.SetActive(true);
				
				if (PlayerPrefs.HasKey("username")) {	 //unlock badge
					FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge03").SetValueAsync(true);	
					badgePanel.gameObject.SetActive(true);
				} else {
					PlayerPrefs.DeleteAll();				
					SceneManager.LoadScene("sign-login");	
				}
				
				Invoke(nameof(NextScene), 3);
			}
		}
		
		private void NextScene() //load next scene and clear PlayerPrefs
		{
			PlayerPrefs.DeleteKey("objPosition");
			PlayerPrefs.DeleteKey("currentIndexFill");
			PlayerPrefs.DeleteKey("imgCount");
			PlayerPrefs.DeleteKey("pointersPosition");
			SceneManager.LoadScene("LevelSelect");
		}

		private void Start()
		{
			_text = Resources.Load("pointersFillGaps") as TextAsset;	//load text file from resources folder

			if (_text == null) {	//ensure file exists
				print("File not found");
				return;
			}
        
        
			var lines = _text.text.Split('\n');	//split text file into lines
        
			foreach (var line in lines) {		//for each line in text file
				var split = line.Split(',');	//split line into key and value
				var temp = new List<string>();
				foreach (var s in split) {
					temp.Add(s);
				}
				_list.Add(temp);
			}
        
			var pos = 0; 
			if (PlayerPrefs.HasKey("currentIndexFill")) { //if key exists, get value
				pos = PlayerPrefs.GetInt("currentIndexFill");
			}
			if (pos > _list.Count-1) { //if value is greater than number of lines, game is over
				_done = true;
				return;
			}
			
        
			PlayerPrefs.SetInt("currentIndexFill", pos); //set key to current line
			CreateButtonsForLine(_list[pos]); //create visual for current line
		}
	}
}
