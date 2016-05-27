using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

	public Camera camera;

	public Text lv;
	public Text kill;
	public RectTransform bg;
	public RectTransform lvProgress;
	public RectTransform skillProgress1;
	public RectTransform skillProgress2;
	public RectTransform skillProgress3;
	public RectTransform skillProgress4;
	public Transform tabPanel;
	public Text unityTime;

	// Use this for initialization
	void Start () {
		bg.sizeDelta = new Vector2(Screen.width, 20);
		lvProgress.sizeDelta = new Vector2(Screen.width, 20);

		skillProgress1.sizeDelta = new Vector2 (10, Screen.height / 5);
		skillProgress2.sizeDelta = new Vector2 (10, Screen.height / 5);
		skillProgress3.sizeDelta = new Vector2 (10, Screen.height / 5);
		skillProgress4.sizeDelta = new Vector2 (10, Screen.height / 5);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Tab)) {
			tabPanel.gameObject.SetActive (true);
		} else {
			tabPanel.gameObject.SetActive (false);
		}

		lv.text = "LV "+ GlobalData.lv;
		kill.text = "Kill " + GlobalData.kill;
		lvProgress.sizeDelta = new Vector2((1 - (GlobalData.maxExp - GlobalData.exp)/ GlobalData.maxExp) * Screen.width, lvProgress.sizeDelta.y);
		Debug.Log ("need exp = " + GlobalData.exp);
		Debug.Log ("max exp = " + GlobalData.maxExp);
		Debug.Log (lvProgress.sizeDelta);

		Debug.Log (GlobalData.statProgress1);
		skillProgress1.sizeDelta = new Vector2 (skillProgress1.sizeDelta.x,(1 - GlobalData.statProgress1 / 7 )* Screen.height / 5);
		skillProgress2.sizeDelta = new Vector2 (skillProgress2.sizeDelta.x,(1 - GlobalData.statProgress2 / 7 )* Screen.height / 5);
		skillProgress3.sizeDelta = new Vector2 (skillProgress3.sizeDelta.x,(1 - GlobalData.statProgress3 / 7 )* Screen.height / 5);
		skillProgress4.sizeDelta = new Vector2 (skillProgress4.sizeDelta.x,(1 - GlobalData.statProgress4 / 7 )* Screen.height / 5);

		int temp = (int)(GlobalData.unityFinalTime - GlobalData.unityTime);
		unityTime.text = temp + "";
	}
}
