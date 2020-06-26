using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class networkManagerScript : MonoBehaviour {

	//The game name, not the room name
	public string gameName;

	//public string roomName;
	public int numberOfPlayer;

	//Structure array store the room list get from server
	private HostData[] hostData = null;
	
	bool refreshingList = false;
	bool isRandomJoin = false;
	bool updateTheRoomList = false;

	//Use for instantiate, enable and disable some gameobject
	public GameObject player;
	public GameObject userInterface;
	public GameObject tempCamera;
	public GameObject gameplaymanaging;

	//Player spawn point
	public GameObject[] spawnPoint = new GameObject[4];

	//Getting UI element
	public Text nameTexter;
	public Text roomName;
	public Button refreshButton;
	private float unInteracableTime = 0.0f;
	public GameObject roomInformationContent;
	public GameObject roomButton;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		//Geting the available game list from server
		if(refreshingList){

			if(MasterServer.PollHostList().Length > 0){

				refreshingList = false;
				hostData = MasterServer.PollHostList();

				if(isRandomJoin){	//If user select random join game
					if(hostData.Length == 0)
						Network.Connect(hostData[0]);
					else{
						int gameNumber = Random.Range(0, hostData.Length);
						Network.Connect(hostData[gameNumber]);
					}
					isRandomJoin = false;
				}
				else if(updateTheRoomList){	//If game are available then update the list by destroy all the previous game information

					foreach (Transform childTransform in roomInformationContent.transform) {
						Destroy(childTransform.gameObject);
					}
					
					if(!hostData.Equals(null)){
						for(int i = 0;i < hostData.Length;i++){
							GameObject childButton = Instantiate(roomButton) as GameObject;
							
							Text roomNameTextChild = childButton.GetComponentInChildren<Text>();
							roomNameTextChild.text = hostData[i].gameName + "     " 
								+ hostData[i].connectedPlayers.ToString() + "/5";
							
							roomInformation RI = childButton.GetComponent<roomInformation>();
							RI.roomHostData = hostData[i];
							
							childButton.transform.SetParent(roomInformationContent.transform);
						}

						updateTheRoomList = false;
					}
				}
			}
			else{
				foreach (Transform childTransform in roomInformationContent.transform) {
					Destroy(childTransform.gameObject);
				}
			}
		}

		if (!refreshButton.interactable) {
			if(unInteracableTime <= 2.0f){
				unInteracableTime += Time.deltaTime;
			}
			else{
				unInteracableTime = 0.0f;
				refreshButton.interactable = true;
			}
		}
	}

	public void startServer(){

		int port = Random.Range(1000, 9000);

		if(roomName.text.Length == 0){
			string tempRoomName = "unamedGame" + Random.Range(0, 100).ToString();
			Network.InitializeServer(numberOfPlayer, port, !Network.HavePublicAddress());
			MasterServer.RegisterHost(gameName, tempRoomName, "Multiplayer game room");
		}
		else{
			Network.InitializeServer(numberOfPlayer, port, !Network.HavePublicAddress());
			MasterServer.RegisterHost(gameName, roomName.text, "Multiplayer game room");
		}
	}

	public void refreshAndJoinHost(){
		hostData = null;

		MasterServer.RequestHostList(gameName);
		refreshingList = true;
		isRandomJoin = true;
	}

	public void refreshHostList(){
		hostData = null;

		MasterServer.RequestHostList(gameName);
		refreshButton.interactable = false;
		refreshingList = true;
		updateTheRoomList = true;
	}

	public void backMainMenu(){
		if(GameObject.Find("Music Box")){
			Destroy(GameObject.Find("Music Box"));
		}
		Application.LoadLevel(0);
	}

	//network class callback function
	void OnServerInitialized(){
		Debug.Log("Server Created");
	}

	void OnMasterServerEvent(MasterServerEvent msEvent) {
		//this only this 1
		if (msEvent == MasterServerEvent.RegistrationSucceeded){
			Debug.Log("Server registered");
			spawnPlayer();
			gameObject.SetActive(false);
		}
	}

	void OnConnectedToServer(){
		spawnPlayer();
		gameObject.SetActive(false);
	}

	void spawnPlayer(){
		tempCamera.SetActive(false);

		int positionOfSpawn = Random.Range(0, 5);

		//Network spawn the player
		GameObject playerObject = Network.Instantiate(player, 
		                                              spawnPoint[positionOfSpawn].transform.position, 
		                                              spawnPoint[positionOfSpawn].transform.rotation, 0) as GameObject;
		characterControl cc = playerObject.GetComponent<characterControl>();


		//Enable the user interface
		userInterface.SetActive(true);
		UIHandler UH = userInterface.GetComponent<UIHandler>();	//Get pointer from user interface
		UH.CC = cc;	//Set the obect

		//Enable the gamemanager
		gameplayManager GM = gameplaymanaging.GetComponent<gameplayManager>();
		GM.playerObject = cc;

		string newName;
		if(nameTexter.text == "NoName"){
			newName = "No" + Random.Range(0, 100);
			GM.playerName = newName;
		}
		else
			GM.playerName = nameTexter.text;

		cc.gm = GM;
		gameplaymanaging.SetActive (true);
	}
}
