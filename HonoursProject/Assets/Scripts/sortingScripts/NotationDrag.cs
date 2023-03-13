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
        private bool _clearing;
        private TextMeshProUGUI _text;
        private void Start()
        {
            _rectTransform = feedbackPanel.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_clearing) return;
            if (_correctCount != 5) return;
            _text.text = "Badge Unlocked! Nice work. You've unlocked the 'Master Sorter' badge";
            _rectTransform.gameObject.SetActive(true);
            UnlockBadge();
        }

        public void OnSubmit()
        {
            string[] buttonNames = { "ConstantButton", "LogarithmicButton", "LinearButton", "QuadraticButton", "CubicButton" };
            var panel = feedbackPanel;
            var panelTransform = panel.GetComponent<RectTransform>();
            var child = panel.transform.GetChild(0).gameObject;
            _text = child.GetComponent<TextMeshProUGUI>();
            
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
                }
            }
            
            if (panel != null) { 
                _text.text = _correctCount == 5 ? "All Correct!" : "Not quite correct, try again!";
            }
            
            panelTransform.gameObject.SetActive(true);
            _clearing = true;
            StartCoroutine(ClearFeedback());
        }

        private void UnlockBadge()
        {
            if (PlayerPrefs.HasKey("username"))
            {
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge04").SetValueAsync(true);
                StartCoroutine(LoadNextScene());
            } else
            {
                SceneManager.LoadScene("sign-login");
            }
        }
        private IEnumerator ClearFeedback()
        {
            yield return new WaitForSeconds(2);
            _rectTransform.gameObject.SetActive(false);
            _clearing = false;
        }
        
        private static IEnumerator LoadNextScene()
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("sorting");
        }
    }
}
