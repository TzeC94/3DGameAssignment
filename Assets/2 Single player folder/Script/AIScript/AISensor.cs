using UnityEngine;
using System.Collections;

public class AISensor : MonoBehaviour {

	AIscripting AIS;
	GameObject myAI;

	bool targetDetected=false;
	Transform playerPos;

	// Use this for initialization
	void Start () {
		//AIS = ;
		myAI = GetComponentInParent<AIscripting>().transform.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(targetDetected){
			Vector3 newPosition = playerPos.position - myAI.transform.position;
			myAI.transform.rotation = Quaternion.Slerp(myAI.transform.rotation, Quaternion.LookRotation(newPosition), 5.0f * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider colli){
		if(colli.gameObject.tag == "Player"){
			targetDetected = true;
			playerPos = colli.gameObject.transform;
		}
	}

	void OnTriggerExit(Collider colli){
		if(colli.gameObject.tag == "Player"){
			targetDetected = false;
		}
	}
}
