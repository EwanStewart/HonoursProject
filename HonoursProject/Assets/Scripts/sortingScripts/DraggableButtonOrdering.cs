using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace sortingScripts
{
    public class DraggableButtonOrdering : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public RectTransform panelOrder; //panel to hold all buttons
        [HideInInspector]
        public Vector3 _lastPosition; // The original position in center panel
        
        public void OnDrag(PointerEventData eventData) //as button is being dragged change position to mouse/touch position
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(panelOrder, Input.mousePosition))
            {
                transform.position = _lastPosition;
                return;
            }
            transform.position = eventData.position;
        }

        private void Update()
        {
            if (transform.localPosition.x != 0)
            {
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            }
            float []yPositions = NotationOrder.yPositions;
            if (Array.IndexOf(yPositions, transform.position.y) == -1)
            {
                transform.position = _lastPosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            var buttons = panelOrder.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                if (button == this) continue;
                if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), Input.mousePosition))
                {
                    var temp = transform.position.y;
                    transform.position = new Vector3(transform.position.x, button.transform.position.y, transform.position.z);
                    button.transform.position = new Vector3(button.transform.position.x, temp, button.transform.position.z);
                    

                    _lastPosition = transform.position;
                    button.GetComponent<DraggableButtonOrdering>()._lastPosition = button.transform.position;
                    return;
                }
            }
            transform.position = _lastPosition;
        }

        private void Start()
        {
            _lastPosition = transform.position;
        }
    }

}
