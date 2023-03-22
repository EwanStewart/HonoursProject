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
                _text.text = _correctCount >= 5 ? "All Correct!" : "Not quite correct, try again!";
            }
            
            panelTransform.gameObject.SetActive(true);
            _clearing = true;
            StartCoroutine(ClearFeedback());
        }
        
        private IEnumerator ClearFeedback()
        {
            yield return new WaitForSeconds(2);
            _rectTransform.gameObject.SetActive(false);
            _clearing = false;
            if (_correctCount >= 5)
            {
                SceneManager.LoadScene("sorting");
            }
        }
    }
}
