using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public bool IsFlollowing;
	[HideInInspector]
	public Transform BirdToFollow;
	public const float minCameraX = -7;
	public const float maxCameraX=11.3f;

	[HideInInspector]
	public Vector3 StartPosition;


	void Start () {	
		StartPosition = transform.position;
	}
	

	void Update () {
		if (IsFlollowing) {
			if (BirdToFollow != null) {
				Vector3 birdPosition = BirdToFollow.transform.position;
				float x = Mathf.Clamp (birdPosition.x, minCameraX, maxCameraX); //使得BirdPosition.X值介于min与max之间
				transform.position = new Vector3 (x, StartPosition.y, StartPosition.z);  //使得物体总在相机中间
			} else
				IsFlollowing = false;
		}
	}
}
