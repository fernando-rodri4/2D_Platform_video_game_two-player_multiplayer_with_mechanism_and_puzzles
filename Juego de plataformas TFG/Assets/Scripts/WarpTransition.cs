using System.Collections;
using UnityEngine;

public class WarpTransition : MonoBehaviour
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
    GameObject player;

    void Awake()
    {
        // Get reference to the animator component.
        cameraTrans = GetComponent<CameraTransition>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Cambiar para permitir transportar a los dos personajes cuando cada uno le de a enter");

        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Enter") && player != null)
        {
            player.GetComponent<PlayerMovement>().canMove = false;

            StartCoroutine(TransportPlayer());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //If the collider object isn't on the Player layer, exit. This is more 
        //efficient than string comparisons using Tags
        if (collision.gameObject.layer == playerLayer)
        {
            player = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //If the collider object isn't on the Player layer, exit. This is more 
        //efficient than string comparisons using Tags
        if (collision.gameObject.layer == playerLayer)
        {
            player = null;
        }
    }

    /// <summary>
    /// Transport the player from one point to another with a camera animation.
    /// </summary>
    IEnumerator TransportPlayer()
    {
        cameraTrans.FadeIn();

        yield return new WaitForSeconds(cameraTrans.GetFadeTime());

        // Transport the player to the exit.
        player.transform.position = transform.GetChild(0).transform.position;

        if (numberTransport == 0)
        {
            gameObject.SetActive(false);
        }

        cameraTrans.FadeOut();

        player.GetComponent<PlayerMovement>().canMove = true;
    }
}
