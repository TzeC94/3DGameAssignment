using UnityEngine;
using System.Collections;

public class fireHoleScriptSP : MonoBehaviour {

	public GameObject fireball;
	public float fireBallSpeed;
	public AudioClip fireSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fireTheBall(){
		audio.PlayOneShot (fireSound);
		Instantiate(fireball, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
	}
}
