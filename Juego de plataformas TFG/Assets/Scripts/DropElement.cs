using UnityEngine;
using UnityEngine.Networking;

public class DropElement : MonoBehaviour
{
    /// <summary>
    /// Reference to the PlayerMovement script.
    /// </summary>
    PlayerMovement plrMove = null;

    void Awake()
    {
        plrMove = GetComponentInParent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (plrMove == null)
		{
            Debug.LogError("Error with DropElement script component " + this);
            Destroy(this);
            return;
		}
    }

    /// <summary>
    /// Sent when a collider on another object stops touching this
    /// object's collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionExit2D(Collision2D other)
    {
        if (plrMove != null)
        {
            if (GetComponent<NetworkIdentity>() == null || plrMove.GetIsServer())
            {
                plrMove.RpcResetRBObject();
            }
            else
            {
                plrMove.CmdResetRBObject();
            }
        }
    }
}