using UnityEngine;
using System.Collections;

public class FireBallScriptSP : MonoBehaviour {

	public float fireBallSpeed;
	public AudioClip hitSound;

	void Update(){
		rigidbody.velocity = transform.forward * fireBallSpeed * Time.deltaTime;
	}

	void OnCollisionEnter(Collision colli){


		if(colli.gameObject.tag == "wall")
			Destroy(gameObject);
		
		if(colli.gameObject.tag == "Player"){
			colli.gameObject.BroadcastMessage("ApplyDmg", 10);
			audio.PlayOneShot (hitSound);
			Destroy(gameObject);
		}

		if(colli.gameObject.tag == "AI"){
			colli.gameObject.BroadcastMessage("ApplyDmg", 50);
			audio.PlayOneShot (hitSound);
			Destroy(gameObject);
		}
	}
}
