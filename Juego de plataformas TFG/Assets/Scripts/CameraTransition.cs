using UnityEngine;

    /// <summary>
    /// The script is based on the video of Hektor Profe in Youtube.
    /// </summary>
public class CameraTransition : MonoBehaviour
{
    /// <summary>
    /// To control whether or not the transition begins.
    /// </summary>
    bool start = false;

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
    float fadeTime = 1f;

    /// <summary>
    // Draw a square with opacity on the screen simulating a transition.
    /// </summary>
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

    /// <summary>
    // Method to activate the input transition.
    /// </summary>
    public void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    /// <summary>
    // Method to activate the output transition.
    /// </summary>
    public void FadeOut()
    {
        isFadeIn = false;
    }

    public bool isStart()
    {
        return start;
    }

    public float getFadeTime()
    {
        return fadeTime;
    }
}