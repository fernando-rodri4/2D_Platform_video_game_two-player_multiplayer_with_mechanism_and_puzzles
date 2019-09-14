using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartObject : MonoBehaviour
{
    public GameObject respawn;
    public string tag = "";

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == tag)
        {
            other.transform.position = respawn.transform.position;
        }
    }
    
}
