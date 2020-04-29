using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pieces : NetworkBehaviour
{

    private Vector2 initialPosition;
    private Vector2 mousePosition;
    [SerializeField] private Transform rightPosition;
    public float deltaX, deltaY;
    public bool locked;


    void Start()
    {
        initialPosition = transform.position;
    }

    public bool isLocked ()
    {
        return locked;
    }

    public bool isCorrect ()
    {
        if(Mathf.Abs(transform.position.x - rightPosition.position.x) < 0.5f &&
           Mathf.Abs(transform.position.y - rightPosition.position.y) < 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
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
        if(hasAuthority && !locked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
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
