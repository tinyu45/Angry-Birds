using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody2D))]   //添加脚本的物体必须包含刚体组件
public class Bird : MonoBehaviour {

	public BirdState State {
		get;
		private set;
	}

	void Start () {
		GetComponent<TrailRenderer> ().enabled = false;   //隐藏拖尾
		GetComponent<TrailRenderer>().sortingLayerName="foreground";

		GetComponent<Rigidbody2D> ().isKinematic = true;  //运动，避免引擎干扰 
		GetComponent<CircleCollider2D>().radius=Constants.BirdColliderRadiusNormal;

		State = BirdState.BeforeThrow;
	}
	

	void FixedUpdate () {   //飞翔中且速度小于最小速度
		if (State == BirdState.Throw && GetComponent<Rigidbody2D>().velocity.sqrMagnitude<=Constants.MinVelocity) {
			StartCoroutine (DestroyAfter (2));  //启动协程
		}
	}


	/***
	 * 
	 * 延时销毁对象 协程
	 * 
	 * */
	IEnumerator DestroyAfter(float time)
	{
		yield return new WaitForSeconds (time);
		Destroy (gameObject);
	}



	/***
	 * 发射中
	 * */
	public void OnThrow()
	{
		GetComponent<TrailRenderer> ().enabled = true;
		GetComponent<Rigidbody2D> ().isKinematic = false;   //交由引擎控制
		State = BirdState.Throw;
	}
}
