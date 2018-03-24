using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System.Linq;       //lin ki

public class GameManger : MonoBehaviour {

	private List<GameObject> Birds;
	private List<GameObject> Bricks;
	private List<GameObject> Pigs;

	public static GameState CurrentGameState=GameState.Start;   //初始化游戏状态
	public SlingShot slingShot;                                 //弹弓
	public CameraFollow cameraFlow;                             //相机跟随

	int BirdIndex;

	void Start () {
		slingShot.enabled = false;            //关闭脚本
		Birds = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Bird"));
		Pigs = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Pig"));
		Bricks = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Brick"));
	}
	

	void Update () {
		switch (CurrentGameState) 
		{
		case GameState.Start:
			if (Input.GetMouseButtonUp (0)) 
			{
				AnimateBirdToSlingShot ();
			}
			break;
		case GameState.Playing:
			if(slingShot.slingShotState==SlingShotState.BirdFlying && (BrickAndPigIsStatic()||Time.time-slingShot.TimeThrow>5f))
			{
				//相机回归
				slingShot.enabled=false;
				AnimateCameraResetPosition ();
				CurrentGameState = GameState.BirdMovingToSlingshot;
			}
			break;
		}
	}



	/***
	 * 
	 * 相机回归
	 * **/
	void AnimateCameraResetPosition()
	{
		float direction = Vector2.Distance (Camera.main.transform.position, cameraFlow.StartPosition) / 10;
		if (direction == 0.0f)
			direction = 0.1f;

		Camera.main.transform.positionTo (
			direction,
			cameraFlow.StartPosition
		).setOnCompleteHandler ((x) => {
			cameraFlow.IsFlollowing=false;
			slingShot.slingShotState=SlingShotState.Idle;
			BirdIndex++;
			AnimateBirdToSlingShot();
		});
	}




	/***
	 * 碰撞结束
	 * **/
	bool BrickAndPigIsStatic()
	{
		foreach (var item in Bricks.Union(Birds).Union(Pigs))  //列表合成一次遍历
		{
			if(item!=null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > Constants.MinVelocity)
				return false;
		}
		return true;
	}



	/***
	 * 小鸟进入发射位置
	 * */
	void AnimateBirdToSlingShot()
	{
		Birds [BirdIndex].transform.positionTo (
			Vector2.Distance(Birds[BirdIndex].transform.position,slingShot.BirdWaitPosition.transform.position)/10,
			slingShot.BirdWaitPosition.transform.position
		).setOnCompleteHandler((x)=>{
			x.complete();
			x.destroy();
			CurrentGameState=GameState.Playing;
			slingShot.BirdToThrow=Birds[BirdIndex];
			cameraFlow.BirdToFollow=Birds[BirdIndex].transform;
			cameraFlow.IsFlollowing=true;
			slingShot.enabled=true;    //显示皮筋
		});
	}

}
