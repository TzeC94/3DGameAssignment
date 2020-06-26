using UnityEngine;
using System.Collections;

public class characterControlSP : MonoBehaviour {

	public float charcterHP;
	public float moveSpeed;
	private float lavaMoveSpeed;
	public float rotationSpeed;
	public float moveSpeedMultiple;
	//use to store the orignal value
	private float oriMoveSpeed;
	
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
	
	//to record the hori and verti data
	private float hori = 0.0f, verti = 0.0f;
	
	//variable for spell object
	public float thridSkillForce;
	private float thirdSkillLastTime;
	public AudioClip thirdSkillSound;

	//current spell variable
	public int currentSpell;
	
	//Shell gameobject
	[HideInInspector]
	public bool requestSpawnShell;
	public GameObject shellObject;
	
	//Script pointer
	fireHoleScriptSP FHS;

	//use to record the player current standing position
	private string currentPosition = "arena";

	private Animation AM;

	public Transform startingPoint;

	public AudioClip deathSound;

	private bool inPause = false;
	public AudioClip menuSound;

	// Use this for initialization
	void Start () {
		oriMoveSpeed = moveSpeed;
		FHS = GetComponentInChildren<fireHoleScriptSP>();
		lavaMoveSpeed = moveSpeed / 1.7f;
		AM = GetComponent<Animation> ();

	}
	
	// Update is called once per frame
	void Update () {
			
		if(!gameEnd){
			keyDetection();
			//Skill cooldown counter script
			if(countSkill1CD >= 0.0f){
				countSkill1CD -= Time.deltaTime;
			}

			if(countSkill2CD >= 0.0f){
				countSkill2CD -= Time.deltaTime;
			}
			else
			{
				requestSpawnShell = false;
			}

			if(countSkill3CD >= 0.0f){
				countSkill3CD -= Time.deltaTime;
			}
			
			//If on lava then minus the HP
			if(currentPosition == "lava")
			{
				if(!requestSpawnShell)
				{
					charcterHP -= (5.0f * Time.deltaTime);
				}
			}
			
			//if no hp then death counter plus 1
			if(charcterHP <= 0.0f){
				audio.PlayOneShot(deathSound);
				transform.position = startingPoint.position;
				transform.rotation = startingPoint.rotation;
				charcterHP = 100.0f;
			}
		}
	}
	
	void ApplyDmg(int dmg){
		charcterHP -= dmg;
	}
	
	void keyDetection(){
		//Key to get decide rotation speed, SHIFT KEY
		if(Input.GetButton("RotateFaster")){
			moveSpeed = oriMoveSpeed * moveSpeedMultiple;
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
					requestSpawnShell = true;
					GameObject SO = Instantiate(shellObject, new Vector3(transform.position.x, 
					                                                             transform.position.y + 1.0f,
					                                                             transform.position.z), 
					                                    Quaternion.identity) as GameObject;
					shellManagerSP SM = SO.GetComponent<shellManagerSP>();
					SM.masterPosition = transform;
					countSkill2CD = skill2CD;
				}
			}
			else if(currentSpell == 3){
				if(countSkill3CD <= 0.0f){
					rigidbody.velocity = transform.forward * thridSkillForce * Time.deltaTime;
					audio.PlayOneShot(thirdSkillSound);
					countSkill3CD = skill3CD;
					thirdSkillLastTime = 1.0f;

				}
			}
		}

		if(Input.GetButtonDown("Cancel"))
		{
			if(!inPause)
			{
				audio.PlayOneShot(menuSound);

				uiHandlerSP.selectPauseMenuButton();

				inPause = true;
			}
			else
			{
				audio.PlayOneShot(menuSound);

				uiHandlerSP.selectResumeButton();
				
				inPause = false;
			}
		}

		if(!inPause)
		{
			Screen.lockCursor = true;
		}

		if(thirdSkillLastTime > 0.0f)
			thirdSkillLastTime -= Time.deltaTime;

		hori = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
		verti = Input.GetAxis("Vertical");
		if(currentPosition == "arena" || requestSpawnShell){
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
		if(colli.gameObject.tag == "AI"){
			if(thirdSkillLastTime > 0.0f)
				colli.gameObject.rigidbody.velocity = -colli.gameObject.transform.forward * thridSkillForce * Time.deltaTime;
		}
		
		if(colli.gameObject.tag == "arena")
			currentPosition = "arena";
		
		if(colli.gameObject.tag == "lava")
			currentPosition = "lava";
	}
}
