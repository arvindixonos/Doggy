using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public	float		m_MoveSpeed = 1f;
	public	float		m_PrevXTranslate = 0f;
	public	Vector3		m_JumpStartPos = Vector3.zero;
	public	Vector3		m_JumpEndPos = Vector3.zero;
	public	bool		m_Jumping = false;
	public	float		m_JumpDuration = 2f;
	private	float		m_JumpTimer = 0f;
	
	public void OnTriggerEnter(Collider other)
	{
		if(other.name.Equals("StartCamera"))
		{
			CameraController.Instance.StartCamera();
		}
	}
	
	public void OnCollisionEnter(Collision collision)
	{
		if(m_Jumping)
			m_Jumping = false;
	}
	
	void Start () {
		
	}
	
	Vector3 BezierJump(float t)
	{
		Vector3 position = Vector3.zero;
		Vector3 height = m_JumpStartPos + (m_JumpEndPos - m_JumpStartPos) / 2f;
		height.y += 20f;
			
		position = (1f - t) * (1f - t) * m_JumpStartPos + 2 * t * (1f - t) * height + t * t * m_JumpEndPos;
		
		return position;
	}
	
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Space))
		{	
			rigidbody.AddForce(Vector3.up * 25f, ForceMode.VelocityChange);
			m_Jumping = true;
		}
		
		//if(!m_Jumping)
		{
			rigidbody.AddForce(Vector3.right * 2f, ForceMode.VelocityChange);
		}
	}
}
