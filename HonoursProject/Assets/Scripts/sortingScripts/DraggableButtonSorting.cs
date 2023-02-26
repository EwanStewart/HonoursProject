using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace sortingScripts
{
    public class DraggableButtonSorting : MonoBehaviour, IDragHandler, IEndDragHandler 
    {
        private Vector3 _originalPosition; // The original position in center panel
        private Transform _startParent;
        public RectTransform panelAnswers; //panel to hold all buttons
        public RectTransform panelNotation; //panel to hold gaps
        
        public void OnDrag(PointerEventData eventData) //as button is being dragged change position to mouse/touch position
        
        {
            transform.position = eventData.position;
        }
    
        public void OnEndDrag(PointerEventData eventData) //when button is released, check if it is in either panel, if not reset to original position
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(panelAnswers, Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(panelNotation, Input.mousePosition))  //if not in either panel, reset to original position
            {
                if (transform.parent == panelNotation || transform.parent == panelAnswers) {  //if out of bounds of panels, reset parent to original parent
                    transform.SetParent(_startParent);
                }
                transform.position = _originalPosition; //reset to original position
            }
        
            if (RectTransformUtility.RectangleContainsScreenPoint(panelAnswers, Input.mousePosition))  //if in panelAnswers, set parent to panelAnswers
            {
                transform.SetParent(panelAnswers);
            } else if (RectTransformUtility.RectangleContainsScreenPoint(panelNotation, Input.mousePosition)) {  //if in panelNotation, set parent to panelNotation
                Button[] buttons = panelNotation.GetComponentsInChildren<Button>();
                foreach (var button in buttons)
                {
                    if (!button.CompareTag("question")) continue;
                    if (!RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(),
                            Input.mousePosition)) continue;
                    Transform transform1;
                    (transform1 = transform).SetParent(button.transform);
                    var position = button.transform.position;
                    transform1.position = position; //set position of button to position of button it is in
                    _originalPosition = position;
                    return;
                }
                transform.position = _originalPosition;
            }
        }
        void Start() 
        {
            var transform1 = transform;
            _startParent = transform1.parent; //set start parent to parent of button
            _originalPosition = transform1.position; //set original position to starting point for button
        }
    }
}
