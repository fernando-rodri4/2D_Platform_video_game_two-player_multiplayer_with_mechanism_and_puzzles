using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatedPlataform : MonoBehaviour
{
    /// <summary>
    /// Reference to the PressStud script that control the platform.
    /// </summary>
    public PressStud button;

    /// <summary>
    /// Reference to the Sprite Renderer component.
    /// </summary>
    [SerializeField]
    float finalPos = 0;
    float initialPos;

    void Start()
    {
        // Get the initial position
        initialPos = transform.position.y;
    }

    // Platform rises or falls depending on the status of the controller button
    void Update()
    {
        if(button.isButtonActivate() && transform.position.y < finalPos)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.03f, transform.position.z);
        }
        else if(!button.isButtonActivate() && transform.position.y > initialPos)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z);
        }
    }
}
