#pragma strict

private var spinSpeed : float = 100.0f;
private var timeCounter:float = 0.0f;

function Update () {
	transform.Rotate(spinSpeed * Time.deltaTime, -spinSpeed * Time.deltaTime, spinSpeed * Time.deltaTime);
	
	if(timeCounter >= 5.0f){
		Screen.lockCursor = false;
		Screen.showCursor = true;
		Application.LoadLevel(0);
	}
	else{
		timeCounter += Time.deltaTime;
	}
}