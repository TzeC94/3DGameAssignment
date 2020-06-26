using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour {

	//public GameObject character;
	[HideInInspector]
	public characterControl CC;

	//Get component gameplay manager
	public GameObject gameManager;
	private gameplayManager GM;

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

	public GameObject pausePanel;
	public GameObject disconnectUI;

	// Use this for initialization
	void Start () {
		GM = gameManager.GetComponent<gameplayManager> ();
		timeText.text = GM.gameTime.ToString("0");
		hpText.text = CC.charcterHP.ToString("0");
		deathText.text =  GM.deathCounter.ToString();
	}

	void FixedUpdate(){
		hpText.text = CC.charcterHP.ToString("0");
		deathText.text = GM.deathCounter.ToString();
		timeText.text = GM.gameTime.ToString("0");

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

	public void selectPauseMenuButton(){
		pausePanel.SetActive(true);
	}

	public void selectResumeButton(){
		pausePanel.SetActive(false);
	}

	public void selectExit(){

		Network.Disconnect();

		if(Network.isServer){
			MasterServer.UnregisterHost();

		}

		if(GameObject.Find("Music Box")){
			Destroy(GameObject.Find("Music Box"));
		}

		Application.LoadLevel(0);
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player, 0);
		Network.DestroyPlayerObjects(player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info){
		if(Network.isClient && GM.gameTime > 0.0f)
			disconnectUI.SetActive(true);
	}

	[RPC]
	void removePlayer(string name){}
}
