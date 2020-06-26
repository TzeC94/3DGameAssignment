using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuUIScript : MonoBehaviour {

	public Animator creditAnimator;
	public AudioClip buttonSound;

	public void selectSinglePlayer(){
		audio.PlayOneShot(buttonSound);	
		switchScene(2);
	}

	public void selectMultiplayer(){
		audio.PlayOneShot(buttonSound);	
		switchScene(1);
	}

	public void selectCredit(){
		audio.PlayOneShot(buttonSound);	
		creditAnimator.enabled = true;

		if (creditAnimator.GetBool ("isHidden") == false) {
			creditAnimator.SetBool ("isHidden", true);
		} else {
			creditAnimator.SetBool ("isHidden", false);
		}
	}

	public void selectExit(){
		audio.PlayOneShot(buttonSound);	
		Application.Quit();
	}

	void switchScene(int which){
		Application.LoadLevel(which);
	}
}
