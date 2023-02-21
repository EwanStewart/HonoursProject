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

public class DragNDrop : MonoBehaviour
{
    private TextAsset text;
    public GameObject prefabButton;
    public RectTransform ParentPanel;
    public RectTransform panelTrue;
    public RectTransform panelFalse;
	public RectTransform panelFeedback;
	bool Done = false;

	private int consecutiveCorrect = 0;
	private int buttonsSubmited = 0;
    public void submit()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        foreach (string k in PlayerPrefs.GetString("keys", "").Split(','))
        {
            string v = PlayerPrefs.GetString(k, "");
            dict.Add(k, v);
        }
        
        //create a panel list
        List<RectTransform> panelList = new List<RectTransform>();
        panelList.Add(panelTrue);
        panelList.Add(panelFalse);

        int i = 0;
        bool a = true;
        
        foreach (RectTransform panel in panelList)
        {
            
            foreach (Transform child in panel)
            {
                if (i > 0)
                {
                    a = false;
                }
               
                if (child.GetComponent<Button>())
                {
                    Button tempButton = child.GetComponent<Button>();
                    string tempText = tempButton.GetComponentInChildren<TextMeshProUGUI>().text;
                    bool tempValue = Convert.ToBoolean(dict[tempText]);
          
                    if (tempValue == a)
                    { 
						consecutiveCorrect++;
					    buttonsSubmited++;
                        Destroy(tempButton.gameObject);
                    } else {
						consecutiveCorrect = 0;
						panelFeedback.gameObject.SetActive(true);
						TextMeshProUGUI feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
						feedBackTxt.text = "That's not quite it, at least one of your answers are incorrect. Try again.";
						Invoke("clearFeedback", 3);
					}
                }
            }

            i++;
        }
	
		if (!Done) {
			if (consecutiveCorrect >=2) {
				if (PlayerPrefs.HasKey("username"))
        		{
            		FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge01").SetValueAsync(true);
					panelFeedback.gameObject.SetActive(true);
					TextMeshProUGUI feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
					feedBackTxt.text = "Congratulations! You have got five answers correct in a row! You have unlocked a badge.";
					Invoke("clearFeedback", 3);
        		} else
        		{
					PlayerPrefs.DeleteAll();
            		SceneManager.LoadScene("sign-login");
        		}
				Done = true;
			}
		} 

    }

    public void clearFeedback()
    {
		panelFeedback.gameObject.SetActive(false);
		TextMeshProUGUI feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
		feedBackTxt.text = "";
    }
	
 	void Update()
    {
		if (buttonsSubmited >= 8) {
			SceneManager.LoadScene("Pointers");
        }
	}

    void Start()
    {
		TextMeshProUGUI feedBackTxt = panelFeedback.GetComponentInChildren<TextMeshProUGUI>();
		feedBackTxt.text = "Drag and drop the statements to the correct side depending on whether they are true or false.";
	    Invoke("clearFeedback", 3);
		
        string fileName = "pointersDragDrop";
        text = Resources.Load(fileName) as TextAsset;

        if (text == null)
        {
            print("File not found");
            return;
        }
        
        
        string[] lines = text.text.Split('\n');
        
        Dictionary<string, string> dict = new Dictionary<string, string>();
        
        foreach (string line in lines)
        {
            string[] split = line.Split(',');
            dict.Add(split[0], split[1]);
        }
        

        string keys = "";

        foreach (KeyValuePair<string, string> entry in dict)
        {
            keys += entry.Key + ",";
            PlayerPrefs.SetString(entry.Key, entry.Value);
        }

        PlayerPrefs.SetString("keys", keys);
        PlayerPrefs.Save();
            
    


        //for each key in dictionary, dynamically create a button
        foreach (KeyValuePair<string, string> entry in dict)
        {
            GameObject goButton = (GameObject)Instantiate(prefabButton);
            goButton.transform.SetParent(ParentPanel, false);
            goButton.transform.localScale = new Vector3(1, 1, 1);
            Button tempButton = goButton.GetComponent<Button>();
            tempButton.transform.localPosition = new Vector3(UnityEngine.Random.Range(-200, 200), UnityEngine.Random.Range(-200, 200), 0);
            TextMeshProUGUI tempText = goButton.GetComponentInChildren<TextMeshProUGUI>();
            tempText.text = entry.Key;
            try
            {
                tempButton.GetComponentInChildren<Text>().text = entry.Value;				
            }
            catch (System.Exception e)
            {
                print(e);
            }
            
            tempButton.gameObject.SetActive(true);
        }
    }
}
