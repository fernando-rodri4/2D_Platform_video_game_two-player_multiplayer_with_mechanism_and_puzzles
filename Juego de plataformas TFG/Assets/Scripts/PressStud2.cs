using UnityEngine;

public class PressStud2 : Buttons
{
    /// <summary>
    /// When it collides with the button collider, if it is the first collision the button is activated and the sprite is changed if not, a counter is increased.
    /// </summary>
    /// <param name="col">The col Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isActivate)
        {
            spRen.enabled = false;

            isActivate = true;
        }
        else
        {
            extraInside++;
        }
    }

    /// <summary>
    /// When the button collider stops colliding, if it is the last collision the button is deactivated and the sprite is changed if not, a counter is decreased.
    /// </summary>
    /// <param name="col">The col Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D col)
    {
        if (extraInside == 0)
        {
            spRen.enabled = true;

            isActivate = false;
        }
        else
        {
            extraInside--;
        }
    }
}