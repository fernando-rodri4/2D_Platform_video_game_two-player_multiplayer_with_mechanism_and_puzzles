using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    int playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If the collision wasn't with the players, exit
        if (collision.gameObject.layer != playerLayer)
        {
            return;
        }
        LevelManager.Instance.PlayerWin();
    }
}
