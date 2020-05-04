using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pieces : NetworkBehaviour
{

    private Vector2 initialPosition;    //Initial position of the piece
    private Vector2 mousePosition;  //Mouse position
    [SerializeField] private Transform rightPosition;   //Right position of the piece
    public float deltaX, deltaY;
    public bool locked; //If the piece is locked or not


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        //Set the initial position of the piece
        initialPosition = transform.position;
    }

    /// <summary>
    /// Return if the piece is locked
    /// </summary>
    public bool isLocked ()
    {
        return locked;
    }

    /// <summary>
    /// Returns if the piece's position is right
    /// </summary>
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

    /// <summary>
    /// Mouse is pressed down
    /// </summary>
    private void OnMouseDown() 
    {   
        if(!locked)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }
    }

    /// <summary>
    /// if has authority and is not locked, the piece will be moved
    /// </summary>
    private void OnMouseDrag() 
    {        
        if(hasAuthority && !locked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
        }
        
    }

    /// <summary>
    /// Mouse is pressed up
    /// </summary>
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
