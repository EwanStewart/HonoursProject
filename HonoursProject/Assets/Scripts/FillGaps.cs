using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using Firebase.Database;
using UnityEngine.SceneManagement;


public class FillGaps : MonoBehaviour
{    
    private TextAsset text;
    public GameObject prefabButton;
    
    public RectTransform ParentPanel;
    public RectTransform panelText;
    public GameObject textObj;
    private List<List<string>> list = new List<List<string>>();
	private int currentIndex = 0;
	public RectTransform panelFeedback;

    public void clearFeedback()	//clear text of panel and hide panel
    {
		Debug.Log("Clearing feedback");
		panelFeedback.gameObject.SetActive(false);
		TextMeshProUGUI feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
		feedBackTxt.text = "";
    }

	public void submitAnswers() {
		int correctAnswers = 0;

		foreach (Transform child in panelText) {
			if (child.GetComponent<Button>() && child.GetComponentInChildren<TextMeshProUGUI>().text == "" && child.gameObject.activeSelf) {
				return;
			}
        }
		print("all buttons filled");

		List<Button> buttons = new List<Button>();

		foreach (Transform child in panelText) {
            if (child.GetComponent<Button>()) {
				if (child.gameObject.activeSelf) {
               	 	Button tempButton = child.GetComponent<Button>();
					string tempText = tempButton.GetComponentInChildren<TextMeshProUGUI>().text;
					buttons.Add(tempButton);	
				}		
            }
        }
		
		buttons.Sort((x, y) => x.name.CompareTo(y.name));
		buttons.Reverse();

		for (int i = 0; i < buttons.Count; i++) {
            if (buttons[i].GetComponentInChildren<TextMeshProUGUI>().text == PlayerPrefs.GetString("key" + i)) {
                correctAnswers++;
            }
        }

		panelFeedback.gameObject.SetActive(true);															
        TextMeshProUGUI feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();	

		if (correctAnswers == buttons.Count) {
            feedBackTxt.text = "That's correct!";	
			for (int i = 0; i < buttons.Count; i++) {
                PlayerPrefs.DeleteKey("key" + i);
            }
		} else {
			feedBackTxt.text = "That's not quite it, at least one of your answers are incorrect. Try again.";	//change text to denote wrong answer
        }
            Invoke("clearFeedback", 3);
	}

    void fillText(List<string> line)
    {
		
		TextMeshProUGUI z = panelText.GetChild(panelText.childCount - 1).GetComponent<TextMeshProUGUI>();
		z.text = "";
		float x, y = 0f;
		int i = 0;

        foreach (char a in line[0]) {
			z = panelText.GetChild(panelText.childCount - 1).GetComponent<TextMeshProUGUI>();
            if (a == '_') {
                GameObject goText = (GameObject)Instantiate(textObj);
                goText.transform.SetParent(panelText, false);

				y = panelText.GetChild(panelText.childCount - 2).transform.localPosition.y;
				x = panelText.GetChild(panelText.childCount - 2).transform.localPosition.x;
				
                goText.transform.localPosition = new Vector3(x, y-100, 0);
				goText.GetComponent<TextMeshProUGUI>().text = "";

				GameObject goButton = (GameObject)Instantiate(prefabButton);
				goButton.transform.SetParent(panelText, false);
				goButton.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
				Button tempButton = goButton.GetComponent<Button>();
				tempButton.transform.SetAsFirstSibling();
				tempButton.tag = "question";
				TextMeshProUGUI tempText = goButton.GetComponentInChildren<TextMeshProUGUI>();
				tempText.text = "";
				
				PlayerPrefs.SetString("key" + i, line[i+1]);
				
				i++;

				y = panelText.GetChild(panelText.childCount - 2).transform.localPosition.y;
				x = panelText.GetChild(panelText.childCount - 2).transform.localPosition.x;

				tempButton.transform.localPosition = new Vector3(x * -1.4f, y, 0);
		        tempButton.gameObject.SetActive(true);		
            } else {
				z.text += a;
            }
        }
    }
    
    void createButtonsForLine(List<string> line)
    {
        fillText(line);
        line.RemoveAt(0);

        foreach (string s in line) {
            GameObject goButton = (GameObject)Instantiate(prefabButton);
            goButton.transform.SetParent(ParentPanel, false);
            goButton.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            Button tempButton = goButton.GetComponent<Button>();
            tempButton.transform.localPosition = new Vector3(UnityEngine.Random.Range(-ParentPanel.rect.width / 2, ParentPanel.rect.width / 2), UnityEngine.Random.Range(-ParentPanel.rect.height / 2, ParentPanel.rect.height / 2), 0);
            tempButton.GetComponentInChildren<TextMeshProUGUI>().text = s;
			tempButton.tag = "answer";
            TextMeshProUGUI tempText = goButton.GetComponentInChildren<TextMeshProUGUI>();
			tempText.text = s;
            tempButton.gameObject.SetActive(true);
        }
    }
    void Start()
    {
        
        text = Resources.Load("pointersFillGaps") as TextAsset;	//load text file from resources folder

        if (text == null) {	//ensure file exists
            print("File not found");
            return;
        }
        
        
        string[] lines = text.text.Split('\n');	//split text file into lines
        
        foreach (string line in lines) {		//for each line in text file
            string[] split = line.Split(',');	//split line into key and value
            List<string> temp = new List<string>();
            foreach (string s in split) {
                temp.Add(s);
            }
            list.Add(temp);
        }

        createButtonsForLine(list[0]);
		
		

    }
}
