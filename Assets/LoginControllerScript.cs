using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginControllerScript : MonoBehaviour {
	private string url = "http://127.0.0.1:3000/users";
	public InputField userName;
	public InputField password;
	public Button signinBtn;
	public Button signupBtn;

	private bool isExist;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoginManagement(){
		string user = userName.text;
		string pw = password.text;

		CheckExistingUser (user, pw);
	}

	private void CheckExistingUser(string user, string pw){
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
					Debug.Log("Logged In");
				} else {
					Debug.Log("Worng Password");
				}
			} else {
				Debug.Log("Invalid User");
			}
		});
	}

	public  string Md5Sum(string strToEncrypt)
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
