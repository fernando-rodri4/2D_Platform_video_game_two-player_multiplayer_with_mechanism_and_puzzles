using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartObject : MonoBehaviour
{
    /// <summary>
    /// Reference the respawn position.
    /// </summary>
    public GameObject respawn;

    /// <summary>
    /// Reference to the tag to collide.
    /// </summary>
    public string tag_ = "";

    // Change the other position to the respawn position
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == tag_)
        {
            other.transform.position = respawn.transform.position;
        }
    }
    
}
