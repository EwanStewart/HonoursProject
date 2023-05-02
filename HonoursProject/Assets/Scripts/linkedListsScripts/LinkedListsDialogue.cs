using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Database;

namespace LinkedListsScripts
{
    public class LinkedListsDialogue : MonoBehaviour
    {
        public TextMeshProUGUI txtComponent; //game-object for main text
        public Slider slider; 				 //slider object  to show progress of dialogue
        public GameObject panel;     		 //game-object for parent panel
        private TextAsset _text; 			 //text asset to load text file

        public string[] lines;				 //holds lines of text from text file
        private List<string> _textLine;	 	 //list to hold lines of text

        public float textSpeed; 			//speed of text appearing

        private int _index = 0; 				//index of current line of text array
        private int _imgCount  = 0; 			//index holding how many images have been displayed
        public RectTransform feedbackPanel; //game-object for feedback panel
        
        private void ClearFeedback()
        {
            if (PlayerPrefs.HasKey("username"))
            {
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge07").SetValueAsync(true);
            } else {
                SceneManager.LoadScene("sign-login");
            }
            feedbackPanel.gameObject.SetActive(false);
        }

        void Start()
        {
            if (PlayerPrefs.HasKey("imgCountLinkedLists")) { 				//check if imgCount has been set
                _imgCount = PlayerPrefs.GetInt("imgCountLinkedLists");		//fetch imgCount from playerprefs
            } else {
                PlayerPrefs.SetInt("imgCountLinkedLists", 0);				//else set imgCount to 0
            }
        
            if (PlayerPrefs.HasKey("LinkedListsPosition"))					 //check if pointersPosition has been set
            {
                var path = PlayerPrefs.GetString("LinkedListsPosition"); //fetch pointersPosition from playerprefs
                _text = Resources.Load(path) as TextAsset; 				 //load file as text asset
            } else {
                _text = Resources.Load("listContent1") as TextAsset;  		//else load initial file
                PlayerPrefs.SetString("LinkedListsPosition", "listContent1");  //set pointersPosition to default file
            }

            if (_text == null) {		//ensure text asset is not null, if it is, load initial file
                _text = Resources.Load("listContent1") as TextAsset;	
                PlayerPrefs.SetString("LinkedListsPosition", "listContent1"); PlayerPrefs.SetInt("imgCountLinkedLists", 0); PlayerPrefs.SetInt("objPositionLinkedLists", 0);	 //reset playerprefs values to initial values
                _imgCount = 0;	//reset imgCount to 0
            }

            if (_text == null)
            {
                return;
            }
            
            if (PlayerPrefs.GetString("LinkedListsPosition") == "listContent1") {
                feedbackPanel.gameObject.SetActive(true);
                feedbackPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = "'Linked Lists' Unlocked!";
                Invoke(nameof(ClearFeedback), 2);
            }
            
            lines = _text.text.Split('\n');	//split the text asset by new line into an array
            _textLine = new List<string>();  //create new list to hold lines of text

            foreach (var line in lines) {	//store array into list
                _textLine.Add(line);
                slider.maxValue += 1;	//increase progress bar length for every line of text
            }

            txtComponent.text = string.Empty;	//reset main text to empty string
            StartDialogue();	//start displaying text
        }

        void Update()
        {
            if (Input.touchCount > 0 || Input.anyKeyDown) //check if user has touched screen or pressed any key
            {
                if (_textLine[_index].ToCharArray()[0] == '/')	//check if first character of current line is a forward slash, denotes that an image is required on screen
                {
                    _textLine[_index] = _textLine[_index].Substring(1);	//remove forward slash from line
                }

                if (txtComponent.text == _textLine[_index]) //check if text has finished displaying
                {
                    if (_imgCount > 0)  { 				  //ensure imgCount has been set
                        panel.transform.GetChild(_imgCount).gameObject.SetActive(false);		//activate child of main panel with index of imgCount, to display image
                    }
                    NextLine(); 						  //display next line of text
                }
            }
        }

        void StartDialogue() 
        {
            _index = 0;	//reset index to 0
            StartCoroutine(TypeLine());	//type a line of text as a coroutine
        }

        IEnumerator TypeLine()  
        {
            if (_textLine[_index].ToCharArray()[0] == '/') 						//if first character of line is a forward slash, an image is required
            {
                _imgCount = PlayerPrefs.GetInt("imgCountLinkedLists");
                panel.transform.GetChild(_imgCount+1).gameObject.SetActive(true); //activate child of main panel with index of imgCount+1, to display image
                _imgCount++; 													//increase imgCount by 1
                PlayerPrefs.SetInt("imgCountLinkedLists", _imgCount);                       //save imgCount to playerprefs
            }
        
            foreach (var c in _textLine[_index].ToCharArray())  				//loop through each character in current line
            {
                if (c == '/') {} //if character is a forward slash, skip
                else { 			//type character with defined text speed
                    txtComponent.text += c; 
                    yield return new WaitForSeconds(textSpeed);
                }
            }
        
            slider.value += 1;	//increase progress bar value by 1
        }

        private void NextLine()	{
            if (_index < _textLine.Count - 1)	{		//check if another line of text exists
                _index++;   							//increase index by 1
                txtComponent.text = string.Empty;	//reset main text to empty string
                StartCoroutine(TypeLine());			//type a line of text as a coroutine
            } else {
                var path = PlayerPrefs.GetString("LinkedListsPosition"); //fetch pointersPosition from playerprefs
                var lastChar = path[^1];	//get number at end of string
                path = path.Remove(path.Length - 1);	//remove number from string
                path += (char)(lastChar + 1);			//increment number by 1 and add to string
                PlayerPrefs.SetString("LinkedListsPosition", path);	//save string to playerprefs

                var objCount = PlayerPrefs.GetInt("objPositionLinkedLists");	//fetch objPosition from playerprefs
                objCount++;			                    			//increase objPosition by 1	
                PlayerPrefs.SetInt("objPositionLinkedLists", objCount);		//save objPosition to playerprefs

                switch (path)
                {
                    //using where the user is in pointers content, load the next scene
                    case "listContent2":
                        SceneManager.LoadScene("LinkedListGame2");	//load matching scene
                        break;
                    case "listContent3":
                        SceneManager.LoadScene(23);	//load swipe scene
                        break;
                    case "listContent4":
                        SceneManager.LoadScene("LinkedListGame1");	//load matching scene
                        break;
                }
            }
        }
    }
}
