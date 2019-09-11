using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Reference to CharacterController2D script.
    /// </summary>
    public CharacterController2D controller;

    /// <summary>
    /// Reference to the collider of both players.
    /// </summary>
    public Collider2D playerCollider, otherPlayerCollider;

    /// <summary>
    /// Reference to the animator component.
    /// </summary>
    Animator animator;

    /// <summary>
    /// Run speed.
    /// </summary>
    public float runSpeed = 40f;

    /// <summary>
    /// Character speed.
    /// </summary>
    float horizontalMove = 0f;

    /// <summary>
    /// Check if the character is jumping or not.
    /// </summary>
    bool isJump = false;

    /// <summary>
    /// Check if the character is in a position to carry something or not.
    /// </summary>
    bool isCarry = false;

    /// <summary>
    /// Reference to the animator parameters.
    /// </summary>
	int speedParamID;
    int jumpParamID;
    int carryParamID;
    int fallParamID;

    void Awake()
	{
        // Get reference to the animator component.
		animator = GetComponent<Animator>();
	}

	void Start()
	{
        // Get the integer hashes of the Animator parameters. This is much more efficient
        // than passing string into the animator.
		speedParamID = Animator.StringToHash("Speed");
        jumpParamID = Animator.StringToHash("Prevent2Jump");
        carryParamID = Animator.StringToHash("IsCarry");
        fallParamID = Animator.StringToHash("VelocityY");
	}

    void Update()
    {
        // Calculate the character speed
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Say the animator to activate the running animation.
        animator.SetFloat(speedParamID, Mathf.Abs(horizontalMove));

        // If press jump button, activate jumping animation and deactivate carry animation if it is playing.
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;

            if(isCarry)
            {
                CarryPosition();
            }
        }

        // If press carry button, call CarryPosition method.
        if (Input.GetButtonDown("Carry"))
        {
            CarryPosition();
        }

        // With this method the character ignores the collider of the other player to avoid colliding.
        if(playerCollider && otherPlayerCollider)
        {
            Physics2D.IgnoreCollision(playerCollider, otherPlayerCollider);
        }
    }

    // Method that deactivate the jumping animation.
    public void OnLanding()
    {
        animator.SetBool(jumpParamID, true);
    }

    void FixedUpdate()
    {
        // Call the "Move" function.
        controller.Move(horizontalMove * Time.fixedDeltaTime, isJump);

        // Prevent 2 jump in the air.
        if(isJump)
        {
            animator.SetBool(jumpParamID, false);
        }

        isJump = false;

        // Deactivate carry animation if it is playing.
        if(isCarry && animator.GetFloat(fallParamID) < -0.01)
        {
            CarryPosition();
        }
    }

    void CarryPosition()
    {
        // Switch the carry, collider and animation state.
        isCarry = !isCarry;
        controller.EnableCarryCollider(isCarry);
        animator.SetBool(carryParamID, isCarry);
    }
}
