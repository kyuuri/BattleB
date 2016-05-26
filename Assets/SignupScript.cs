using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SignupScript : MonoBehaviour {
	private string url = "https://limitless-caverns-30248.herokuapp.com/users";
	public InputField userName;
	public InputField password;
	public InputField rePassword;
	public Button signupBtn;
	public Button cancelBtn;
	private bool isExist;
	private HttpControllerScript httpController;

	// Use this for initialization
	void Start () {
		httpController = new HttpControllerScript ();
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
			httpController.CreateUser (user, pw, rePw);
		}

//		else if (CheckExistingUser(user)) {
//			Debug.Log ("This user name already exist");
//		} else if (pw == rePw) {
//			string encryptPw = Md5Sum (pw);
//			PostToDB (user, encryptPw);
//		}
	}
}
