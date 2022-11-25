using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    public GameObject question;
    public GameObject answerA;
    public GameObject answerB;
    public GameObject answerC;
    public GameObject answerD;

    private Button _lastPressed;

    public List<GameObject> gameObjList;

    
    private List<List<string>> _textLine;

    private int _count = 0;

    private const int WaitCount = 500;
    private int _waitCurrent = 0;
    private bool _waiting = false;

    


    // Start is called before the first frame update
    void Start()
    {
        _waitCurrent = WaitCount;

        _textLine = new List<List<string>>();
        gameObjList = new List<GameObject> {question, answerA, answerB, answerC, answerD};

        var file = Resources.Load("pointersQuiz") as TextAsset;

        var linesFromfile = file.text.Split("\n"[0]);
        foreach (string line in linesFromfile)
        {
            var a = new List<string>(line.Split(','));
            _textLine.Add(a);
        }

        
        foreach (var obj in gameObjList)
        {
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        SetText(_textLine[0]);


        foreach (var obj in gameObjList)
        {
            if (obj.name.Contains("Button"))
            {
                obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(obj));
            }
        }


    }



    private void NextQuestion()
    {
        if (_count < _textLine.Count - 1)
        {
            _count++;
            SetText(_textLine[_count]);
        }
        else
        {
            SceneManager.LoadScene("pointers");
        }
    }

    private void CheckAnswer(GameObject a)
    {
        
        var buttonText = a.GetComponentInChildren<TextMeshProUGUI>().text;
        print(buttonText);
        print(_textLine[_count][1]);
        var buttonColor = a.GetComponent<Button>().colors;

        if (buttonText.Contains(_textLine[_count][1]))
        {
            buttonColor.normalColor = Color.green;
            buttonColor.selectedColor = Color.green;
        }
        else
        {
            buttonColor.normalColor = Color.red;
            buttonColor.selectedColor = Color.red;

        }

        a.GetComponent<Button>().colors = buttonColor;

        _lastPressed = a.GetComponent<Button>();
        
        _waiting = true;
        
    }

    
    public List<GameObject> Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }

    private void SetText(List<string> data)
    {
        gameObjList[0].GetComponent<TMPro.TextMeshProUGUI>().text = data[0];

        var tempList = new List<GameObject>(gameObjList);
        tempList.RemoveAt(0);
        
        var shuffledList = Shuffle(tempList);

        for (var i = 0; i < shuffledList.Count; i++)
        {
            shuffledList[i].GetComponentInChildren<TextMeshProUGUI>().text = data[i + 1];
        }

    }



    

    // Update is called once per frame
    void Update()
    {
        if(_waiting)
        {
            _waitCurrent--;

            if(_waitCurrent <= 0)
            {
                NextQuestion();
                _waitCurrent = WaitCount;
                
                var buttonColor = _lastPressed.GetComponent<Button>().colors;
                buttonColor.normalColor = Color.white;
                buttonColor.selectedColor = Color.white;
                _lastPressed.GetComponent<Button>().colors = buttonColor;
                _waiting = false;
            }
        }

    }
}
