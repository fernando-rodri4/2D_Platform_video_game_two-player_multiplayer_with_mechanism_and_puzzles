using UnityEngine;
using System.Collections.Generic;

public class IgnoreCollision2D : MonoBehaviour
{
    /// <summary>
    /// Reference to the colliders that we want to be ignored.
    /// </summary>
    [SerializeField] Collider2D collider_ = null;
    [SerializeField] List<Collider2D> otherColliders = null;

    bool isCollisionIgnored = false;

    void Awake()
    {
        if (otherColliders.Count == 0)
        {
            otherColliders = new List<Collider2D>();
        }
    }
    void Start()
    {
        if (otherColliders.Count != 0)
        {
            foreach (var otherCollider in otherColliders)
            {
                Physics2D.IgnoreCollision(collider_, otherCollider);
            }

            isCollisionIgnored = true;
        }
    }

    void Update()
    {

        if (!isCollisionIgnored)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length == 2)
            {
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

                isCollisionIgnored = true;
            }
        } 
    }
}
