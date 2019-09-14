using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Cinemachine;

    /// <summary>
    /// The script is based on the video of Hektor Profe in Youtube.
    /// </summary>
public class CameraTransition : MonoBehaviour
{
    /// <summary>
    /// To control whether or not the transition begins.
    /// </summary>
    [HideInInspector]
    public bool start = false;

    /// <summary>
    /// To control whether the transition is inbound or outbound.
    /// </summary>
    bool isFadeIn = false;

    /// <summary>
    /// Initial opacity of the transition square.
    /// </summary>
    float alpha = 0;

    /// <summary>
    /// 1 second transition.
    /// </summary>
    [HideInInspector]
    public float fadeTime = 1f;

    public CinemachineConfiner cineConfi;

    public PolygonCollider2D polygonCollider;

    // Draw a square with opacity on the screen simulating a transition.
    void OnGUI(){

        // If the transition does not start we leave the event directly.
        if(!start){
            return;
        }

        // If it has started, we create a color with an initial opacity of 0.
        GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        // Create a temporary texture to fill the screen.
        Texture2D texture;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.black);
        texture.Apply();

        // Draw the texture over the entire screen.
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);

        // Control transparency
        if(isFadeIn)
        {
            // If it appears, we add opacity.
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }
        else
        {
            // If it is to disappear we subtract opacity.
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);

            // If the opacity reaches 0 we deactivate the transition.
            if(alpha < 0)
            {
                start = false;
            }
        }
    }

    // Method to activate the input transition.
    public void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    // Method to activate the output transition.
    public void FadeOut()
    {
        isFadeIn = false;
    }

    // Change confiner of the camera
    public void ChangeConfiner()
    {
        cineConfi.m_BoundingShape2D = polygonCollider;
        cineConfi.InvalidatePathCache();
    }
}
