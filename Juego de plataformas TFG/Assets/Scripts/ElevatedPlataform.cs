using UnityEngine;

public class ElevatedPlataform : MonoBehaviour
{
    /// <summary>
    /// Reference to the PressStud script that control the platform.
    /// </summary>
    [SerializeField] PressStud button = null;

    [SerializeField] float finalPos = 0;
    float initialPos;

    void Start()
    {
        if (button == null)
        {
            Debug.LogError("Error with ElevatedPlatform script component " + this);
            Destroy(this);
            return;
        }

        // Get the initial position.
        initialPos = transform.position.y;
    }

    /// <summary>
    // Platform rises or falls depending on the status of the controller button.
    /// </summary>
    void FixedUpdate()
    {
        bool isButtonActivate = button.IsButtonActivate();

        if(!isButtonActivate && transform.position.y == initialPos)
        {
            return;
        }

        if (isButtonActivate && transform.position.y < finalPos)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.03125f, transform.position.z);
        }
        else if(isButtonActivate && transform.position.y > finalPos)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.03125f, transform.position.z);
        }
        else if(!isButtonActivate && transform.position.y > initialPos)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.03125f, transform.position.z);
        }
        else if(!isButtonActivate && transform.position.y < initialPos)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.03125f, transform.position.z);
        }
    }
}