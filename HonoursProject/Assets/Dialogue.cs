using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI txtComponent;
    public string[] lines;
    
    public float textSpeed;
    private int index;
    private string[] paths;

    // Start is called before the first frame update



    void Start()
    {

        lines = new string[2] { "A pointer is an object that stores a memory address, " +
            "a powerful feature... but also a dangerous one.", 
            "A pointer is a reference type – it points to another variable in memory." };

        //paths = new string[2] { "pointers-1", "pointers-2"};

        //image.sprite = Resources.Load<Sprite>("images/" + paths[0]);


        txtComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.anyKeyDown)
        {
            if (txtComponent.text == lines[index])
            {
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
        foreach (char c in lines[index].ToCharArray())
        {
            txtComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length -1)
        {
            index++;
            txtComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            gameObject.SetActive(false);
        }
    }
}
