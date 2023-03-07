using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace sortingScripts
{
    public class NotationOrder : MonoBehaviour
    {
        public RectTransform panelOrder; //panel to hold all buttons
        public RectTransform feedbackPanel;
            [HideInInspector]
        public static float[] YPositions;

        private bool over = false;
        private bool waiting = false;
        
        private void SetFeedbackPanel(string text)
        {
            feedbackPanel.gameObject.SetActive(true);
            feedbackPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
        
        private void CloseFeedbackPanel()
        {
            waiting = false;
            feedbackPanel.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!waiting && over)
            {
                SceneManager.LoadScene("sorting");
            }
            
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
                over = true;
                waiting = true;
                Invoke(nameof(CloseFeedbackPanel), 3f);
            } else {
                SetFeedbackPanel("That's not quite right. Try again!");
                Invoke(nameof(CloseFeedbackPanel), 3f);
            }
        }
        private void RandomOrder()
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

