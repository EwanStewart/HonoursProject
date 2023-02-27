using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace sortingScripts
{
    public class DraggableButtonSorting : MonoBehaviour, IDragHandler, IEndDragHandler 
    {
        private Vector3 _lastPosition; // The original position in center panel
        private Vector3 _firstPosition; // The original position in center panel
        private Transform _startParent;
        public RectTransform panelAnswers; //panel to hold all buttons
        public RectTransform panelNotation; //panel to hold gaps
        
        public void OnDrag(PointerEventData eventData) //as button is being dragged change position to mouse/touch position
        
        {
            transform.position = eventData.position;
        }

        private bool OutOfBounds()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(panelAnswers, Input.mousePosition) ||
                RectTransformUtility.RectangleContainsScreenPoint(panelNotation, Input.mousePosition)) return false; 
            if (transform.parent == panelNotation || transform.parent == panelAnswers) {  
                transform.SetParent(_startParent);
            }
            transform.position = _lastPosition;
            return true;
        }

        private void CheckBeneath()
        {
            string[] buttonNames = { "ConstantGapButton", "LogarithmicGapButton", "LinearGapButton", "QuadraticGapButton", "CubicGapButton" };
            foreach (var buttonName in buttonNames)
            {
                var button = GameObject.Find(buttonName);
                var buttonComponent = button.GetComponent<Button>();
                var children = button.GetComponentsInChildren<Button>();
                if (children.Count(child => child.CompareTag("answer")) > 1)
                {
                    var firstChild = children.First(child => child.CompareTag("answer"));
                    firstChild.transform.SetParent(panelAnswers);
                    var position = firstChild.transform.position;
                    firstChild.transform.position = _firstPosition;
                    _lastPosition = position;
                }
            }

 

        }
        private void CheckForMovement()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(panelAnswers, Input.mousePosition))  //if in panelAnswers, set parent to panelAnswers
            {
                transform.SetParent(panelAnswers);
            } else if (RectTransformUtility.RectangleContainsScreenPoint(panelNotation, Input.mousePosition)) {  //if in panelNotation, set parent to panelNotation
                var buttons = panelNotation.GetComponentsInChildren<Button>();
                foreach (var button in buttons)
                {
                    if (!button.CompareTag("question")) continue;
                    if (!RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(),
                            Input.mousePosition)) continue;
                    Transform transform1;
                    (transform1 = transform).SetParent(button.transform);
                    var position = button.transform.position;
                    transform1.position = position; //set position of button to position of button it is in
                    _lastPosition = position;
                    CheckBeneath();
                    return;
                }
                transform.position = _lastPosition;
            }
        }
        
        public void OnEndDrag(PointerEventData eventData) //when button is released, check if it is in either panel, if not reset to original position
        {
            if (!OutOfBounds())
            {
                CheckForMovement();
            }
            
        }
        void Start() 
        {
            var transform1 = transform;
            _startParent = transform1.parent; //set start parent to parent of button
            _lastPosition = transform1.position; //set original position to starting point for button
            _firstPosition = transform1.position;
        }
    }
}
