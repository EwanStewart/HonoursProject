using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using UnityEngine.SceneManagement;

namespace sortingScripts
{
    public class NotationDrag : MonoBehaviour
    {
        public GameObject feedbackPanel;
        private int _correctCount;
        private RectTransform _rectTransform;
        private bool clearing = false;
        private TextMeshProUGUI text;
        private void Start()
        {
            _rectTransform = feedbackPanel.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (clearing) return;
            if (_correctCount == 5)
            {
                text.text = "Congratulations! You have unlocked the badge!";
                _rectTransform.gameObject.SetActive(true);
                unlockBadge();
            }
        }

        public void OnSubmit()
        {
            string[] buttonNames = { "ConstantButton", "LogarithmicButton", "LinearButton", "QuadraticButton", "CubicButton" };
            var panel = feedbackPanel;
            var panelTransform = panel.GetComponent<RectTransform>();
            var child = panel.transform.GetChild(0).gameObject;
            text = child.GetComponent<TextMeshProUGUI>();
            
            foreach (var buttonName in buttonNames)
            {
                var button = GameObject.Find(buttonName);
                var buttonComponent = button.GetComponent<Button>();
                var parent = button.transform.parent;
                var splitName = Regex.Split(buttonComponent.name, @"(?<!^)(?=[A-Z])");
                var splitParent = Regex.Split(parent.name, @"(?<!^)(?=[A-Z])");
                var equals = splitName[0].Equals(splitParent[0]);
                if (equals) {
                    _correctCount++;
                } else {
                }
            }
            
            if (panel != null) { 
                text.text = _correctCount == 5 ? "All Correct!" : "Not quite correct, try again!";
            }
            
            panelTransform.gameObject.SetActive(true);
            clearing = true;
            StartCoroutine(ClearFeedback());
        }

        private void unlockBadge()
        {
            if (PlayerPrefs.HasKey("username"))
            {
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge03").SetValueAsync(true);
                StartCoroutine(loadNextScene());
            } else
            {
                SceneManager.LoadScene("sign-login");
            }
        }
        private IEnumerator ClearFeedback()
        {
            yield return new WaitForSeconds(2);
            _rectTransform.gameObject.SetActive(false);
            clearing = false;
        }
        
        private IEnumerator loadNextScene()
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("sorting");
        }
    }
}
