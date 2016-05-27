using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

	public Text lv;
	public Text kill;
	public RectTransform lvProgress;
	public RectTransform skillProgress1;
	public RectTransform skillProgress2;
	public RectTransform skillProgress3;
	public RectTransform skillProgress4;
	public Transform tabPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Tab)) {
			tabPanel.gameObject.SetActive (true);
		} else {
			tabPanel.gameObject.SetActive (false);
		}

		lv.text = "LV "+GlobalData.lv;
		kill.text = "Kill " + GlobalData.kill;
		lvProgress.sizeDelta = new Vector2(GlobalData.exp/GlobalData.maxExp, lvProgress.sizeDelta.y);
		skillProgress1.sizeDelta = new Vector2 (skillProgress1.sizeDelta.x,GlobalData.statProgress1 / 7);
		skillProgress2.sizeDelta = new Vector2 (skillProgress2.sizeDelta.x,GlobalData.statProgress2 / 7);
		skillProgress3.sizeDelta = new Vector2 (skillProgress3.sizeDelta.x,GlobalData.statProgress3 / 7);
		skillProgress4.sizeDelta = new Vector2 (skillProgress4.sizeDelta.x,GlobalData.statProgress4 / 7);


	}
}
