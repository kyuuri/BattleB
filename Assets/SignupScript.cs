using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SignupScript : MonoBehaviour {
	private string url = "http://127.0.0.1:3000/users";
	public InputField userName;
	public InputField password;
	public InputField rePassword;
	public Button signupBtn;
	public Button cancelBtn;
	private bool isExist;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Signup(){
		string user = userName.text;
		string pw = password.text;
		string rePw = rePassword.text;

		Debug.Log ("hello");
		if (user == "" || pw == "" || rePw == "") {
			Debug.Log ("Uncomplete Input");
		} else {
			CheckExistingUser (user, pw, rePw);
		}

//		else if (CheckExistingUser(user)) {
//			Debug.Log ("This user name already exist");
//		} else if (pw == rePw) {
//			string encryptPw = Md5Sum (pw);
//			PostToDB (user, encryptPw);
//		}
	}

	private void CheckExistingUser(string user, string pw, string rePw){
		isExist = false;
		HTTP.Request someRequest = new HTTP.Request( "get", url);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
//			accessData(thing);
			for(int i = 0; i < thing.list.Count; i++){
				JSONObject j = thing.list[i];
				JSONObject userName = j["UserName"];
				Debug.Log("check leaw");
				if(user == userName.str){
					isExist = true;
				}
			}
			if(isExist){
				Debug.Log ("This user name already exist");
			} else if (pw == rePw) {
				string encryptPw = Md5Sum (pw);
				PostToDB (user, encryptPw);
			}
		});
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
