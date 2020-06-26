using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class musicBoxScript : MonoBehaviour {

	AudioSource AS;
	int willUpdate;
	public Toggle soundToggle;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		AS = GetComponent<AudioSource> ();

		Debug.Log("called");
		if(AudioListener.pause){
			soundToggle.isOn = true;
			AudioListener.pause = true;
		}else{
			soundToggle.isOn = false;
			AudioListener.pause = false;
		}
	}

	void FixedUpdate(){
		if (willUpdate >= 10) {
			if (Application.loadedLevel == 0) {
				AS.volume = 0.6f;
			} else if (Application.loadedLevel == 1 || Application.loadedLevel == 2) {
				AS.volume = 0.25f;
			} else if (Application.loadedLevel == 3) {
				AS.volume = 0.4f;
			}
			willUpdate = 0;
		} else
			willUpdate++;
	}

	public void changeSoundSetting(){
		if(!AudioListener.pause)
			AudioListener.pause = true;
		else
			AudioListener.pause = false;
	}
}
