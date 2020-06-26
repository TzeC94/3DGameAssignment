using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gamePlayScript : MonoBehaviour {

	//Instance and variable for time
	private float timeCounter = 0.0f;
	public Text timeText;

	// Use this for initialization
	void Start () {
		timeText.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
		timeText.text = timeCounter.ToString("0");
		timeCounter += Time.deltaTime;
	}
}
