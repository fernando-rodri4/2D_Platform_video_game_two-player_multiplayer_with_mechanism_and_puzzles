using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class IgnoreCollision2D : MonoBehaviour
{
    /// <summary>
    /// Reference to the colliders that we want to be ignored.
    /// </summary>
    [SerializeField] Collider2D collider_ = null;
    [SerializeField] List<Collider2D> otherColliders = null;

    void Awake()
    {
        //Create out collection to hold the thieves
        otherColliders = new List<Collider2D>();
    }

    void Update()
    {
        if (otherColliders.Count == 0)
        {
            var players = ClientScene.localPlayers;

            foreach (var player in players)
            {
                if (player.gameObject.GetComponent<Collider2D>() != collider_)
                {
                    otherColliders.Add(player.gameObject.GetComponent<Collider2D>());
                }
            }

            foreach (var otherCollider in otherColliders)
            {
                Physics2D.IgnoreCollision(collider_, otherCollider);
            }
        }
    }
}
