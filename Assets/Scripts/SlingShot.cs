using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class SlingShot : MonoBehaviour {
	[HideInInspector]
	public Vector3 SlingshotMiddleVector;
	public LineRenderer TrajectoryLineRenderer;
	public float ThrowSpeed;

	public LineRenderer SlingshotLinerenderer01;
	public LineRenderer SlingshotLinerenderer02;
	public Transform BirdWaitPosition;
	public Transform LeftSlingshotPostion, RightSlingshotPosition;

	public GameObject BirdToThrow;     //要发射的鸟 Idele, Flying
	[HideInInspector]

	public float BirdDistance = 2;     //皮筋拉动最大长度
	public SlingShotState slingShotState;

	void Start () {
		SlingshotLinerenderer01.sortingLayerName="foreground";
		SlingshotLinerenderer02.sortingLayerName="foreground";
		SlingshotLinerenderer01.sortingOrder = 0;
		SlingshotLinerenderer02.sortingOrder = 0;
		SlingshotLinerenderer01.SetPosition (0, LeftSlingshotPostion.position);
		SlingshotLinerenderer02.SetPosition (0, RightSlingshotPosition.position);
	

		SlingshotMiddleVector = new Vector3 (
			(LeftSlingshotPostion.position.x + RightSlingshotPosition.position.x) / 2,
			(LeftSlingshotPostion.position.y + RightSlingshotPosition.position.y) / 2,
			0);
		TrajectoryLineRenderer.sortingLayerName ="foreground";
			
	}
	

	void Update () {
		switch (slingShotState) 
		{
		case SlingShotState.Idle:
			InitializeBird ();
			SetSlingShotLineRenderersActive (true);       //重新显示皮筋
			SetTrajectoryLineRenderersActive(true);       //重新显示弹道线
			DisPlaySlingshotLineRenderers ();
			if (Input.GetMouseButtonDown (0)) {
				Vector3 location = Camera.main.ScreenToWorldPoint (Input.mousePosition); //将屏幕位置转为坐标位置
				if(BirdToThrow.GetComponent<CircleCollider2D>()==Physics2D.OverlapPoint(location))  //鼠标是否点在鸟上
				{
					slingShotState = SlingShotState.UserPulling;     
				}
			}
			break;

		case SlingShotState.UserPulling:          //小鸟位置跟随鼠标移动
			DisPlaySlingshotLineRenderers ();
			if (Input.GetMouseButton (0)) {
				Vector3 location = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				location.z = 0;
				//皮筋拉伸限度；
				if (Vector3.Distance (location, SlingshotMiddleVector) > BirdDistance) {
					Vector3 maxPosition = (location - SlingshotMiddleVector).normalized * BirdDistance + SlingshotMiddleVector;
					BirdToThrow.transform.position = maxPosition;
				} else {
					BirdToThrow.transform.position = location;
				}
				float distance = Vector3.Distance (SlingshotMiddleVector, BirdWaitPosition.position);
				DisplayTrajectoryLineRenderer2 (distance);
			}
			else   //鼠标弹起 
			{
				SetTrajectoryLineRenderersActive (false);
				float distance = Vector3.Distance (SlingshotMiddleVector, BirdToThrow.transform.position);
				if (distance > 1) {
					SetSlingShotLineRenderersActive (false);  //隐藏皮筋
					slingShotState=SlingShotState.BirdFlying;//设置小鸟处于发射状态
					ThrowBird(distance);
				} 
				else {
					//GoKit回归
					BirdToThrow.transform.positionTo (distance / 10, BirdWaitPosition.transform.position)
						.setOnCompleteHandler((x)=>{   //设置一个处理器
							x.complete();
							x.destroy();
							InitializeBird();
							});   //拉姆达 表达式
				}
			}
			break;

		}
	}

	/***
	 * 设置飞翔**
	 * */
	void ThrowBird(float distance)
	{
		Vector3 velocity = SlingshotMiddleVector - BirdToThrow.transform.position;
		BirdToThrow.GetComponent<Bird> ().OnThrow ();
		BirdToThrow.GetComponent<Rigidbody2D> ().velocity = new Vector2 (velocity.x, velocity.y)*ThrowSpeed*distance;
	}
	/***
	 * 设置辅助线是否激活
	 * */
	void SetTrajectoryLineRenderersActive(bool isActive)
	{
		TrajectoryLineRenderer.enabled = isActive;
	}

	void SetSlingShotLineRenderersActive(bool isActive)
	{
		SlingshotLinerenderer01.enabled = isActive;
		SlingshotLinerenderer02.enabled = isActive;
	}


	/***
	 * 初始化小鸟位置
	 * */
	void InitializeBird(){
		BirdToThrow.transform.position = BirdWaitPosition.position;
		slingShotState = SlingShotState.Idle;
	}



	/***
	 * 
	 *绘制皮筋
	 * */
	void DisPlaySlingshotLineRenderers(){
		SlingshotLinerenderer01.SetPosition (1, BirdToThrow.transform.position);
		SlingshotLinerenderer02.SetPosition (1, BirdToThrow.transform.position);
	}


	/***
	 * 绘制弹道线
	 * */
	void DisplayTrajectoryLineRenderer2(float distance){
		// x axis :space x=initSpaceX+velocityX*time;
		// y axis :space y=initSpaceY+VelocityY*time+0.5*accelerationY * time^2; 
		Vector2 v2=SlingshotMiddleVector-BirdToThrow.transform.position;
		int segmentCount = 15;

		Vector2[] segments = new Vector2[segmentCount];
		segments [0] = BirdToThrow.transform.position;
		Vector2 segVelocity = new Vector2 (v2.x, v2.y) * ThrowSpeed * distance;

		for (int i = 1; i < segmentCount; i++) {
			float time2 = i * Time.fixedDeltaTime * 5;
			segments [i] = segments [0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow (time2, 2);
		}

		TrajectoryLineRenderer.numPositions =segmentCount;
		for (int i = 0; i < segmentCount; i++) {
			TrajectoryLineRenderer.SetPosition (i, segments [i]);
		}
	}
}
