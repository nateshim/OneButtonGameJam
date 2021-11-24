using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private Vector2 m_JumpForce = new Vector2(50f, 200f);							// Amount of force added when the player jumps.
	[SerializeField] protected LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] protected Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] protected Transform m_CeilingCheck;							// A position marking where to check for ceilings

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private float jumpTimer = 1f;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public Vector2 GetVelocity()
	{
		return m_JumpForce;
	}
    
    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
        Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        Vector2 playerScreenPoint = Camera.main.WorldToScreenPoint(GameObject.Find("Player").transform.position);
        if (mouse.x < playerScreenPoint.x) {
            Flip(false);
        } else {
            Flip(true);
        }
		if (Input.GetButton("Jump") && m_Grounded)
        {
			if (jumpTimer > 0)
			{
            	GetComponent<LineRenderer>().enabled = true;
            	GetComponent<LaunchArcRenderer>().RenderArc();
				m_JumpForce.x += 1f;
				m_JumpForce.y += 5f;
				jumpTimer -= Time.deltaTime;
			} else {
				m_JumpForce.x = 0f;
				m_JumpForce.y = 0f;
				GetComponent<LineRenderer>().enabled = false;
			}
        } else {
			GetComponent<LineRenderer>().enabled = false;
			jumpTimer = 1f;
		}
	}


	public void Jump(bool jump)
	{
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
            if (m_FacingRight) {
                m_Rigidbody2D.AddForce(m_JumpForce);
            } else {
			    m_Rigidbody2D.AddForce(new Vector2(-m_JumpForce.x, m_JumpForce.y));
            }
			m_JumpForce = new Vector2(50f, 200f);
		}
	}


	private void Flip(bool right)
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = right;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}