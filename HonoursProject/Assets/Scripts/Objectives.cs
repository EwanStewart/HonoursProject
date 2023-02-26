using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objectives : MonoBehaviour
{
    public TextMeshProUGUI txtComponent;
    private List<string> textLine;

    private string textFilePath;
    private int count = 0;

    void Start()
    {
        
        textLine = new List<string>(); 
        txtComponent.text = string.Empty;
        TextAsset file;

        if (SceneManager.GetActiveScene().name == "Pointers") {
            file = Resources.Load("pointers") as TextAsset; 
            if (!PlayerPrefs.HasKey("objPosition")) {
                PlayerPrefs.SetInt("objPosition", 0);
            } else  {
                count = PlayerPrefs.GetInt("objPosition");
            }
        } else if (SceneManager.GetActiveScene().name == "sorting") {
            file = Resources.Load("sorting") as TextAsset;
            if (!PlayerPrefs.HasKey("objPositionSorting")) {
                PlayerPrefs.SetInt("objPositionSorting", 0);
            }                 
            count = PlayerPrefs.GetInt("objPositionSorting");
        } else {
            return;
        }

        string[] linesFromfile = file.text.Split("\n"[0]); 
        foreach (string line in linesFromfile)
        {
            textLine.Add(line);

        }
        

		Debug.Log(count);
		Debug.Log(textLine.Count-1);
        txtComponent.text = textLine[count]; 
    }

    public void nextLine() 
    {
        count++;
        txtComponent.text = textLine[count];
    }
}
