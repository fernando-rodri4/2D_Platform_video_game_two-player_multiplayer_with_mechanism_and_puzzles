using UnityEngine;
using UnityEngine.Events;

/// <summary>
///The script is based on the one provided by Unity as part of their Standard Assets.
/// </summary>
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] float m_MovementSmoothing = .05f;	// How much to smooth out the movement.
	[SerializeField] bool m_AirControl = false;							// Whether or not a player can steer while jumping.
	[SerializeField] LayerMask m_WhatIsGround;							// A mask determining what is ground to the character.
	[SerializeField] Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] Collider2D m_CarryEnableCollider;					// A collider that will be enable when carry something o press the button.

	const float k_GroundedRadius = .2f;			// Radius of the overlap circle to determine if grounded.
	bool m_Grounded;            				// Whether or not the player is grounded.
	Rigidbody2D m_Rigidbody2D;
	bool m_FacingRight = true;  				// For determining which way the player is currently facing.
	Vector3 velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;				// Allows us to trigger certain functionality when something occurs.

	/// <summary>
    /// Reference to the animator component.
    /// </summary>
	Animator animator;

    /// <summary>
    /// Reference to the animator parameters.
    /// </summary>
	int directionParamID;
    int fallParamID;

	void Awake()
	{
		// Get reference to the rigidbody2d, animator component and create an UnityEvent.
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		animator = GetComponent<Animator>();
	}

	void Start()
	{
		// Get the integer hashes of the Animator parameters. This is much more efficient
        // than passing string into the animator.
		directionParamID = Animator.StringToHash("Direction");
        fallParamID = Animator.StringToHash("VelocityY");
	}

	void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
				if(!wasGrounded)
				{
					OnLandEvent.Invoke();
				}
		}

		// Say the animator to activate the fall animation.
		animator.SetFloat(fallParamID, m_Rigidbody2D.velocity.y);
	}

	public void Move(float move, bool jump)
	{
		// Only control the player if grounded or airControl is turned on.
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity.
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// and then smoothing it out and applying it to the character.
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Tell the animator if he looks to the right or to the left.
		if(m_FacingRight)
		{
			animator.SetFloat(directionParamID, 0);
		}
		else
		{
			animator.SetFloat(directionParamID, 1);
		}
	}

	// Activate and deactivate the collider for when the character take something.
	public void EnableCarryCollider(bool isEnable)
	{
		if(m_CarryEnableCollider != null)
		{
			m_CarryEnableCollider.enabled = isEnable;
		}
	}
}