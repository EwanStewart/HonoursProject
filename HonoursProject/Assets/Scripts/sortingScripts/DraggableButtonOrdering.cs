using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace sortingScripts
{
    public class DraggableButtonOrdering : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public RectTransform panelOrder;            //panel that holds all buttons
        [HideInInspector]
        public Vector3 lastPosition;                //Holds the last position of the button
        
        public void OnDrag(PointerEventData eventData) //as button is being dragged change position to mouse/touch position
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(panelOrder, Input.mousePosition)) //if button is dragged outside of panel, return to original position
            {
                transform.position = lastPosition;
                return;
            }
            transform.position = eventData.position; //change position to mouse/touch position
        }

        private void Update()
        {
            if (transform.localPosition.x != 0) //ensure button is always in center of panel
            {
                var buttonTransformOfDraggedButton = transform;    
                var localPositionOfDraggedButton = buttonTransformOfDraggedButton.localPosition;
                
                localPositionOfDraggedButton = new Vector3(0, localPositionOfDraggedButton.y, localPositionOfDraggedButton.z);
                buttonTransformOfDraggedButton.localPosition = localPositionOfDraggedButton;
            }
            var yPositions = NotationOrder.YPositions;             //get the y positions of all buttons in the panel from NotationOrder.cs
            if (Array.IndexOf(yPositions, transform.position.y) == -1)  //if button is not in a valid y position, return to original position
            {
                transform.position = lastPosition; 
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            var buttons = panelOrder.GetComponentsInChildren<Button>(); //get all buttons in panel
            foreach (var button in buttons)
            {
                if (button.name == transform.name) continue;    //skip the button being dragged
                
                if (!RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(),    //check if button being dragged is colliding with another button
                        Input.mousePosition)) continue;
                
                var transformOfDraggedButton = transform;   
                var positionOfDraggedButton = transformOfDraggedButton.position;
                var yOfDraggedButton = positionOfDraggedButton.y;
                
                var transformOfCollidingButton = button.transform;
                var positionOfCollidingButton = transformOfCollidingButton.position;
                
                positionOfDraggedButton = new Vector3(positionOfDraggedButton.x, positionOfCollidingButton.y, positionOfDraggedButton.z);   //swap the y positions of the buttons
                transformOfDraggedButton.position = positionOfDraggedButton;
                
                positionOfCollidingButton = new Vector3(positionOfCollidingButton.x, yOfDraggedButton, positionOfCollidingButton.z);        //swap the y positions of the buttons
                transformOfCollidingButton.position = positionOfCollidingButton;
                
                lastPosition = positionOfDraggedButton;                                                    //update the last position of the button being dragged
                button.GetComponent<DraggableButtonOrdering>().lastPosition = positionOfCollidingButton;   //update the last position of the button being collided with
                return;
            }
            transform.position = lastPosition; //if no collision, return to original position
        }

        private void Start()
        {
            lastPosition = transform.position; //set the last position to the starting position
        }
    }

}
