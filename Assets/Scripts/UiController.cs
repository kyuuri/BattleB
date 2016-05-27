using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

	public Camera camera;

	public Text lv;
	public Text kill;
	public RectTransform bg;
	public RectTransform lvProgress;
	public Text skillProgress1;
	public Text skillProgress2;
	public Text skillProgress3;
	public Text skillProgress4;
	public Text skillProgress5;

	public Text classProgress1;
	public Text classProgress2;
	public Text classProgress3;
	public Text classProgress4;
	public Text classProgress5;

	public Transform tabPanel;
	public Text unityTime;

	// Use this for initialization
	void Start () {
		bg.sizeDelta = new Vector2(Screen.width, 20);
		lvProgress.sizeDelta = new Vector2(Screen.width, 20);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Tab) || GlobalData.statPoint > 0) {
			tabPanel.gameObject.SetActive (true);
		} else {
			tabPanel.gameObject.SetActive (false);
		}


		skillProgress1.text = GlobalData.statProgress1+"";
		skillProgress2.text = GlobalData.statProgress2+"";
		skillProgress3.text = GlobalData.statProgress3+"";
		skillProgress4.text = GlobalData.statProgress4+"";
		skillProgress5.text = GlobalData.statPoint+"";

		classProgress1.text = GlobalData.classProgress1+"";
		classProgress2.text = GlobalData.classProgress2+"";
		classProgress3.text = GlobalData.classProgress3+"";
		classProgress4.text = GlobalData.classProgress4+"";
		classProgress5.text = GlobalData.classPoint+"";

		lv.text = "LV "+ GlobalData.lv;
		kill.text = "Kill " + GlobalData.kill;
		lvProgress.sizeDelta = new Vector2((1 - (GlobalData.maxExp - GlobalData.exp)/ GlobalData.maxExp) * Screen.width, lvProgress.sizeDelta.y);
		Debug.Log ("need exp = " + GlobalData.exp);
		Debug.Log ("max exp = " + GlobalData.maxExp);
		Debug.Log (lvProgress.sizeDelta);

		Debug.Log (GlobalData.statProgress1);
	
		int temp = (int)(GlobalData.unityFinalTime - GlobalData.unityTime);
		unityTime.text = temp + "";
	}
}
