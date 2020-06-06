using UnityEngine;

public class Platform : MonoBehaviour
{
    /// <summary>
    /// Reference to the PressStud script that control the platform.
    /// </summary>
    [SerializeField] Buttons button = null;

    [SerializeField] float finalPos = 0;
    float initialPos;

    void Start()
    {
        if (button == null)
        {
            Debug.LogError("Error with Platform script component " + this);
            Destroy(this);
            return;
        }

        // Get the initial position.
        initialPos = transform.position.x;
    }

    /// <summary>
    // Platform rises or falls depending on the status of the controller button.
    /// </summary>
    void FixedUpdate()
    {

        bool isButtonActivate = button.IsButtonActivate();

        if (!isButtonActivate && transform.position.x == initialPos)
        {
            return;
        }

        if (isButtonActivate && transform.position.x < finalPos)
        {
            transform.position = new Vector3(transform.position.x + 0.03125f, transform.position.y, transform.position.z);
        }
        else if (isButtonActivate && transform.position.x > finalPos)
        {
            transform.position = new Vector3(transform.position.x - 0.03125f, transform.position.y, transform.position.z);

        }
        else if (!isButtonActivate && transform.position.x > initialPos)
        {
            transform.position = new Vector3(transform.position.x - 0.05f, transform.position.y, transform.position.z);
        }
        else if (!isButtonActivate && transform.position.x < initialPos)
        {
            transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y, transform.position.z);

        }
    }
}