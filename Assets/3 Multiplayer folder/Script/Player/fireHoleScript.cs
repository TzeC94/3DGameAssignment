using UnityEngine;
using System.Collections;

public class fireHoleScript : MonoBehaviour {

	public GameObject fireball;
	public AudioClip fireSound;

	public void fireTheBall(){
		audio.PlayOneShot (fireSound);
		Network.Instantiate(fireball, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation,0);
	}
}
