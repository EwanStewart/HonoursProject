using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageDraggableButton : MonoBehaviour, IDragHandler, IEndDragHandler 
{
    private Vector3 _originalPosition; // The original position in center panel
    private Transform _startParent;
    public RectTransform questionsPanel;
    public void OnDrag(PointerEventData eventData) //as button is being dragged change position to mouse/touch position
    {
        if (!CompareTag("question"))
        {
            transform.position = eventData.position;
        }

        if (!CompareTag("answer")) return;
        if (!RectTransformUtility.RectangleContainsScreenPoint(questionsPanel, eventData.position)) return;
        if (transform.parent == questionsPanel)
        {
            transform.position = _originalPosition;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData) //when button is released, check if it is in either panel, if not reset to original position
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(questionsPanel, eventData.position))
        {
            transform.position = _originalPosition;
            transform.SetParent(_startParent);
            return;
        }
        
        Button[] buttons = questionsPanel.GetComponentsInChildren<Button>();
        
        
        foreach (Button button in buttons)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(),
                    eventData.position)) continue;
            if (button.CompareTag("answer"))
            {
                transform.position = _originalPosition;
                transform.SetParent(_startParent);
                return;
            }

            Transform transform1;
            (transform1 = transform).SetParent(questionsPanel);
            var transform2 = button.transform;
            var position = transform2.position;
            transform1.position = position; //set position of button to position of button it is in
            _originalPosition = position;
            transform.name = button.transform.position.y.ToString();
            button.gameObject.SetActive(false);
            buttons = questionsPanel.GetComponentsInChildren<Button>();
            return;
        }
    }
    void Start() 
    {
        var transform1 = transform;
        _startParent = transform1.parent; //set start parent to parent of button
        _originalPosition = transform1.position; //set original position to starting point for button
    }
}