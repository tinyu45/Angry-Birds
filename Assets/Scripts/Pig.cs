using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour {

	public float Health=100f;

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.GetComponent<Rigidbody2D> () == null)
			return;
		if (col.gameObject.CompareTag ("Bird")) {    //小鸟碰撞猪怪
			Destroy (gameObject);
		} 
		else 
		{
			float damage = col.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude * 10;
			if(damage>10)
			{
				AudioClip clip = Resources.Load<AudioClip> ("pig2");
				AudioSource.PlayClipAtPoint(clip, transform.position, 0.8f);
			}
			Health -= damage;

			if (Health <= 0)
				Destroy (this.gameObject);
		}
	}
}
