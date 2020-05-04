using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    /// <summary>
    /// Reference to CharacterController2D script.
    /// </summary>
    protected CharacterController2D controller = null;

    /// <summary>
    /// Reference to the animator component.
    /// </summary>
    protected Animator animator = null;

    /// <summary>
    /// Player or box GameObject that collides with player.
    /// </summary>
    protected GameObject elementToCarry = null;

    /// <summary>
    /// Element that player carry.
    /// </summary>
    public GameObject carriedElement = null;

    /// <summary>
    /// Indicates if player can move
    /// </summary>
    [HideInInspector] public bool canMove = true;

    /// <summary>
    /// Id that indicate if the player is server or client
    /// </summary>
    [SerializeField] protected int id = 0;

    /// <summary>
    /// Player speed.
    /// </summary>
    protected float horizontalMove = 0f;

    /// <summary>
    /// Check if the player is jumping or not.
    /// </summary>
    protected bool isJump = false;

    /// <summary>
    /// Check if the player is in a position to carry something or not.
    /// </summary>
    public bool isCarry = false;

    /// <summary>
    /// Check if player press carry button
    /// </summary>
    protected bool carryButton = false;

    /// <summary>
    /// Reference to the animator parameters.
    /// </summary>
	protected int speedParamID;
    protected int carryParamID;
    protected int fallParamID;

    /// <summary>
    /// Run speed.
    /// </summary>
    [SerializeField] protected float runSpeed = 40f;

    protected void Awake()
    {
        // Get reference to the animator component.
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected void Start()
    {
        if (controller == null || animator == null)
        {
            Destroy(this);
            Debug.LogError("Error with PlayerMovement script components " + this);
            return;
        }

        // Get the integer hashes of the Animator parameters. This is much more efficient
        // than passing string into the animator.
        speedParamID = Animator.StringToHash("Speed");
        carryParamID = Animator.StringToHash("IsCarry");
        fallParamID = Animator.StringToHash("VelocityY");

        //Set the id of the player
        if (GetComponent<NetworkIdentity>() != null)
        {
            if (GetComponent<NetworkIdentity>().isServer) 
            {
                id = 0;
            }
            else
            {
                id = 1;
            }
        }
    }

    protected void Update()
    {
        if (GetComponent<NetworkIdentity>() != null)
        {
            if (!isLocalPlayer)
            {
                return;
            }
        }

        MovePlayer();

        // If press jump button, activate jumping animation and deactivate carry animation if it is playing.
        if (Input.GetButtonDown("Jump") && canMove)
        {
            isJump = true;

            if (isCarry)
            {
                CarryPosition();
            }
        }

        // If press carry button, call CarryPosition method.
        if (Input.GetButtonDown("Carry") && canMove && horizontalMove == 0 && animator.GetFloat(fallParamID) == 0)
        {
            carryButton = true;

            CarryPosition();
        }

        /*// Deactivate carry animation if it is falling.
        if (isCarry && animator.GetFloat(fallParamID) < -0.01)
        {
            CarryPosition();
        }*/
    }

    protected void FixedUpdate()
    {
        // Call the "Move" function.
        controller.Move(horizontalMove * Time.fixedDeltaTime, isJump);

        isJump = false;
    }

    /// <summary>
    /// Select the gameObject to carry
    /// </summary>
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box") || other.CompareTag("Player"))
        {
            elementToCarry = other.gameObject;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Box") || other.CompareTag("Player"))  && other.gameObject == elementToCarry)
        {
            elementToCarry = null;
        }
        else if (other.CompareTag("Hands"))
        {
            if (GetComponent<NetworkIdentity>() == null)
            {
                ResetRBObject(other.gameObject);
            }
            else if (isServer)
            {
                RpcResetRBObject(other.gameObject);
            }
            else if (isClient)
            {
                CmdResetRBObject(other.gameObject);
            }
        }
    }

    /// <summary>
    /// Calculate and perform player movement.
    /// </summary>
    protected void MovePlayer()
    {
        // Calculate the player speed
        if (canMove)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            if (carriedElement != null)
            {
                horizontalMove *= 0.8f;
            }
        }
        else
        {
            horizontalMove = 0;
        }

        // Say the animator to activate the running animation.
        animator.SetFloat(speedParamID, Mathf.Abs(horizontalMove));
    }

    /// <summary>
    /// Perform the actions to pick up or drop an object and fit the animation to carry.
    /// </summary>
    protected void CarryPosition()
    {
        if (GetComponent<NetworkIdentity>() == null)
        {
            if (carryButton && elementToCarry != null && !isCarry)
            {
                carryButton = false;

                TakeObject();
            }

            isCarry = !isCarry;
            controller.EnableCarryCollider(isCarry);
            animator.SetBool(carryParamID, isCarry);

            if (carryButton && carriedElement != null && !isCarry)
            {
                carryButton = false;

                DropObject(5f);
            }
            else if (carriedElement != null && !isCarry)
            {
                DropObject(0f);
            }
        }
        else if(isServer)
        {
            if (carryButton && elementToCarry != null && !isCarry)
            {
                carryButton = false;

                RpcTakeObject();
            }

            isCarry = !isCarry;
            RpcStatus(isCarry);
            animator.SetBool(carryParamID, isCarry);

            if (carryButton && carriedElement != null && !isCarry)
            {
                carryButton = false;

                RpcDropObject(5f);
            }
            else if (carriedElement != null && !isCarry)
            {
                RpcDropObject(0f);
            }
        }
        else if(isClient)
        {
            if (carryButton && elementToCarry != null && !isCarry)
            {
                carryButton = false;

                CmdTakeObject();
            }

            isCarry = !isCarry;
            CmdStatus(isCarry);
            animator.SetBool(carryParamID, isCarry);

            if (carryButton && carriedElement != null && !isCarry)
            {
                carryButton = false;

                CmdDropObject(5f);
            }
            else if (carriedElement != null && !isCarry)
            {
                CmdDropObject(0f);
            }
        }
    }

    /// <summary>
    /// Take the object that was selected to carry
    /// </summary>
    protected void TakeObject()
    {
        elementToCarry.transform.SetParent(this.transform);
        elementToCarry.transform.position = transform.GetChild(1).transform.position;

        Rigidbody2D rb = elementToCarry.GetComponent<Rigidbody2D>();
        rb.simulated = false;

        carriedElement = elementToCarry;

        elementToCarry = null;
    }

    /// <summary>
    /// Drop the gameObject that was carried
    /// </summary>
    protected void DropObject(float thrust)
    {
        carriedElement.transform.parent = null;

        Rigidbody2D rb = carriedElement.GetComponent<Rigidbody2D>();
        rb.simulated = true;

        carriedElement = null;

        if (controller.GetFacingRight())
        {
            rb.AddForce(Vector3.right * (thrust * rb.mass), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.left * (thrust * rb.mass), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// When the object leaves the player's collider, its rigidbody is restored.
    /// </summary>
    public void ResetRBObject(GameObject other)
    {
        transform.parent = null;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;
    }

    protected void Status(bool carry)
    {
        // Switch the carry, collider and animation state.
        controller.EnableCarryCollider(carry);
    }

    //Functions that make the synchronization between server and client
    [Command]
    void CmdTakeObject()
    {
        RpcTakeObject();
    }

    [Command]
    void CmdDropObject(float thrust)
    {
        RpcDropObject(thrust);
    }

    [ClientRpc]
    protected void RpcTakeObject()
    {
        TakeObject();
    }

    [ClientRpc]
    protected void RpcDropObject(float thrust)
    {
        DropObject(thrust);
    }

    [Command]
    protected void CmdStatus(bool carry)
    {
        RpcStatus(carry);
    }

    [ClientRpc]
    protected void RpcStatus(bool carry)
    {
        Status(carry);
    }

    [ClientRpc]
    protected void RpcResetRBObject(GameObject other)
    {
        ResetRBObject(other);
    }

    [Command]
    protected void CmdResetRBObject(GameObject other)
    {
        RpcResetRBObject(other);
    }

    public int GetId()
    {
        return id;
    }

    public bool GetIsServer()
    {
        return isServer;
    }
}
