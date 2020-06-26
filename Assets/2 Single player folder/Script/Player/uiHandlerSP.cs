using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class uiHandlerSP : MonoBehaviour {

	//public GameObject character;
	public characterControlSP CC;

	
	//public float totalTime;
	public Text timeText;
	public Text hpText;
	public Text deathText;
	
	public Image skill1;
	public Image skill2;
	public Image skill3;
	public Text skill1CDText;
	public Text skill2CDText;
	public Text skill3CDText;
	
	public Animator pausePanel;
	public static Animator menuPanel;

	public AudioClip pauseSound;
	
	// Use this for initialization
	void Start () {

		hpText.text = CC.charcterHP.ToString("0");

		menuPanel = pausePanel;

		Screen.showCursor = false;
	}
	
	void FixedUpdate(){
		hpText.text = CC.charcterHP.ToString("0");

		
		//to update the current spell status
		switch(CC.currentSpell){
		case 1:
			skill1.color = Color.white;
			skill2.color = Color.grey;
			skill3.color = Color.grey;
			break;
		case 2:
			skill1.color = Color.grey;
			skill2.color = Color.white;
			skill3.color = Color.grey;
			break;
		case 3:
			skill1.color = Color.grey;
			skill2.color = Color.grey;
			skill3.color = Color.white;
			break;
		}
		
		//cool down counter
		if(CC.countSkill1CD > 1.0f)
			skill1CDText.text = CC.countSkill1CD.ToString("0");
		else
			skill1CDText.text = "";
		
		if(CC.countSkill2CD > 1.0f)
			skill2CDText.text = CC.countSkill2CD.ToString("0");
		else
			skill2CDText.text = "";
		
		if(CC.countSkill3CD > 1.0f)
			skill3CDText.text = CC.countSkill3CD.ToString("0");
		else
			skill3CDText.text = "";
	}
	
	public static void selectPauseMenuButton(){
		menuPanel.enabled = true;
		menuPanel.SetBool("isHidden", true);

		Screen.showCursor = true;
		Screen.lockCursor = false;
	}
	
	public static void selectResumeButton(){
		menuPanel.SetBool("isHidden", false);

		Screen.showCursor = false;
	}
	
	public void selectExit(){
		Application.LoadLevel(0);
		Destroy(GameObject.Find("Music Box"));
	}
}
