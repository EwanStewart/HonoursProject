using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace sortingScripts
{
    public class SortingDialogue : MonoBehaviour
    {
        public TextMeshProUGUI txtComponent; //gameobject for main text
        public Slider slider; 				 //slider object  to show progress of dialogue
        public GameObject panel;     		 //gameobject for parent panel
        private TextAsset text; 			 //text asset to load text file

        public string[] lines;				 //holds lines of text from text file
        private List<string> textLine;	 	 //list to hold lines of text

        public float textSpeed; 			//speed of text appearing

        private int index = 0; 				//index of current line of text array
        private int imgCount  = 0; 			//index holding how many images have been displayed

    
        void Start()
        {
            if (PlayerPrefs.HasKey("imgCount")) { 				//check if imgCount has been set
                imgCount = PlayerPrefs.GetInt("imgCount");		//fetch imgCount from playerprefs
            } else {
                PlayerPrefs.SetInt("imgCount", 0);				//else set imgCount to 0
            }
        
            if (PlayerPrefs.HasKey("sortingPosition"))					 //check if pointersPosition has been set
            {
                var path = PlayerPrefs.GetString("sortingPosition"); //fetch pointersPosition from playerprefs
                text = Resources.Load(path) as TextAsset; 				 //load file as text asset
            } else {
                text = Resources.Load("sortingContent1") as TextAsset;  		//else load initial file
                PlayerPrefs.SetString("sortingPosition", "sortingContent1");  //set pointersPosition to default file
            }

            if (text == null) {		//ensure text asset is not null, if it is, load initial file
                text = Resources.Load("sortingContent1") as TextAsset;	
                PlayerPrefs.SetString("sortingPosition", "sortingContent1"); PlayerPrefs.SetInt("imgCountSorting", 0); PlayerPrefs.SetInt("objPositionSorting", 0);	 //reset playerprefs values to initial values
                imgCount = 0;	//reset imgCount to 0
            }

            if (text == null)
            {
                return;
            }
            lines = text.text.Split('\n');	//split the text asset by new line into an array
            textLine = new List<string>();  //create new list to hold lines of text

            foreach (var line in lines) {	//store array into list
                textLine.Add(line);
                slider.maxValue += 1;	//increase progress bar length for every line of text
            }

            txtComponent.text = string.Empty;	//reset main text to empty string
            StartDialogue();	//start displaying text
        }

        void Update()
        {
            if (Input.touchCount > 0 || Input.anyKeyDown) //check if user has touched screen or pressed any key
            {
                if (textLine[index].ToCharArray()[0] == '/')	//check if first character of current line is a forward slash, denotes that an image is required on screen
                {
                    textLine[index] = textLine[index].Substring(1);	//remove forward slash from line
                }

                if (txtComponent.text == textLine[index]) //check if text has finished displaying
                {
                    if (imgCount > 0)  { 				  //ensure imgCount has been set
                        panel.transform.GetChild(imgCount).gameObject.SetActive(false);		//activate child of main panel with index of imgCount, to display image
                    }
                    NextLine(); 						  //display next line of text
                }
            }
        }

        void StartDialogue() 
        {
            index = 0;	//reset index to 0
            StartCoroutine(TypeLine());	//type a line of text as a coroutine
        }

        IEnumerator TypeLine()  
        {
            if (textLine[index].ToCharArray()[0] == '/') 						//if first character of line is a forward slash, an image is required
            {
                imgCount = PlayerPrefs.GetInt("imgCountSorting"); 				
                
                panel.transform.GetChild(imgCount+1).gameObject.SetActive(true); //activate child of main panel with index of imgCount+1, to display image
                imgCount++; 													//increase imgCount by 1
                PlayerPrefs.SetInt("imgCountSorting", imgCount);                       //save imgCount to playerprefs
            }
        
            foreach (var c in textLine[index].ToCharArray())  				//loop through each character in current line
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
            if (index < textLine.Count - 1)	{		//check if another line of text exists
                index++;   							//increase index by 1
                txtComponent.text = string.Empty;	//reset main text to empty string
                StartCoroutine(TypeLine());			//type a line of text as a coroutine
            } else {
                var path = PlayerPrefs.GetString("sortingPosition"); //fetch pointersPosition from playerprefs
                var lastChar = path[^1];	//get number at end of string
                path = path.Remove(path.Length - 1);	//remove number from string
                path += (char)(lastChar + 1);			//increment number by 1 and add to string
                PlayerPrefs.SetString("sortingPosition", path);	//save string to playerprefs

                var objCount = PlayerPrefs.GetInt("objPositionSorting");	//fetch objPosition from playerprefs
                objCount++;			                    			//increase objPosition by 1	
                PlayerPrefs.SetInt("objPositionSorting", objCount);		//save objPosition to playerprefs

                switch (path)
                {
                    //using where the user is in pointers content, load the next scene
                    case "sortingContent2":
                        SceneManager.LoadScene("NotationDrag");	//load matching scene
                        break;
                    case "sortingContent3":
                        SceneManager.LoadScene("OrganiseOrder");	//load swipe scene
                        break;
                    case "sortingContent4":
                        SceneManager.LoadScene("TrueFalseSorting");	//load swipe scene
                        break;
                }
            }
        }
    }
}
