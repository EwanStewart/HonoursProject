using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    public RectTransform panelTrue;
    public RectTransform panelFalse;
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(panelTrue, Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(panelFalse, Input.mousePosition))
        {
            transform.position = originalPosition;
        }
        
        if (RectTransformUtility.RectangleContainsScreenPoint(panelTrue, Input.mousePosition))
        {
            transform.SetParent(panelTrue);
        } else if (RectTransformUtility.RectangleContainsScreenPoint(panelFalse, Input.mousePosition))
        {
            transform.SetParent(panelFalse);
        }
        
    }
    
    void Start() 
    {
        originalPosition = transform.position;
    }
    
    
}