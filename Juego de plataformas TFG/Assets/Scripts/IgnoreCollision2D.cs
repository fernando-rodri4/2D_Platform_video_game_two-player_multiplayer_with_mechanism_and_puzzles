using UnityEngine;

public class IgnoreCollision2D : MonoBehaviour
{
    /// <summary>
    /// Reference to the colliders that we want to be ignored.
    /// </summary>
    [SerializeField] Collider2D collider_ = null, otherCollider = null;

    void Start()
    {
        if(collider_ == null || otherCollider == null)
        {
            Debug.LogError("Error with IgnoreCollision2D script component " + this);
            Destroy(this);
            return;
        }

        Physics2D.IgnoreCollision(collider_, otherCollider);
    }
}
