using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;



namespace sortingScripts
{
    public class NotationDrag : MonoBehaviour
    {
        public RectTransform panelNotation; 
        private int correctCount = 0;
        private int incorrectCount = 0;
        public void OnSubmit()
        {
            string[] buttonNames = new string[] { "ConstantButton", "LogarithmicButton", "LinearButton", "QuadraticButton", "CubicButton" };
            
            foreach (string buttonName in buttonNames)
            {
                GameObject button = GameObject.Find(buttonName);
                Button buttonComponent = button.GetComponent<Button>();
                Transform parent = button.transform.parent;
                string[] splitName = Regex.Split(buttonComponent.name, @"(?<!^)(?=[A-Z])");
                string[] splitParent = Regex.Split(parent.name, @"(?<!^)(?=[A-Z])");
                var equals = splitName[0].Equals(splitParent[0]);
                if (!equals)
                {
                    incorrectCount++;
                }
                else
                {
                    correctCount++;
                }
            }
            Debug.Log("Correct: " + correctCount + " Incorrect: " + incorrectCount);






        }
    }
}
