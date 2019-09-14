﻿using UnityEngine;

public class PressStud : MonoBehaviour
{
    /// <summary>
    /// Reference to the Sprite Renderer component.
    /// </summary>
    public SpriteRenderer spRen;

    /// <summary>
    /// Reference to the sprites that change.
    /// </summary>
    public Sprite sprite1, sprite2;

    /// <summary>
    /// Indicates if the button is pressed or not.
    /// </summary>
    private bool isActivate = false;

    // When something enters the collider the sprite is changed and notify the platform that controls the change.
    void OnTriggerEnter2D(Collider2D col)
    {
        spRen.sprite = sprite2;

        isActivate = true;
    }

    // When something exits the collider the sprite is changed and notify the platform that controls the change.
    void OnTriggerExit2D(Collider2D col)
    {
        spRen.sprite = sprite1;
        
        isActivate = false;
    }

    public bool isButtonActivate()
    {
        return isActivate;
    }
}
