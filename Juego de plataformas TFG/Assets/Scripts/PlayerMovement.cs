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
        jumpParamID = Animator.StringToHash("IsJumping");
        carryParamID = Animator.StringToHash("IsCarry");
	}

    void Update()
    {
        // Calculate the character speed
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Say the animator to activate the running animation.
        animator.SetFloat(speedParamID, Mathf.Abs(horizontalMove));

        // If press jump button, activate jumping animation.
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;
            animator.SetBool(jumpParamID, true);
        }

        // If press jump button, enable the collider and activate jumping animation.
        if (Input.GetButtonDown("Carry"))
        {
            isCarry = true;
            controller.EnableCarryCollider(isCarry);
            animator.SetBool(carryParamID, true);
        }
        // If loose jump button, enable the collider and activate jumping animation.
        else if(Input.GetButtonUp("Carry"))
        {
            isCarry = false;
            controller.EnableCarryCollider(isCarry);
            animator.SetBool(carryParamID, false);
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
        animator.SetBool(jumpParamID, false);
    }

    void FixedUpdate()
    {
        // Call the "Move" function.
        controller.Move(horizontalMove * Time.fixedDeltaTime, isJump);
        isJump = false;
    }
}
