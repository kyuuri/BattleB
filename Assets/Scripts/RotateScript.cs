using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

	public float xRotate = 0;
	public float yRotate = 0;
	public float zRotate = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (new Vector3(xRotate * Time.deltaTime, yRotate * Time.deltaTime, zRotate * Time.deltaTime));
	}
}
