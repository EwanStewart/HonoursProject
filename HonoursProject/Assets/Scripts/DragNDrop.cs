using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;


public class DragNDrop : MonoBehaviour
{
    private TextAsset text;
    public GameObject prefabButton;
    public RectTransform ParentPanel;
    public RectTransform panelTrue;
    public RectTransform panelFalse;
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
                    print(tempText);
                    print(tempValue);
                    
                    if (tempValue == a)
                    { 
                        Destroy(tempButton.gameObject);
                    }

                }
            }

            i++;
        }





    }
    
    void Start()
    {
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
