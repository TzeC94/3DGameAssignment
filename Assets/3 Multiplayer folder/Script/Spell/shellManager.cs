using UnityEngine;
using System.Collections;

public class shellManager : MonoBehaviour {

	public Transform masterPosition;
	private float destroyCounter = 0.0f;

	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			transform.position = new Vector3(masterPosition.position.x, masterPosition.position.y + 1.0f, masterPosition.position.z);

			if(destroyCounter >= 4.0f){
				Network.Destroy(gameObject);
			}
			else
				destroyCounter += Time.deltaTime;

		}
	}

	void OnTriggerEnter(Collider colli){
		if(colli.gameObject.tag == "fireball"){
			Network.Destroy(gameObject);
			Network.Destroy(colli.gameObject);
		}
	}
}
