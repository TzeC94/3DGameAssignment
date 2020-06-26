using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class tutorialHint : MonoBehaviour
{
	public Text hintText;
	public Image hintBg;
	public string hint;

	public GameObject target;
	
	public string msg;
	private float msgTimer = 0.0f;
	public float msgDrt;
	public bool destroyOnExit = true;

	void Start ()
	{
		showHint(false);

		hintText.text = hint;
	}

	void showHint(bool t)
	{
		hintText.text = hint;

		hintText.enabled = t;
		hintBg.enabled = t;
	}

	void OnTriggerEnter (Collider collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "Player")
		{
			showHint(true);
		} 
	}

	void OnTriggerExit (Collider collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "Player")
		{
			showHint(false);

			if(destroyOnExit)
			{
				Destroy(target);
			}
		}
	}


	void Update()
	{
		if(target == null && (hintText.text == hint || hintText.text == msg))
		{
			if(msgTimer < msgDrt)
			{
				showHint(true);

				hintText.text = msg;

				msgTimer += Time.deltaTime;
			}
			else
			{
				showHint(false);

				Destroy(this);
			}
		}
	}
}