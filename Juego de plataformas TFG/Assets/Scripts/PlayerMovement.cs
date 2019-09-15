using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Reference to CharacterController2D script.
    /// </summary>
    public CharacterController2D controller;

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
    /// Check if the character enters the door collider.
    /// </summary>
    GameObject elementEnterDoor = null;

    /// <summary>
    /// Check if the character enters the box collider.
    /// </summary>
    GameObject elementEnterBox = null;

    /// <summary>
    /// Check if the character enters the box collider.
    /// </summary>
    GameObject carriedElement = null;

    /// <summary>
    /// Reference to the CameraTransition.
    /// </summary>
    public CameraTransition cameraTrans;

    /// <summary>
    /// Reference to the IgnoreCollision2D script.
    /// </summary>
    public IgnoreCollision2D ignoreCollision;

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

        if(elementEnterDoor != null && Input.GetButtonDown("Enter"))
        {
            StartCoroutine(TransportPlayer());
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
        if (Input.GetButtonDown("Carry") && elementEnterBox != null && !isCarry)
        {
            TakeObject();
        }

        // Switch the carry, collider and animation state.
        isCarry = !isCarry;
        controller.EnableCarryCollider(isCarry);
        animator.SetBool(carryParamID, isCarry);

        if (Input.GetButtonDown("Carry") && carriedElement != null && !isCarry)
        {
            DropObject(50.0f);
        }
        else if (carriedElement != null && !isCarry)
        {
            DropObject(0.0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Warp Enter")
        {
            elementEnterDoor = other.gameObject;
        }

        if(other.gameObject.tag == "Box")
        {
            elementEnterBox = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Warp Enter")
        {
            elementEnterDoor = null;
        }

        if(other.gameObject.tag == "Box")
        {
            elementEnterBox = null;
        }
    }

    IEnumerator TransportPlayer()
    {
        lock(lock_)
        {
                cameraTrans.FadeIn();

                yield return new WaitForSeconds(cameraTrans.fadeTime);

                // Transport the character to the exit.
                transform.position = elementEnterDoor.transform.GetChild(0).transform.position;
                elementEnterDoor.SetActive(false);

                cameraTrans.ChangeConfiner();

                cameraTrans.FadeOut();
        }
    }

    void TakeObject()
    {
        elementEnterBox.transform.SetParent(this.transform);
        elementEnterBox.transform.position = transform.GetChild(1).transform.position;
        elementEnterBox.GetComponent<Rigidbody2D>().simulated = false;

        carriedElement = elementEnterBox;
    }

    void DropObject(float thrust)
    {
        carriedElement.transform.parent = null;
        
        Rigidbody2D rb = carriedElement.GetComponent<Rigidbody2D>();

        rb.simulated = true;

        if(controller.GetFacingRight())
        {
            rb.AddForce(Vector3.right * thrust, ForceMode2D .Impulse);
        }
        else
        {
            rb.AddForce(Vector3.left * thrust, ForceMode2D .Impulse);
        }

    }
}