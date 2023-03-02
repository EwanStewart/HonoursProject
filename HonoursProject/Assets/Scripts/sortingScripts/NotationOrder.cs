using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sortingScripts
{
    public class NotationOrder : MonoBehaviour
    {
        public RectTransform panelOrder; //panel to hold all buttons
        public RectTransform feedbackPanel;
            [HideInInspector]
        public static float[] yPositions;


        private void SetFeedbackPanel(string text)
        {
            feedbackPanel.gameObject.SetActive(true);
            feedbackPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
        
        private void CloseFeedbackPanel()
        {
            feedbackPanel.gameObject.SetActive(false);
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
                SetFeedbackPanel("True");
                Invoke(nameof(CloseFeedbackPanel), 3f);
            } else {
                SetFeedbackPanel("False");
                Invoke(nameof(CloseFeedbackPanel), 3f);
            }
        }

        private void Update()
        {
            
        }

        private void RandomOrder()
        {
            var buttons = panelOrder.GetComponentsInChildren<Button>();
            yPositions = new float[buttons.Length];
            for (var i = 0; i < buttons.Length; i++)
            {
                var temp = buttons[i].transform.position;
                var randomIndex = UnityEngine.Random.Range(i, buttons.Length);
                buttons[i].transform.position = buttons[randomIndex].transform.position;
                buttons[randomIndex].transform.position = temp;
                yPositions[i] = buttons[i].transform.position.y;
            }
            Array.Sort(yPositions);
        }
        private void Start()
        {
            RandomOrder();
        }
    }
}

