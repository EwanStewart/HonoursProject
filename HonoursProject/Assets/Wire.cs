using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private Transform startP;
    [SerializeField] private Transform AttachPoint;
    [SerializeField] private bool receiver;

    public SpriteRenderer wireEnd;
    public GameObject lightOn;
    Vector3 startPoint;
    Vector3 startPosition;


    // Start is called before the first frame update
    void Start()
    {
        startPoint = startP.position;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDrag()
    {
        if (!receiver)
        { //moves
            Vector3 newPosition = Input.mousePosition;
            newPosition.z = 5;
            newPosition = Camera.main.ScreenToWorldPoint(newPosition);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, .2f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    Debug.Log(collider.transform.position);
                    updateWire(collider.GetComponent<Wire>().GetAttachPoint());

                    if (transform.parent.name.Equals(collider.transform.parent.name))
                    {

                        collider.GetComponent<Wire>()?.Done();
                        Done();
                    }
                    return;
                }
            }
            updateWire(newPosition);
        }
    }

    void Done()
    {
        lightOn.SetActive(true);
        Destroy(this);
    }

    private void OnMouseUp()
    {
        if (!receiver)
        {
            wireEnd.size = new Vector2(2f, wireEnd.size.y);

            updateWire(startPosition);
        }
    }

    void updateWire (Vector3 newPosition)
    {
        transform.position = newPosition;

        //sets direction
        Vector3 direction = newPosition - startPoint;
        transform.right = direction * transform.lossyScale.x;

        //scale it
        float dist = Vector2.Distance(startPoint, newPosition);
        wireEnd.size = new Vector2(dist, wireEnd.size.y);
    }

    public Vector3 GetAttachPoint()
    {
        return AttachPoint.position;
    }
}
