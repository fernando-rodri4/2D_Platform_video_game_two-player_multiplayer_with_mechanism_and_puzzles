using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObject : MonoBehaviour
{

    /// <summary>
    /// Reference to the PressStud script that control the platform.
    /// </summary>
    [SerializeField] Buttons button = null;
    [SerializeField] int sumPos = 0;
    
    Vector3 initialPos;

    void Start()
    {
        if (button == null)
        {
            Debug.LogError("Error with ElevatedPlatform script component " + this);
            Destroy(this);
            return;
        }

        // Get the initial position.
        initialPos = transform.position;
    }

    /// <summary>
    // Platform rises or falls depending on the status of the controller button.
    /// </summary>
    void FixedUpdate()
    {

        bool isButtonActivate = button.IsButtonActivate();

        if(isButtonActivate)
        {

            AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();

            Vector3 newPosition = transform.position;
            newPosition.y += sumPos;
            transform.position = newPosition;
        }
        else if(!isButtonActivate && transform.position != initialPos)
        {

            AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();
            Vector3 newPosition = transform.position;
            newPosition.y -= sumPos;
            transform.position = newPosition;     
        }
        else{

            return;
        }
    }
}
