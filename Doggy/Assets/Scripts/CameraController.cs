using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public	static	CameraController	Instance = null;
	
	public	PlayerController		m_Player;
	
	public 	Vector3			m_Offset;
	
	private	bool			m_StartCamera = false;
	
	public void	StartCamera()
	{
		m_StartCamera = true;
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
	
	void Update () {
		
		if(m_StartCamera)
		{
			float xTranslate = m_Player.transform.position.x + m_Offset.x;
			float zTranslate = (16f - m_Player.transform.position.y) * 0.1f;
			iTween.MoveUpdate(gameObject, iTween.Hash("x", xTranslate, "z", zTranslate - 20f, "time", 3.0f));
		}
	}
}
