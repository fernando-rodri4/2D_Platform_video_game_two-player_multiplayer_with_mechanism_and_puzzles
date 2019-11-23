using UnityEngine;

public class RestartObject : MonoBehaviour
{
    /// <summary>
    /// Reference the respawn position.
    /// </summary>
    [SerializeField] GameObject respawn = null;

    /// <summary>
    /// Reference to the tag to collider.
    /// </summary>
    [SerializeField] string _tag = "";

    void Start()
    {
        if (respawn == null || _tag == "")
        {
            Destroy(this);
            Debug.LogError("Error with RestartObject script components " + this);
            return;
        }
    }

    /// <summary>
    /// Change the other position to the respawn position
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(_tag))
        {
            other.transform.position = respawn.transform.position;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
    }
}