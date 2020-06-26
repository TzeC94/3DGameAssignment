using UnityEngine;
using System.Collections;

public class FireBallScript : MonoBehaviour {

	public float fireBallSpeed;
	
	// Update is called once per frame
	void Update () {
		rigidbody.velocity = transform.forward * fireBallSpeed * Time.deltaTime;
	}
	
	void OnCollisionEnter(Collision colli){
	
		if(colli.gameObject.tag == "wall")
			Destroy(gameObject);

		if(colli.gameObject.tag == "Player"){
			colli.gameObject.BroadcastMessage("ApplyDmg", 20);
			Destroy(gameObject);
		}
	}
}
