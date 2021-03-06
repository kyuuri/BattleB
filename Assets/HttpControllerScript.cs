﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HttpControllerScript {
	private string url = "https://limitless-caverns-30248.herokuapp.com/users";
	private string userName;
	private bool isExist;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateUser(SignupScript signupController, string user, string pw, string rePw){
		isExist = false;
		HTTP.Request someRequest = new HTTP.Request( "get", url);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//			accessData(thing);
			for(int i = 0; i < thing.list.Count; i++){
				JSONObject j = thing.list[i];
				JSONObject userName = j["UserName"];
				if(user == userName.str){
					isExist = true;
				}
			}
			if(isExist){
				string text = "This user name already exist";
				signupController.SetResponseText(text);
				Debug.Log (text);
			} else {
				if (pw == rePw) {
					string encryptPw = Md5Sum (pw);
					PostToDB (user, encryptPw);
					signupController.SignedUpSuccess();
				} else {
					string text = "Doesn't match password";
					signupController.SetResponseText(text);
					Debug.Log (text);
				}
			}
		});
	}



	public List<string[]> GetPlayers () {
		List<string[]> allUsers = new List<string[]> ();

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
				allUsers.Add(user);
				//				JSONObject userName = j["UserName"];
			}
		});
		return allUsers;
	}

	private void PostToDB(string userName, string encryptPw){
		Hashtable data = new Hashtable();
		data.Add( "UserName", userName );
		data.Add( "password", encryptPw );
		data.Add( "name", "kok" );
		data.Add( "score", 0 );


		// When you pass a Hashtable as the third argument, we assume you want it send as JSON-encoded
		// data.  We'll encode it to JSON for you and set the Content-Type header to application/json
		HTTP.Request theRequest = new HTTP.Request( "post", url, data );
		theRequest.Send( ( request ) => {

			// we provide Object and Array convenience methods that attempt to parse the response as JSON
			// if the response cannot be parsed, we will return null
			// note that if you want to send json that isn't either an object ({...}) or an array ([...])
			// that you should use JSON.JsonDecode directly on the response.Text, Object and Array are
			// only provided for convenience
			Hashtable result = request.response.Object;
			if ( result == null )
			{
				Debug.LogWarning( "Could not parse JSON response!" );
				return;
			} else {
				Debug.Log("pass naja");
			}

		});
	}

	public void PutScore (string name, int score){
		Hashtable data = new Hashtable();
		data.Add( "score", score );
		string user = name;


		// When you pass a Hashtable as the third argument, we assume you want it send as JSON-encoded
		// data.  We'll encode it to JSON for you and set the Content-Type header to application/json
		HTTP.Request theRequest = new HTTP.Request( "post", url +"/" + user, data );
		theRequest.Send( ( request ) => {
			Hashtable result = request.response.Object;
			if ( result == null )
			{
				Debug.LogWarning( "Could not parse JSON response!" );
				return;
			} else {

				Debug.Log("pass naja");
			}

		});
	}

	public void CheckExistingUser(LoginControllerScript loginController,string user, string pw){
		isExist = false;
		string encryptPw = Md5Sum (pw);
		string checkEncryptPw ="";

		HTTP.Request someRequest = new HTTP.Request( "get", url);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//			accessData(thing);
			for(int i = 0; i < thing.list.Count; i++){
				JSONObject j = thing.list[i];
				JSONObject userName = j["UserName"];
				if(user == userName.str){
					isExist = true;
					JSONObject pass = j["Password"];
					checkEncryptPw = pass.str;
				}
			}
			if(isExist){
				if(encryptPw == checkEncryptPw){
					PlayerPrefs.SetString("user", user);
					Debug.Log("Logged In");
					loginController.LoggedIn();
				} else {
					string text = "Wrong Password";
					loginController.SetResponseText(text);
					Debug.Log(text);
				}
			} else {
				string text = "Invalid User";
				loginController.SetResponseText(text);
				Debug.Log(text);
			}
		});
	}

	private string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}
}
