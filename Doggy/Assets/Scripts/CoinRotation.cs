using UnityEngine;
using System.Collections;

public class CoinRotation : MonoBehaviour {

	void Start () {
		iTween.RotateTo(gameObject, iTween.Hash("y", 180f, "time", 1f, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.easeOutCubic, "includechildren", false));
		
	}
	
	void Update () {
	
	}
}
