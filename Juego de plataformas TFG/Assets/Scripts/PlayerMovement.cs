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
    protected GameObject carriedElement = null;

    /// <summary>
    /// Indicates if player can move
    /// </summary>
    [HideInInspector] public bool canMove = true;

    [SerializeField] protected int id = -1;

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
    protected bool isCarry = false;

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

        if(GetComponent<NetworkIdentity>() != null){

            if(!isLocalPlayer){

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
            CarryPosition();
        }

        // Deactivate carry animation if it is falling.
        if (isCarry && animator.GetFloat(fallParamID) < -0.01)
        {
            CarryPosition();
        }
    }

    protected void FixedUpdate()
    {
        // Call the "Move" function.
        controller.Move(horizontalMove * Time.fixedDeltaTime, isJump);

        isJump = false;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box") || other.CompareTag("Player"))
        {
            elementToCarry = other.gameObject;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Box") || other.CompareTag("Player"))
        {
            elementToCarry = null;
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
        if (Input.GetButtonDown("Carry") && elementToCarry != null && !isCarry)
        {
            TakeObject();
        }

        // Switch the carry, collider and animation state.
        isCarry = !isCarry;
        controller.EnableCarryCollider(isCarry);
        animator.SetBool(carryParamID, isCarry);

        if (Input.GetButtonDown("Carry") && carriedElement != null && !isCarry)
        {
            DropObject(5f);
        }
        else if (carriedElement != null && !isCarry)
        {
            DropObject(0f);
        }
    }

    protected void TakeObject()
    {
        elementToCarry.transform.SetParent(this.transform);
        elementToCarry.transform.position = transform.GetChild(1).transform.position;

        Rigidbody2D rb = elementToCarry.GetComponent<Rigidbody2D>();
        rb.simulated = false;

        carriedElement = elementToCarry;
    }

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
    public void ResetRBObject()
    {
        if (carriedElement != null)
        {
            carriedElement.transform.parent = null;

            Rigidbody2D rb = carriedElement.GetComponent<Rigidbody2D>();
            rb.simulated = true;

            carriedElement = null;
        }
    }

    public int GetId()
    {
        return id;
    }
}
