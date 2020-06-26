using UnityEngine;
using System.Collections;

public class roomInformation : MonoBehaviour {

	[HideInInspector]
	public HostData roomHostData;

	public void JoinTheGame(){
		Network.Connect(roomHostData);
	}
}
