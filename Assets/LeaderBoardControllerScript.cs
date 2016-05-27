using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderBoardControllerScript : MonoBehaviour {
	private string url = "https://limitless-caverns-30248.herokuapp.com/users";

	private List<string[]> users;
	private HttpControllerScript httpController;

	public GameObject board;

	// Use this for initialization
	void Start () {
		board.SetActive (false);
		httpController = new HttpControllerScript ();
		GetPlayers ();
	}
	
	// Update is called once per frame
	void Update () {
	 //
	}

	void setScore(int index, string[] user){
		string userName = user [0];
		string score = user [1];
		string name = user [2];
		GameObject.Find ("Text (" + index + ")").GetComponent<Text> ().text = (index+1) + ". " + userName;
		GameObject.Find ("Score (" + index + ")").GetComponent<Text> ().text = score;
	}

	private void ShowUsers(){
		users.Sort(CompareListByScore);
		board.SetActive (true);
		for (int i = 0; i < 5; i++) {
			setScore (i, users [i]);
			Debug.Log (i);
		}
	}

	private static int CompareListByScore(string[] i1, string[] i2)
	{
		return i2[1].CompareTo(i1[1]); 
	}

	private void GetPlayers () {
		users = new List<string[]> ();

		HTTP.Request someRequest = new HTTP.Request( "get", url);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//			accessData(thing);
			for(int i = 0; i < thing.list.Count; i++){
				JSONObject j = thing.list[i];
				string[] user = new string[3];
				user[0] = j["UserName"].str;
				user[1] = j["Score"].n+""	;
				user[2] = j["Name"].str;
				users.Add(user);
				//				JSONObject userName = j["UserName"];
			}
			ShowUsers ();
		});
	}
}
