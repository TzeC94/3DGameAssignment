using UnityEngine;
using System.Collections;

public class shellManagerSP : MonoBehaviour {

	public Transform masterPosition;
	
	// Use this for initialization
	void Start () {
		Destroy(gameObject, 4.0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(masterPosition.position.x, masterPosition.position.y + 1.0f, masterPosition.position.z);
	}
	
	void OnTriggerEnter(Collider colli){
		if(colli.gameObject.tag == "fireball"){
			Destroy(gameObject);
			Destroy(colli.gameObject);
		}
	}
}
