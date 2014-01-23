using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	private		Vector3			m_JumpStartPos = Vector3.zero;
	private		Vector3			m_JumpEndPos = Vector3.zero;
	private		bool			m_Jumping = false;
	private		float			m_JumpTimer = 0f;
	
	public		bool			m_DoubleJumpOn = true;
	private		bool			m_DoubleJumped = false;
	
	public		float			m_MoveSpeed = 20f;
	
	public		float			m_FallDownCurve = -5f;
	public		float			m_JumpDistance = 25f;
	public		float			m_JumpHeight = 30f;
	
	public void OnTriggerEnter(Collider other)
	{
		if(m_Jumping && m_JumpTimer > 0.5f)
		{
			collider.isTrigger = false;	
		}
	}
	
	public void OnCollisionEnter(Collision collision)
	{		
		if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
		{		
			if(m_Jumping)
			{			
				Landed();			
			}
		}
	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(0f, 0f, 50f, 50f), "Restart"))
		{
			Application.LoadLevel("TestScene");
		}
	}
	
	void Start () {
		
	}
	
	Vector3 BezierJump(float t)
	{
		Vector3 position = Vector3.zero;
		Vector3 height = m_JumpStartPos + (m_JumpEndPos - m_JumpStartPos) / 2f;
		height.y += m_JumpHeight;
			
		position = (1f - t) * (1f - t) * m_JumpStartPos + 2 * t * (1f - t) * height + t * t * m_JumpEndPos;
		
		return position;
	}
	
	void FixedUpdate () 
	{			
		if(m_Jumping)
		{
			m_JumpTimer += Time.fixedDeltaTime;
			Vector3 position = BezierJump(m_JumpTimer);
			
			if(m_JumpTimer > 0.5f)
			{
				Vector3 movementThisStep = position - transform.position; 
				RaycastHit hitInfo;  
	      		if (Physics.Raycast(transform.position, movementThisStep, out hitInfo, 1f, ~(1<<gameObject.layer))) 
				{
					Landed();
	   				return;
				}
			}

			transform.position = position;										
		}
		else
		{
			float xTranslate = m_MoveSpeed * Time.fixedDeltaTime;
			float yTranslate = m_FallDownCurve * Time.fixedDeltaTime;
		
			transform.Translate(xTranslate, yTranslate, 0f);	
		}

#if UNITY_EDITOR
   		if(Input.GetKeyDown(KeyCode.Space))
		{	
			Touching();
		}
#else
		if(Input.touchCount > 0)
		{
    		Touch touch = Input.GetTouch(0);
			
			if(touch.phase == TouchPhase.Began)
			{	
				Touching();
			}
		}
#endif		
		
	}
	
	void Touching()
	{
		if(!m_Jumping)
			Jump();	
		else
		{
			if(m_DoubleJumpOn && !m_DoubleJumped)
			{
				m_DoubleJumped = true;
				Jump();
			}
		}
	}
	
	void Landed()
	{
		collider.isTrigger = false;
		
		m_Jumping = false;
		m_DoubleJumped = false;
	}
	
	void Jump()
	{
		collider.isTrigger = true;
		
		m_Jumping = true;
		m_JumpTimer = 0f;
		m_JumpStartPos = transform.position;
		m_JumpEndPos = m_JumpStartPos + new Vector3(m_JumpDistance, 0f, 0f);	
	}
}
