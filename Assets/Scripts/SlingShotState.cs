using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public enum SlingShotState
	{
		Idle,        //默认状态
		UserPulling, //用户拖动中
		BirdFlying   //发射中
	}

	//小鸟的状态
	public enum BirdState
	{
		BeforeThrow, //扔出前
		Throw        //发射中
	}


	//游戏状态
	public enum GameState
	{
		Start,
		BirdMovingToSlingshot,    //待发射
		Playing,                  //发射
		Wan,
		Lost
	}
}
