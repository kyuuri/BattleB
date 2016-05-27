using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Timer : NetworkBehaviour {

	//[SyncVar(hook = "OnChangeHealth")]
	public float currentTime = 120;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
