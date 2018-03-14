using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {

	public float ParallaxFactor;       //移动比例
	Vector3 previousCameraTransform;
	Camera camera;

	void Start () {
		camera = Camera.main;
		previousCameraTransform = camera.transform.position;   //相机位置
	}
	

	void Update () {
		Vector3 delta = camera.transform.position - previousCameraTransform;
		delta.y = 0;
		delta.z = 0;

		transform.position += delta / ParallaxFactor;         //相对运动原理  实现远处景物移动速度慢
		previousCameraTransform = camera.transform.position;  //景物移动速度=相机速度-景物跟随速度  
	}
}
