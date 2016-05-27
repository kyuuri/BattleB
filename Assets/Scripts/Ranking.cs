using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ranking : MonoBehaviour {
	public GameObject[] allPlayer;
	// Use this for initialization
	void Start () {
		int[] scoreList = GlobalData.allPlayerScore;
		string[] nameList = GlobalData.allPlayersName;
		Debug.Log (scoreList [0]);
		Debug.Log (scoreList [1]);
//		Debug.Log (allPlayer.Length);
//		for (int index = 0; index < 10; index++) {
//			PlayerController player = allPlayer [index].GetComponent<PlayerController> ();
////			GameObject.Find ("Score (" + index + ")").GetComponent<Text> ().text;
//			Debug.Log(player.name+" = "+player.score);
//		}

	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
