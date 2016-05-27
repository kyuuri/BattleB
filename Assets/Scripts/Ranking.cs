using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Ranking : MonoBehaviour {
	public GameObject[] allPlayer;
	private string[,] listUsers;
	public int userNum = 10;

	// Use this for initialization
	void Start () {
		int[] scoreList = GlobalData.allPlayerScore;
		string[] nameList = GlobalData.allPlayersName;
//		string[] nameList = new string[3];
//			int[] scoreList = new int[3];

//		for (int j = 0; j < nameList.Length; j++) {
//			Debug.Log (j);
//			listUsers [j, 0] = nameList [j];
//			listUsers [j, 1] = scoreList [j] + "";
//
//			Debug.Log ("index " + j + " " + listUsers[j ,0] + " " + listUsers [j, 1]);
//		}
//		nameList[0]= "Aof";
//		scoreList[0] = 15;
//		nameList[1] = "Kok";
//		scoreList [1] = 40;
//		nameList[2] = "Kuy";
//		scoreList [2] = 10;

		Debug.Log ("------------------");
		Array.Sort (scoreList, nameList);

		for (int i = 0; i < scoreList.Length; i++) {

			Debug.Log ("index " + i + " " + nameList[i] + " " + scoreList[i]);
		}

//		CombineNameWithScore (nameList, scoreList);
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

	private void CombineNameWithScore(string[] names, int[] scores){
		Debug.Log (name.Length);

	}
}