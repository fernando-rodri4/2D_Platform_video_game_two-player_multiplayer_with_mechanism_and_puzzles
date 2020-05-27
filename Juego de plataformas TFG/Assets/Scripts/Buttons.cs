using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    /// <summary>
    /// Reference to the Sprite Renderer component.
    /// </summary>
    protected SpriteRenderer spRen;

    /// <summary>
    /// Indicates if the button is pressed or not.
    /// </summary>
    protected bool isActivate = false;

    /// <summary>
    /// How many elements collide with the button, minus 1.
    /// </summary>
    protected int extraInside = 0;

    void Awake()
    {
        // Get reference to the SpriteRenderer.
        spRen = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected void Start()
    {
        if (spRen == null)
        {
            Debug.LogError("Error with PressStud script components " + this);
            Destroy(this);
            return;
        }
    }
    public bool IsButtonActivate()
    {
        return isActivate;
    }
}
