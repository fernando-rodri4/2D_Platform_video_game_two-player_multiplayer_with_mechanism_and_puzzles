using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WarpTransition : NetworkBehaviour
{
    /// <summary>
    /// Reference to the CameraTransition.
    /// </summary>
    CameraTransition cameraTrans;

    /// <summary>
    /// Indicates if Warp Point should be deactivated, if numberTransport = -1 infinite transport
    /// </summary>
    [SerializeField] int numberTransport = -1;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    int playerLayer;

    /// <summary>
    /// Player that collider with warp point
    /// </summary>
    GameObject player0, player1;

    void Awake()
    {
        // Get reference to the animator component.
        cameraTrans = GetComponent<CameraTransition>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Enter"))
        {
            if (player0 != null)
            {
                player0.GetComponent<PlayerMovement>().canMove = false;

                if (numberTransport == 0)
                {
                    gameObject.SetActive(false);
                }

                StartCoroutine(TransportPlayer(player0));

                if(player0.GetComponent<PlayerMovement>().GetIsServer())
                {
                    RpcSubtractTransport();
                }
                else
                {
                    CmdSubtractTransport();
                }
            }
            else if(player1 != null)
            {
                player1.GetComponent<PlayerMovement>().canMove = false;

                if (numberTransport == 0)
                {
                    gameObject.SetActive(false);
                }

                StartCoroutine(TransportPlayer(player1));

                if (player1.GetComponent<PlayerMovement>().GetIsServer())
                {
                    RpcSubtractTransport();
                }
                else
                {
                    CmdSubtractTransport();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //If the collider object isn't on the Player layer, exit. This is more 
        //efficient than string comparisons using Tags
        if (collision.gameObject.layer == playerLayer && collision.gameObject.GetComponent<PlayerMovement>().isLocalPlayer)
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().GetId() == 0)
            {
                player0 = collision.gameObject;
            }
            else
            {
                player1 = collision.gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //If the collider object isn't on the Player layer, exit. This is more 
        //efficient than string comparisons using Tags
        if (collision.gameObject.layer == playerLayer && collision.gameObject.GetComponent<PlayerMovement>().isLocalPlayer)
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().GetId() == 0)
            {
                player0 = null;
            }
            else
            {
                player1 = null;
            }
        }
    }

    /// <summary>
    /// Transport the player from one point to another with a camera animation.
    /// </summary>
    IEnumerator TransportPlayer(GameObject player)
    {
        cameraTrans.FadeIn();

        yield return new WaitForSeconds(cameraTrans.GetFadeTime());

        // Transport the player to the exit.
        player.transform.position = transform.GetChild(0).transform.position;

        cameraTrans.FadeOut();

        player.GetComponent<PlayerMovement>().canMove = true;
    }

    void SubtractTransport ()
    {
        numberTransport--;
    }

    [ClientRpc]
    void RpcSubtractTransport()
    {
        SubtractTransport();
    }

    [Command]
    void CmdSubtractTransport()
    {
        RpcSubtractTransport();
    }
}
