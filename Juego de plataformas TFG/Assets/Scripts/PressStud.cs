using UnityEngine;

public class PressStud : MonoBehaviour
{
    /// <summary>
    /// Reference to the Sprite Renderer component.
    /// </summary>
    public SpriteRenderer spRen;

    /// <summary>
    /// Reference to the ElevatedPlataform script that button control.
    /// </summary>
    public ElevatedPlataform[] plataforms;

    /// <summary>
    /// Reference to the sprites that change.
    /// </summary>
    public Sprite sprite1, sprite2;

    // When something enters the collider the sprite is changed and notify the platform that controls the change.
    void OnTriggerEnter2D(Collider2D col)
    {
        spRen.sprite = sprite2;

        foreach (var plataform in plataforms)
        {
            plataform.isUp = true;
        }
    }

    // When something exits the collider the sprite is changed and notify the platform that controls the change.
    void OnTriggerExit2D(Collider2D col)
    {
        spRen.sprite = sprite1;
        
        foreach (var plataform in plataforms)
        {
            plataform.isUp = false;
        }
    }
}
