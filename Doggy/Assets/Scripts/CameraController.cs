using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public	static	CameraController	Instance = null;
	
	public	PlayerController		m_Player;	
	public 	float					m_xPlayerOffset;
	
	public	float					m_MiddleYPosition = 30f;
	public	float					m_StartZPosition = -50f;
	public	float					m_ZExtend = 20f;
	public	float					m_ZMoveEasing = 0.9f;
	
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
	
		if(distanceRemaining.x - m_xPlayerOffset < 0f)
		{		
			float deltaX = m_xPlayerOffset - distanceRemaining.x;
			position.x += deltaX;
		}	
		
//		position.y -= distanceRemaining.y * m_ZMoveEasing;
	
		float deltaZ = (m_Player.transform.position.y - m_MiddleYPosition) / m_MiddleYPosition;
		
		deltaZ *= m_ZMoveEasing;
		deltaZ += 0.5f;
		
		position.z = Mathf.Lerp(m_StartZPosition + m_ZExtend, m_StartZPosition - m_ZExtend, deltaZ);	
		
		transform.position = position;		
	}
}
