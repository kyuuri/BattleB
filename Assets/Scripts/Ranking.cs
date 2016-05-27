using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Ranking : MonoBehaviour {
	public GameObject[] allPlayer;
	private string[,] listUsers;
	public int userNum = 10;
	private HttpControllerScript httpController;

	// Use this for initialization
	void Start () {
		httpController = new HttpControllerScript ();
		int[] scoreList = GlobalData.allPlayerScore;
		string[] nameList = GlobalData.allPlayersName;
		if (scoreList != null) {
			userNum = scoreList.Length;
			
		
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

				Debug.Log ("index " + i + " " + nameList [i] + " " + scoreList [i]);
			}
			
			for (int i = scoreList.Length - 1; i >= 0; i--) {
				Debug.Log (i);
				GameObject.Find ("Score (" + (userNum - i - 1) + ")").GetComponent<Text> ().text = scoreList [i] + "";
				GameObject.Find ("Name (" + (userNum - i - 1) + ")").GetComponent<Text> ().text = nameList [i];
			}

			for (int i = 0; i < userNum; i++) {
				httpController.PutScore (nameList [i], scoreList [i]);
			}
		}


	}

	// Update is called once per frame
	void Update () {


	}

	private void CombineNameWithScore(string[] names, int[] scores){
		Debug.Log (name.Length);

	}
	public void GoToLobby(){
		Application.LoadLevel ("NetworkLobby");
	}
}