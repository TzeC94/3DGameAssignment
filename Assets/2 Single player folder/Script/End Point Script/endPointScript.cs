using UnityEngine;
using System.Collections;

public class endPointScript : MonoBehaviour {

	void OnTriggerEnter(Collider colli){
		if (colli.gameObject.tag == "Player") {
			Screen.showCursor = true;
			Application.LoadLevel("cut scene");
		}
	}
}
