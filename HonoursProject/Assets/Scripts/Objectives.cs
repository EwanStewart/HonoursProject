using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objectives : MonoBehaviour
{
    public TextMeshProUGUI txtComponent;
    private List<string> textLine;

    private string textFilePath;
    private int count= 0;
    // Start is called before the first frame update
    void Start()
    {

        textLine = new List<string>();
        txtComponent.text = string.Empty;
        TextAsset file;

        if (SceneManager.GetActiveScene().name == "Pointers")
        {
            file = Resources.Load("pointers") as TextAsset;
        }
        else
        {
            return;
        }


        string[] linesFromfile = file.text.Split("\n"[0]);
        foreach (string line in linesFromfile)
        {
            textLine.Add(line);

        }
        txtComponent.text = textLine[0];
    }

    public void nextLine()
    {
        count++;
        txtComponent.text = textLine[count];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
