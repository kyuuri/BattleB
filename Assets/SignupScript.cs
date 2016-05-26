using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SignupScript : MonoBehaviour {
	public InputField userName;
	public InputField password;
	public InputField rePassword;
	public Button signupBtn;
	public Button cancelBtn;
	private bool isExist;
	public Text responseText;

	private HttpControllerScript httpController;

	// Use this for initialization
	void Start () {
		httpController = new HttpControllerScript ();
		responseText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public void Signup(){
		string user = userName.text;
		string pw = password.text;
		string rePw = rePassword.text;

		if (user == "" || pw == "" || rePw == "") {
			Debug.Log ("Uncomplete Input");
		} else {
			httpController.CreateUser (this, user, pw, rePw);
		}

//		else if (CheckExistingUser(user)) {
//			Debug.Log ("This user name already exist");
//		} else if (pw == rePw) {
//			string encryptPw = Md5Sum (pw);
//			PostToDB (user, encryptPw);
//		}
	}

	public void SetResponseText(string text){
		responseText.enabled = true;
		responseText.text = text;
	}

	public void SignedUpSuccess(){
		userName.text = "";
		password.text = "";
		rePassword.text = "";
		responseText.text = "Successful signup";
	}

	public void GoBack(){
		Application.LoadLevel ("LoginScene");
	}
}
