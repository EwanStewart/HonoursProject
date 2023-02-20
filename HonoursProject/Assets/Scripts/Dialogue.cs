using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
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
    private TextAsset text;
    public Slider slider;
    void Start()
    {
        
        if (PlayerPrefs.HasKey("imgCount"))
        {
            imgCount = PlayerPrefs.GetInt("imgCount");
        }
        else
        {
            PlayerPrefs.SetInt("imgCount", 0);
        }
        
        if (PlayerPrefs.HasKey("pointersPosition"))
        {
            string path = PlayerPrefs.GetString("pointersPosition");
            text = Resources.Load(path) as TextAsset;
        }
        else
        {
            text = Resources.Load("pointersContent1") as TextAsset;
            PlayerPrefs.SetString("pointersPosition", "pointersContent1");
        }

        if (text == null)
        {
            text = Resources.Load("pointersContent1") as TextAsset;
            PlayerPrefs.SetString("pointersPosition", "pointersContent1");
            PlayerPrefs.SetInt("imgCount", 0);
            imgCount = 0;
            PlayerPrefs.SetInt("objPosition", 0);
        }
        lines = text.text.Split('\n');
        textLine = new List<string>();
        foreach (string line in lines)
        {
            textLine.Add(line);
            slider.maxValue += 1;

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
            PlayerPrefs.SetInt("imgCount", imgCount);
        }
        

        foreach (char c in textLine[index].ToCharArray()) 
        {
            if (c == '/') {}
            else {
                txtComponent.text += c; 
                yield return new WaitForSeconds(textSpeed);
            }
        }
        
        slider.value += 1;
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
            string path = PlayerPrefs.GetString("pointersPosition");
            char lastChar = path[path.Length - 1];
            int objCount = PlayerPrefs.GetInt("objPosition");
            objCount++;
            PlayerPrefs.SetInt("objPosition", objCount);
            path = path.Remove(path.Length - 1);
            path += (char)(lastChar + 1);
            PlayerPrefs.SetString("pointersPosition", path);
            string pointersPosition = PlayerPrefs.GetString("pointersPosition");
            Debug.Log(pointersPosition);
            
            if (pointersPosition == "pointersContent2")
            {
                SceneManager.LoadScene("Pointers1");
            } else if (pointersPosition == "pointersContent3") {
                SceneManager.LoadScene("SwipeLeftRight");
            }
        }
        
    }
}
