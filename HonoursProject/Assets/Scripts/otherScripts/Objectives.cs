using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace otherScripts
{
    public class Objectives : MonoBehaviour
    {
        public TextMeshProUGUI txtComponent;
        private List<string> _textLine;

        private string _textFilePath;
        private int _count = 0;

        private void Start()
        {
        
            _textLine = new List<string>(); 
            txtComponent.text = string.Empty;
            TextAsset file;

            if (SceneManager.GetActiveScene().name == "Pointers") {
                file = Resources.Load("pointers") as TextAsset; 
                if (!PlayerPrefs.HasKey("objPosition")) {
                    PlayerPrefs.SetInt("objPosition", 0);
                } else  {
                    _count = PlayerPrefs.GetInt("objPosition");
                }
            } else if (SceneManager.GetActiveScene().name == "sorting") {
                file = Resources.Load("sorting") as TextAsset;
                if (!PlayerPrefs.HasKey("objPositionSorting")) {
                    PlayerPrefs.SetInt("objPositionSorting", 0);
                }                 
                _count = PlayerPrefs.GetInt("objPositionSorting");
            } else {
                file = Resources.Load("LinkedLists") as TextAsset;
                if (!PlayerPrefs.HasKey("objPositionLinkedLists")) {
                    PlayerPrefs.SetInt("objPositionLinkedLists", 0);
                }                 
                _count = PlayerPrefs.GetInt("objPositionLinkedLists");
            }

            string[] linesFromfile = file.text.Split("\n"[0]); 
            foreach (string line in linesFromfile)
            {
                _textLine.Add(line);

            }
            
            txtComponent.text = _textLine[_count]; 
        }

        public void nextLine() 
        {
            _count++;
            txtComponent.text = _textLine[_count];
        }
    }
}
