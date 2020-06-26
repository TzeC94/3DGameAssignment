using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class characterControl : MonoBehaviour {

	public float charcterHP;

	public float moveSpeed;
	private float lavaMoveSpeed;
	public float rotationSpeed;
	public float moveSpeedMultiple;

	//variable for spell object
	public float thridSkillForce;
	private float thirdSkillLastTime;
	public AudioClip thidSkillSound;

	//to record the hori and verti data
	private float hori = 0.0f, verti = 0.0f;

	//use to store the orignal value
	private float oriMoveSpeed;
	//use to record the player current standing position
	private string currentPosition;

	//current spell variable
	public int currentSpell;

	[HideInInspector]
	public bool gameEnd = false;

	//Skill CD time variable
	public float skill1CD;
	public float skill2CD;
	public float skill3CD;
	[HideInInspector]
	public float countSkill1CD=0.0f;
	[HideInInspector]
	public float countSkill2CD=0.0f;
	[HideInInspector]
	public float countSkill3CD=0.0f;

	//Respawn point
	private GameObject[] respawnPoint = new GameObject[6];
	private int respawnPort;

	[HideInInspector]
	public gameplayManager gm;
	//Shell gameobject
	public GameObject shellObject;
	//testing the networking variable
	public GameObject myCamera;
	//Script pointer
	fireHoleScript FHS;

	private Animation AM;

	//For spawning effect
	private bool newSpawn = true;
	private float spawnFlashCounter = 0.0f;
	private int flashCounter = 0;
	public GameObject meshObject;

	public AudioClip deathSound;

	// Use this for initialization
	void Start () {

		oriMoveSpeed = moveSpeed;

		FHS = GetComponentInChildren<fireHoleScript>();
		AudioListener AL = myCamera.GetComponent<AudioListener>();
		AM = GetComponent<Animation> ();
		if(!networkView.isMine)
			AL.enabled = false;

		lavaMoveSpeed = moveSpeed / 1.7f;

		string pointName;

		for(int i = 0; i < 6; i++){
			pointName = "Spawn Point " + (i+1);
			respawnPoint[i] = GameObject.Find(pointName);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(networkView.isMine){
			myCamera.SetActive(true);
		
			if(newSpawn){
				if(spawnFlashCounter < 1.5f){
					if(flashCounter == 8){
						meshObject.SetActive(!meshObject.activeSelf);
						flashCounter = 0;
					}
					else{
						flashCounter++;
					}
					spawnFlashCounter += Time.deltaTime;
				}
				else{
					meshObject.SetActive(true);
					newSpawn = false;
					spawnFlashCounter = 0.0f;
				}
			}

			if(!gameEnd){
				keyDetection();
				//Skill cooldown counter script
				if(countSkill1CD >= 0.0f){
					countSkill1CD -= Time.deltaTime;
				}
				if(countSkill2CD >= 0.0f){
					countSkill2CD -= Time.deltaTime;
				}
				if(countSkill3CD >= 0.0f){
					countSkill3CD -= Time.deltaTime;
				}

				//If on lava then minus the HP
				if(currentPosition == "lava")
					charcterHP -= (5.0f * Time.deltaTime);

				//if no hp then death counter plus 1
				if(charcterHP <= 0.0f){
					audio.PlayOneShot(deathSound);
					charcterHP = 100.0f;
					gm.deathCounter += 1;
					respawnPort = Random.Range(0, 6);
					transform.position = new Vector3(respawnPoint[respawnPort].transform.position.x,
					                                 respawnPoint[respawnPort].transform.position.y + 1.0f,
					                                 respawnPoint[respawnPort].transform.position.z);
					newSpawn = true;
				}
			}
		}
		else{
			myCamera.SetActive(false);
		}
		
	}

	void ApplyDmg(int dmg){
		charcterHP -= dmg;
	}

	void keyDetection(){
		//Key to get decide rotation speed, SHIFT KEY
		if(Input.GetButton("RotateFaster")){
			moveSpeed = oriMoveSpeed + oriMoveSpeed * moveSpeedMultiple;
		}
		else{
			moveSpeed = oriMoveSpeed;
		}
		
		//TO detect current skill
		if(Input.GetButtonDown("firstSkill")){
			currentSpell = 1;
		}
		if(Input.GetButtonDown("secondSkill")){
			currentSpell = 2;
		}
		if(Input.GetButtonDown("thirdSkill")){
			currentSpell = 3;
		}
		
		//Fire key
		if(Input.GetButtonDown("Fire1")){
			if(currentSpell == 1){
				if(countSkill1CD <= 0.0f){
					FHS.fireTheBall();
					countSkill1CD = skill1CD;
				}
			}
			else if(currentSpell == 2){
				if(countSkill2CD <= 0.0f){
					GameObject SO = Network.Instantiate(shellObject, new Vector3(transform.position.x, 
					                                                             transform.position.y + 1.0f,
					                                                             transform.position.z), 
					                                    Quaternion.identity, 0) as GameObject;
					shellManager SM = SO.GetComponent<shellManager>();
					SM.masterPosition = transform;
					countSkill2CD = skill2CD;
				}
			}
			else if(currentSpell == 3){
				if(countSkill3CD <= 0.0f){
					thirdSkillLastTime = 1.0f;
					rigidbody.velocity = transform.forward * thridSkillForce * Time.deltaTime;
					audio.PlayOneShot(thidSkillSound);
					countSkill3CD = skill3CD;
				}
			}
		}

		if(thirdSkillLastTime > 0.0f){
			thirdSkillLastTime -= Time.deltaTime;
		}

		hori = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
		verti = Input.GetAxis("Vertical");

		if(currentPosition == "arena"){
			verti *= (moveSpeed * Time.deltaTime);
		}
		else{
			verti *= (lavaMoveSpeed * Time.deltaTime);
		}

		if (Input.GetAxis ("LeftRight") > 0) {
			transform.Translate(lavaMoveSpeed * Time.deltaTime, 0.0f, 0.0f);
		} else if (Input.GetAxis ("LeftRight") < 0) {
			transform.Translate(-lavaMoveSpeed * Time.deltaTime, 0.0f, 0.0f);
		}
		
		transform.Translate(0.0f, 0.0f, verti);
		transform.Rotate(0.0f, hori, 0.0f);

		//Animation tranform
		if (Input.GetAxis ("Vertical") != 0.0f  && !Input.GetKey(KeyCode.LeftShift)) {
			AM.Play("Walk");
		} 
		else if(Input.GetAxis ("Vertical") != 0.0f && Input.GetKey(KeyCode.LeftShift)){
			AM.Play("Run");
		}
		else{
			AM.Play("Idle");
		}
	}

	void OnCollisionEnter(Collision colli){
		if(colli.gameObject.tag == "Player"){
			if(thirdSkillLastTime > 0.0f)
				colli.gameObject.rigidbody.velocity = -colli.gameObject.transform.forward * thridSkillForce * Time.deltaTime;
		}

		if (colli.gameObject.tag == "arena") {
			currentPosition = "arena";
		}

		if (colli.gameObject.tag == "lava") {
			currentPosition = "lava";
		}
	}
}
