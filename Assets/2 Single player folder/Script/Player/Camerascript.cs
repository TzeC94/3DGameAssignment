using UnityEngine;
using System.Collections;

public class Camerascript : MonoBehaviour {
		
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	void FixedUpdate(){
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position, -transform.up, out hit, 5.0f)){
			if(hit.collider.tag == "wall"){
				anim.SetBool("changeCamera", true);
			}
		}
		else{
			anim.SetBool("changeCamera", false);
		}
	}
}
