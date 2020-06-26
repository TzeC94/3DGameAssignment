using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class gameplayManager : MonoBehaviour {
	
	public float gameTime = 50.0f;

	[HideInInspector]
	public string playerName;
	[HideInInspector]
	public int deathCounter = 0;
	[HideInInspector]
	public characterControl playerObject;

	//private Vector3[] mapSize = new Vector3[3];
	public GameObject[] firstArea;
	public GameObject[] secondArea;
	public GameObject[] thirdArea;

	//For calculating and storing winner and player name
	public List<string> nameList = new List<string>();
	public List<int> scoreList = new List<int>();
	private bool executed = false;
	public GameObject winnerCanvas;
	private int calStep = 0;
	private bool isTier = false;
	private int indexOfWinner = 0;

	// Use this for initialization
	void Start () {

		if(Network.isServer){
			nameList.Add(playerName);
			scoreList.Add(0);
		}
		else{
			networkView.RPC("sendNameToServer", RPCMode.Server, playerName);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//To check whether is host or joiner, if host then calculate the time and send out to other people
		if(gameTime > 0.0f){
			if(Network.isServer && !Network.isClient){
				gameTime -= Time.deltaTime;
				networkView.RPC("updateGameTime", RPCMode.Others, gameTime);
			}

			//Base of current time and resize the arena size
			if (gameTime <= 60.0f && gameTime > 40.0f) {
				for(int i = 0; i < firstArea.Length; i++){
					firstArea[i].transform.Translate(Vector3.down * 0.05f * Time.deltaTime);
				}
			}
			else if (gameTime <= 40.0f && gameTime > 20.0f) {

				for(int i = 0; i < 4; i++){
					if(firstArea[i] != null){
						Destroy(firstArea[i]);
					}
				}

				for(int i = 0; i < firstArea.Length; i++){
					secondArea[i].transform.Translate(Vector3.down * 0.05f * Time.deltaTime);
				}
			}
			else if (gameTime <= 20.0f) {

				for(int i = 0; i < 4; i++){
					if(firstArea[i] != null){
						Destroy(firstArea[i]);
					}
				}

				for(int i = 0; i < 4; i++){
					if(secondArea[i] != null){
						Destroy(secondArea[i]);
					}
				}

				for(int i = 0; i < firstArea.Length; i++){
					thirdArea[i].transform.Translate(Vector3.down * 0.05f * Time.deltaTime);
				}
			}

			if(Network.isServer){

				for(int i = 0; i < nameList.Count; i++){
					if(nameList[i].Equals(playerName)){
						scoreList[i] = deathCounter;
						break;
					}
				}
			}
			else{
				networkView.RPC("getEveryoneScore", RPCMode.Server, playerName, deathCounter);
			}

		}
		else if(gameTime <= 0.0f){
			playerObject.gameEnd = true;
			if(!executed){
			
				switch(calStep){
				case 0:
					if(Network.isServer){
						for(int i = 0; i < nameList.Count; i++){
							if(nameList[i].Equals(playerName)){
								scoreList[i] = deathCounter;
								break;
							}
						}
					}
					else{
						networkView.RPC("getEveryoneScore", RPCMode.Server, playerName, deathCounter);
					}
					if(Network.isServer)
						calStep = 1;
					else
						executed = true;

					break;
				case 1:

					int currentLowest = scoreList[0];
					if(nameList.Count != 1){
						for(int i = 1; i < nameList.Count;i++){
							if(currentLowest == scoreList[i]){
								isTier = true;
							}
							else if(currentLowest > scoreList[i]){
								indexOfWinner = i;
								currentLowest = scoreList[i];
							}
						}
					}
		
					calStep = 2;
					
					break;
				case 2:
					if(isTier){
						networkView.RPC("annouceWinner", RPCMode.AllBuffered, "gameIsTier");
					}
					else
						networkView.RPC("annouceWinner", RPCMode.AllBuffered, nameList[indexOfWinner]);

					executed = true;
					break;
				}
			}
		}
	}

	public void removeThePlayer(){
		networkView.RPC("removePlayer", RPCMode.Server, playerName);
	}

	[RPC]
	void updateGameTime(float hostTime){
		if (Network.isClient && !Network.isServer) {
			gameTime = hostTime;
		}
	}

	[RPC]
	void sendNameToServer(string name){
		nameList.Add(name);
		scoreList.Add(0);
	}

	[RPC]
	void getEveryoneScore(string msgPlayerName, int deathScore){
		for(int i = 0; i < nameList.Count; i++){
		
			if(nameList[i].Equals(msgPlayerName)){
				scoreList[i] = deathScore;
				break;
			}
		}
	}

	[RPC]
	void annouceWinner(string winnerName){
		winnerCanvas.SetActive(true);
		Text winnerText = winnerCanvas.GetComponentInChildren<Text>();

		if(winnerName.Equals("gameIsTier")){
			winnerText.text = "Game is Tier, no 1 win";
		}else{
			if(winnerName == playerName){
				winnerText.text = "You Win!!!";
			}else{
				winnerText.text = "You Lose, Winner: "+ winnerName;
			}
		}
	}

	[RPC]
	void removePlayer(string name){
		int index = nameList.IndexOf(name);
		scoreList.RemoveAt(index);
		nameList.Remove(name);
	}
}
