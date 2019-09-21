using UnityEngine;

public class RestartObject : MonoBehaviour
{
    /// <summary>
    /// Reference the respawn position.
    /// </summary>
    public GameObject respawn;

    /// <summary>
    /// Reference to the tag to collider.
    /// </summary>
    public string tag_ = "";

    /// <summary>
    // Change the other position to the respawn position
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == tag_)
        {
            other.transform.position = respawn.transform.position;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
    }
}