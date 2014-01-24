using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public	static	CameraController	Instance = null;
	
	public	PlayerController		m_Player;	
	public 	float					m_xPlayerOffset;
	public	float					m_MiddleYPosition = 30f;
	public	float					m_StartZPosition = -50f;
	public	float					m_ZExtend = 20f;
	public	float					m_ZMoveSpeed = 10f;
	
	private	bool					m_StopCamera = false;
		
	public	void			StopCamera()
	{
		m_StopCamera = true;
	}
	
	public	void			StartCamera()
	{
		m_StopCamera = false;
	}
	
	void Awake()
	{
		if(Instance == null)
			Instance = this;
	}
	
	void OnDestroy()
	{
		Instance = null;
	}
	
	void Start () {	

		Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
	}
	
	void FixedUpdate () 
	{		
		if(m_StopCamera)
			return;
		
		Vector3 distanceRemaining = transform.position - m_Player.transform.position;		
					
		Vector3 position = transform.position;
	
		float deltaX = distanceRemaining.x - m_xPlayerOffset;
	
		if(deltaX < 0f)
		{		
			position.x -= deltaX;
		}	
		
		if(!m_Player.isJumping())
		{	
			float deltaY = Mathf.Abs((m_Player.transform.position.y - m_MiddleYPosition ) / m_MiddleYPosition);	
			deltaY = Mathf.Clamp01(deltaY);		
				
			float distanceZ = Mathf.Lerp(m_StartZPosition, m_StartZPosition - m_ZExtend, deltaY);								
			distanceZ = distanceZ - transform.position.z;
			
			if(Mathf.Abs(distanceZ) > m_ZMoveSpeed)
				distanceZ = m_ZMoveSpeed * Mathf.Sign(distanceZ);
			
			position.z += distanceZ;
		}
		
		transform.position = position;
	}
}
