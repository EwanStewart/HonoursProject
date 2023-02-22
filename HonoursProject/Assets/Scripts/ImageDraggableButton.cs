using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageDraggableButton : MonoBehaviour, IDragHandler, IEndDragHandler 
{
    private Vector3 originalPosition; // The original position in center panel
    private Transform startParent;
    public RectTransform questionsPanel;
    public void OnDrag(PointerEventData eventData) //as button is being dragged change position to mouse/touch position
    {
        if (tag != "question")
        {
            transform.position = eventData.position;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData) //when button is released, check if it is in either panel, if not reset to original position
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(questionsPanel, eventData.position))
        {
            transform.position = originalPosition;
            transform.SetParent(startParent);
            return;
        }
        
        Button[] buttons = questionsPanel.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), eventData.position))
            {
                if (button.tag == "answer")
                {
                    transform.position = originalPosition;
                    transform.SetParent(startParent);
                    return;
                }
                transform.SetParent(questionsPanel);
                transform.position = button.transform.position; //set position of button to position of button it is in
                transform.name = button.transform.position.y.ToString();
                button.gameObject.SetActive(false);
                buttons = questionsPanel.GetComponentsInChildren<Button>();
                
                return;
            }
        }
    }
    void Start() 
    {
        startParent = transform.parent; //set start parent to parent of button
        originalPosition = transform.position; //set original position to starting point for button
    }
}