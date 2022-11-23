using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public SpriteRenderer wireEnd;
    Vector3 startPoint;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.parent.position;
        Debug.Log(startPoint);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDrag()
    {
        //moves
        Vector3 newPosition = Input.mousePosition;
        newPosition.z = 5;
        newPosition = Camera.main.ScreenToWorldPoint(newPosition);
        transform.position = newPosition;
        

        //sets direction
        Vector3 direction = newPosition - startPoint;
        transform.right = direction * transform.lossyScale.x;



        //scale it
        float dist = Vector2.Distance(startPoint, newPosition);
        wireEnd.size = new Vector2(dist, wireEnd.size.y);
    }
}
