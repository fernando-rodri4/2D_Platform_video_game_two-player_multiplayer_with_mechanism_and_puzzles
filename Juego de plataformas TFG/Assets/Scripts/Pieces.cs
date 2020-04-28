using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{

    private Vector2 initialPosition;
    public Vector2 mousePosition;
    public Vector2 actualPosition;
    [SerializeField] private Transform rightPosition;
    private float deltaX, deltaY;
    public static bool locked;

    void Start()
    {
        initialPosition = transform.position;
        actualPosition = transform.position;
    }

    public bool isLocked ()
    {
        return locked;
    }

    private void OnMouseDown() 
    {   
        if(!locked)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }
    }

    private void OnMouseDrag() 
    {        
        if(!locked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
            actualPosition = transform.position;
        }
    }

    private void OnMouseUp()
    {        
        if(Mathf.Abs(transform.position.x - rightPosition.position.x) < 0.5f &&
           Mathf.Abs(transform.position.y - rightPosition.position.y) < 0.5f)
        {
               transform.position = new Vector2(rightPosition.position.x, rightPosition.position.y);
               locked = true;
        }
        else
        {
            transform.position = new Vector2(initialPosition.x, initialPosition.y);
        }
    }
}
