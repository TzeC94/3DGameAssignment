using UnityEngine;
using System.Collections;

public class AIscripting : MonoBehaviour {

	private float hp = 100.0f;
	public float moveSpeed;
	public float rotationSpeed;
	public float rotationSpeedMultiple;
	
	[HideInInspector]
	public bool gameEnd = false;
	
	//Skill CD time variable
	public float skill1CD;
	[HideInInspector]
	public float countSkill1CD=0.0f;


	//Script pointer
	fireHoleScriptSP FHS;
	
	public GameObject shellObject;

	private bool isDead = false;

	public AudioClip deathSound;
	private float deathTimer = 0.0f;

	public SkinnedMeshRenderer skin;

	// Use this for initialization
	void Start () {
		FHS = GetComponentInChildren<fireHoleScriptSP>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!gameEnd){
			AIBehaviour();
	
			//Skill cooldown counter script
			if(countSkill1CD >= 0.0f){
				countSkill1CD -= Time.deltaTime;
			}
			
			//if no hp then death counter plus 1
			if(hp <= 0.0f && !isDead)
			{
				audio.PlayOneShot(deathSound);

				isDead = true;

				skin.enabled = false;

			}

			if(isDead)
			{
				if(deathTimer < 0.4f)
				{
					deathTimer += Time.deltaTime;
				}
				else
					Destroy(gameObject);
			}
			
		}
	}

	//AI behaviour
	void AIBehaviour(){

		RaycastHit hit;

		Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward * 100.0f);

		if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out hit, 100.0f)){

			if(hit.collider.tag == "Player" || hit.collider.tag == "sheild"){
				transform.Translate(new Vector3(0.0f, 0.0f, 7.0f * Time.deltaTime));
				if(countSkill1CD <= 0.5f){

					FHS.fireTheBall();
					countSkill1CD = skill1CD;
				}
			}
		}
	}

	//Broadcast function
	void ApplyDmg(float dmg){
		hp -= dmg;
	}

	void OnCollisionEnter(Collision colli){
		if (colli.gameObject.tag == "lava") {
			Destroy(gameObject);
		}
	}
}
