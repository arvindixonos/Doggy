using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	private		Vector3			m_JumpStartPos = Vector3.zero;
	private		Vector3			m_JumpEndPos = Vector3.zero;
	private		bool			m_Jumping = false;
	private		float			m_JumpTimer = 0f;
	private		Vector3			m_PrevPosition = Vector3.zero;
	
	public		bool			m_DoubleJumpOn = true;
	private		bool			m_DoubleJumped = false;
	
	public		float			m_MoveSpeed = 20f;
	
	public		float			m_FallDownCurve = -5f;
	public		float			m_JumpDistance = 25f;
	public		float			m_JumpHeight = 30f;
	public		float			m_JumpMultiplier = 0.03f;
	
	public void OnTriggerEnter(Collider other)
	{

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
		Vector3 height = (m_JumpStartPos + m_JumpEndPos) / 2f;
		height.y += m_JumpHeight;
			
		position = (1f - t) * (1f - t) * m_JumpStartPos + 2 * t * (1f - t) * height + t * t * m_JumpEndPos;
		
		return position;
	}
	
	void FixedUpdate () 
	{			
		if(m_Jumping)
		{				
			m_JumpTimer += m_MoveSpeed * Time.fixedDeltaTime * m_JumpMultiplier;		
			
			if((m_JumpTimer < 0.3f) || (m_JumpTimer < 0.5f && isTouching()))
			{				
				Vector3 position = BezierJump(m_JumpTimer);
				
				transform.position = position;										
			}
			else
			{
				Run();
			}
		}
		else
		{
			Run();
		}
				
		Vector3 movementThisStep = transform.position - m_PrevPosition; 
		RaycastHit hitInfo;  
  		if (Physics.Raycast(transform.position, movementThisStep, out hitInfo, 1f, ~(1<<gameObject.layer))) 
		{
			if(m_Jumping)	
				Landed();
		}
		
		if(isTouching())
		{
			Touching();
		}		
		
		m_PrevPosition = transform.position;
	}
	
	void Run()
	{
		float xTranslate = m_MoveSpeed * Time.fixedDeltaTime;
		float yTranslate = m_FallDownCurve * Time.fixedDeltaTime;
		
		transform.Translate(xTranslate, yTranslate, 0f);
	}
	
	bool isTouching()
	{
#if UNITY_EDITOR
   		if(Input.GetKey(KeyCode.Space))
		{	
			return true;
		}
#else
		if(Input.touchCount > 0)
		{
			return true;
		}
#endif	
		
		return false;
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
