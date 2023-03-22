using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;

namespace sortingScripts
{
    public class NotationOrder : MonoBehaviour
    {
        public RectTransform panelOrder; //panel to hold all buttons
        public RectTransform feedbackPanel;
        public RectTransform badgePanel;
        [HideInInspector]
        public static float[] YPositions;


        private bool _over = false;
        private bool _waiting = false;
        
        private void SetFeedbackPanel(string text)
        {
            feedbackPanel.gameObject.SetActive(true);
            feedbackPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
        
        private void CloseFeedbackPanel()
        {
            _waiting = false;
            feedbackPanel.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_waiting && _over)
            {
                var badgeText = badgePanel.GetComponentInChildren<TextMeshProUGUI>();
                
                badgeText.text = "'Sorted it' unlocked!";
                badgePanel.gameObject.SetActive(true);
                
                if (PlayerPrefs.HasKey("username")) {	
                    FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge04").SetValueAsync(true);	//store badge in firebase
                } else {
                    PlayerPrefs.DeleteAll();				
                    SceneManager.LoadScene("sign-login");	
                }
                
                SetFeedbackPanel("You've successfully sorted the notations!");
                Invoke(nameof(LoadNextScene), 4);
            }
        }
        
        private void LoadNextScene()
        {
            SceneManager.LoadScene("sorting");
        }

        public void OnSubmit()
        {
            //get the buttons in the panel in order of their y position
            var buttons = panelOrder.GetComponentsInChildren<Button>();
            Array.Sort(buttons, (a, b) => b.transform.position.y.CompareTo(a.transform.position.y));
            var correctOrder = true;
            for (var i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].CompareTag(i.ToString())) continue;
                correctOrder = false;
                break;
            }
            if (correctOrder)
            {
                SetFeedbackPanel("That's the correct order!");
                _over = true;
                _waiting = true;
                Invoke(nameof(CloseFeedbackPanel), 3f);
            } else {
                SetFeedbackPanel("That's not quite right. Try again!");
                Invoke(nameof(CloseFeedbackPanel), 3f);
            }
        }
        public void RandomOrder()
        {
            var buttons = panelOrder.GetComponentsInChildren<Button>();
            YPositions = new float[buttons.Length];
            for (var i = 0; i < buttons.Length; i++)
            {
                var temp = buttons[i].transform.position;
                var randomIndex = UnityEngine.Random.Range(i, buttons.Length);
                buttons[i].transform.position = buttons[randomIndex].transform.position;
                buttons[randomIndex].transform.position = temp;
                YPositions[i] = buttons[i].transform.position.y;
            }
            Array.Sort(YPositions);
        }
        private void Start()
        {
            RandomOrder();
        }
    }
}

