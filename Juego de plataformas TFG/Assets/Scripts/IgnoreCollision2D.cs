using UnityEngine;

public class IgnoreCollision2D : MonoBehaviour
{
    /// <summary>
    /// Reference to the collider of both players.
    /// </summary>
    public Collider2D collider_, otherCollider;

    void Start()
    {
        if(collider_ == null || otherCollider == null)
        {
            Destroy(this);
            Debug.LogError("Error with IgnoreCollision2D script component " + this);
        }
    }

    void Update()
    {
        Physics2D.IgnoreCollision(collider_, otherCollider);
    }
}
