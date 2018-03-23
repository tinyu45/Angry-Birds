using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

	public float Health = 50;    //抗碰撞能力


	//回调方法 碰撞处理函数
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.GetComponent<Rigidbody2D> () == null)
			return;
		/***
		 * 由于此处使用冲击速度计算伤害，所以不用区分碰撞物体是否为木板
		 * */
		float damange = col.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude * 10;  //危险系数(伤害值)
		if(damange>10)
		{
			AudioClip clip = Resources.Load<AudioClip> ("wood");
			AudioSource.PlayClipAtPoint(clip, transform.position, 0.8f);
		}
		Health-=damange;
		if (Health <= 0) {
			Destroy (this.gameObject);
		}
	

	}

}
