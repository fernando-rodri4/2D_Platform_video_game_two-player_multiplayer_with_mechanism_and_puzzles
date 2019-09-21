using UnityEngine;

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
    private int isEnter = 0;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if(spRen == null || sprite1 == null || sprite2 == null)
        {
            Destroy(this);
            Debug.LogError("Error with PressStud script components " + this);
        }
    }

    /// <summary>
    /// When it collides with the button collider, if it is the first collision the button is activated and the sprite is changed if not, a counter is increased.
    /// </summary>
    /// <param name="col">The col Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if(!isActivate)
        {
            spRen.sprite = sprite2;

            isActivate = true;
        }
        else
        {
            isEnter++;
        }
    }

    /// <summary>
    /// When the button collider stops colliding, if it is the last collision the button is deactivated and the sprite is changed if not, a counter is decreased.
    /// </summary>
    /// <param name="col">The col Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D col)
    {
        if(isEnter == 0)
        {
            spRen.sprite = sprite1;
            
            isActivate = false;
        }
        else
        {
            isEnter--;
        }
    }

    public bool isButtonActivate()
    {
        return isActivate;
    }
}