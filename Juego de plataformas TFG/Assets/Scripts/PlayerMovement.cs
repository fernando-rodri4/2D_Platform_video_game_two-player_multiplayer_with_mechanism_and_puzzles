using UnityEngine;
using System.Collections;

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
    int carryParamID;
    int fallParamID;

    /// <summary>
    /// Locking object to synchronize the function.
    /// </summary>
    readonly object lock_ = new object();

    /// <summary>
    /// Reference to the CameraTransition.
    /// </summary>
    public CameraTransition cameraTrans;

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
        carryParamID = Animator.StringToHash("IsCarry");
        fallParamID = Animator.StringToHash("VelocityY");
	}

    void Update()
    {
        // Calculate the character speed
        if(!cameraTrans.start)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        }
        else
        {
             horizontalMove = 0;
        }

        // Say the animator to activate the running animation.
        animator.SetFloat(speedParamID, Mathf.Abs(horizontalMove));

        // If press jump button, activate jumping animation and deactivate carry animation if it is playing.
        if (Input.GetButtonDown("Jump") && !cameraTrans.start)
        {
            isJump = true;

            if(isCarry)
            {
                CarryPosition();
            }
        }

        // If press carry button, call CarryPosition method.
        if (Input.GetButtonDown("Carry") && !cameraTrans.start)
        {
            CarryPosition();
        }

        // With this method the character ignores the collider of the other player to avoid colliding.
        if(playerCollider && otherPlayerCollider)
        {
            Physics2D.IgnoreCollision(playerCollider, otherPlayerCollider);
        }
    }

    void FixedUpdate()
    {
        // Call the "Move" function.
        controller.Move(horizontalMove * Time.fixedDeltaTime, isJump);

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

    IEnumerator OnTriggerStay2D(Collider2D other)
    {
        lock(lock_)
        {
            if(other.tag == "Warp Enter" && Input.GetButtonDown("Enter"))
            {
                cameraTrans.FadeIn();

                yield return new WaitForSeconds(cameraTrans.fadeTime);

                // Transport the character to the exit.
                other.gameObject.SetActive(false);
                transform.position = other.transform.GetChild(0).transform.position;
                cameraTrans.ChangeConfiner();

                cameraTrans.FadeOut();
            }
        }
    }
}
