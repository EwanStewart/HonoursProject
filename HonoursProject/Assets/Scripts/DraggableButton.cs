using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableButton : MonoBehaviour, IDragHandler, IEndDragHandler 
{
    private Vector3 _originalPosition; // The original position in center panel
    private Transform _startParent;
    public RectTransform panelTrue; //Panel object for true statements
    public RectTransform panelFalse; //Panel object for false statements
    public void OnDrag(PointerEventData eventData) //as button is being dragged change position to mouse/touch position
    {
        transform.position = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData) //when button is released, check if it is in either panel, if not reset to original position
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(panelTrue, Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(panelFalse, Input.mousePosition))  //if not in either panel, reset to original position
        {
            if (transform.parent == panelTrue || transform.parent == panelFalse) {  //if outofbounds of panels, reset parent to original parent
                transform.SetParent(_startParent);
            }
            transform.position = _originalPosition; //reset to original position
        }
        
        if (RectTransformUtility.RectangleContainsScreenPoint(panelTrue, Input.mousePosition)) //if in true panel, set parent of button to true panel
        {
            transform.SetParent(panelTrue);
        } else if (RectTransformUtility.RectangleContainsScreenPoint(panelFalse, Input.mousePosition)) { //if in false panel, set parent of button to false panel
            transform.SetParent(panelFalse);
        }
    }
    void Start() 
    {
        _startParent = transform.parent; //set start parent to parent of button
        _originalPosition = transform.position; //set original position to starting point for button
    }
}