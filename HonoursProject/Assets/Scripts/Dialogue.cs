using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI txtComponent;
    public string[] lines;
    
    public float textSpeed;
    private int index = 0;
    private string[] paths;
    private List<string> textLine;
    public GameObject panel;
    
    private int imgCount  = 0;
    void Start()
    {
        textLine = new List<string>(); 
        TextAsset file = Resources.Load("pointersContent") as TextAsset;  
        string[] linesFromfile = file.text.Split("\n"[0]); 
        foreach (string line in linesFromfile)
        {
            textLine.Add(line);
        }
        txtComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.anyKeyDown) 
        {
            if (textLine[index].ToCharArray()[0] == '/')
            {
                textLine[index] = textLine[index].Substring(1);
            }

            if (txtComponent.text == textLine[index])
            {
                if (imgCount > 0) 
                {
                    panel.transform.GetChild(imgCount).gameObject.SetActive(false);
                }
                NextLine();
            }

        }

    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() 
    {

        if (textLine[index].ToCharArray()[0] == '/')
        {
            panel.transform.GetChild(imgCount+1).gameObject.SetActive(true);
            imgCount++;
        }
        

        foreach (char c in textLine[index].ToCharArray()) 
        {
            if (c == '/') {}
            else {
                txtComponent.text += c; 
                yield return new WaitForSeconds(textSpeed);
            }
        }
        
    }

    void NextLine()
    {
        if (index < textLine.Count - 1)
        {
            index++;
            txtComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
}
