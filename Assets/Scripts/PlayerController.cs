using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{

	public enum PlayerClass {NOVICE, SHOTGUN, CANNON, BLADER, SNIPER};
	public float baseEXP = 50;
	public float maxEXP = 100;

	public int playerId;
	public int score;

	[SyncVar (hook = "OnChangeClass")]
	public PlayerClass playerClass = PlayerClass.NOVICE;

	public PlayerStatus status;

	public GameObject bulletPrefab;
	public GameObject bladePrefab;
	public GameObject sniperBulletPrefab;


	public Transform bulletSpawn;
	public bool blade = false;

	public GameObject bladeObject;

	private float fireDelay = 0;


	void Start(){
		status = new PlayerStatus (playerClass);
		playerId = (int)GetComponent<NetworkIdentity> ().netId.Value;
		GlobalData.unityTime = Network.time;
		GlobalData.unityStartTime = Network.time;
		GlobalData.unityFinalTime = Network.time + 300;

		if (isLocalPlayer) {
			transform.position = new Vector3 (Random.Range (-5, 5), 0, Random.Range (-5, 5));
		}
	}

	void Update()
	{

		if (!isLocalPlayer)
		{
			return;
		}

		GlobalData.unityTime = Network.time;
		//Debug.Log (Network.time);
		CameraScript camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();
		camera.target = gameObject;

		var hori = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
		var verti = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		Vector3 pos = transform.position;

		transform.position = new Vector3 (pos.x + hori, 0, pos.z + verti);

		LookAtMouse ();

		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (fireDelay <= 0) {
				Debug.Log ("nnnn");
				CmdFire ();
				Debug.Log ("xxxxx");
				fireDelay = status.fireSpeed;
			}
		}
		if (fireDelay > 0) {
			fireDelay -= Time.deltaTime;
		}

		if (!blade) {
			Destroy (bladeObject);
		}

		if (Input.GetKeyDown("space")) {
			status.exp -= 50;
		}

		CheckEXP ();
		UpStat ();

		if (Input.GetKeyDown ("z")) {
			CmdChangeClass (PlayerClass.SHOTGUN);
		}
		else if (Input.GetKeyDown ("c")) {
			CmdChangeClass (PlayerClass.CANNON);
		}
		else if (Input.GetKeyDown ("v")) {
			CmdChangeClass (PlayerClass.BLADER);
		}
		else if (Input.GetKeyDown ("b")) {
			CmdChangeClass (PlayerClass.SNIPER);
		}
		else if (Input.GetKeyDown ("n")) {
			CmdChangeClass (PlayerClass.NOVICE);
		}
	}

	void OnChangeClass(PlayerClass pc){
		this.playerClass = pc;
		GameObject newObject = (GameObject)Instantiate (Resources.Load(playerClass.ToString() + "Type"));
		foreach (Transform t in transform.GetComponentsInChildren<Transform>()) {
			if (t.CompareTag ("Class")) {
				Destroy (t.gameObject);
				newObject.transform.parent = transform;
				newObject.transform.localPosition = new Vector3 (0, -2.5f, 0);
				newObject.transform.localRotation = Quaternion.Euler (0, 0, 0);
				break;
			}
		}
		if (isLocalPlayer) {
			status.SetClass (pc);
		}
	}

	[Command]
	void CmdChangeClass(PlayerClass pc){
		if (isServer) {
			this.playerClass = pc;
		}
	}

	void LookAtMouse(){
		// Generate a plane that intersects the transform's position with an upwards normal.
		Plane playerPlane = new Plane(Vector3.up, transform.position);

		// Generate a ray from the cursor position
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		// Determine the point where the cursor ray intersects the plane.
		// This will be the point that the object must look towards to be looking at the mouse.
		// Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
		//   then find the point along that ray that meets that distance.  This will be the point
		//   to look at.
		float hitdist = 0.0f;
		// If the ray is parallel to the plane, Raycast will return false.
		if (playerPlane.Raycast (ray, out hitdist)) 
		{
			// Get the point along the ray that hits the calculated distance.
			Vector3 targetPoint = ray.GetPoint(hitdist);

			// Determine the target rotation.  This is the rotation if the transform looks at the target point.
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			targetRotation = Quaternion.Euler (0, targetRotation.eulerAngles.y + 90, 90);
			//			targetRotation = Quaternion.Euler (0, targetRotation.eulerAngles.y + 90, 0);
			//Debug.Log (targetRotation.eulerAngles);

			// Smoothly rotate towards the target point.
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7 * Time.deltaTime);
		}

	}

	// This [Command] code is called on the Client …
	// … but it is run on the Server!
	[Command]
	void CmdFire()
	{
		if (playerClass == PlayerClass.NOVICE) {
			NoviceFire ();
		} else if (playerClass == PlayerClass.SHOTGUN) {
			ShotGunFire ();
		} else if (playerClass == PlayerClass.CANNON) {
			CannonFire ();
		} else if (playerClass == PlayerClass.BLADER) {
			BladerFire ();
		} else if (playerClass == PlayerClass.SNIPER) {
			SniperFire ();
		}
	}

	void CheckEXP(){
		if (status.exp <= 0 && status.level < 20) {
			status.level += 1;
			if (status.level == 20) {
				status.exp = 10000.0f;
			} else {
				status.exp = (float)baseEXP * Mathf.Pow (1.2f, status.level) + status.exp;
				maxEXP = (float)baseEXP * Mathf.Pow (1.2f, status.level);

				GlobalData.exp = status.exp;
				GlobalData.maxExp = maxEXP;
			}
			status.pointLeft += 1;
			GlobalData.lv = status.level;
			GlobalData.statPoint = status.pointLeft;

			Debug.Log ("Level UP + " + status.level);
		}
	}

	void UpStat(){
		if (status.pointLeft > 0) {
			if (Input.GetKeyDown (KeyCode.Keypad1) || (Input.GetKeyDown (KeyCode.Alpha1))) {
				if (status.pointMaxHp < 7) {
					status.pointMaxHp++;
					status.pointLeft--;
					GlobalData.statPoint = status.pointLeft;
					GlobalData.statProgress1 = status.pointMaxHp;
				}
			} else if (Input.GetKeyDown (KeyCode.Keypad2) || (Input.GetKeyDown (KeyCode.Alpha2))) {
				if (status.pointMoveSpeed < 7) {
					status.pointMoveSpeed++;
					status.pointLeft--;
					GlobalData.statPoint = status.pointLeft;
					GlobalData.statProgress2 = status.pointMoveSpeed;
				}
			} else if (Input.GetKeyDown (KeyCode.Keypad3) || (Input.GetKeyDown (KeyCode.Alpha3))) {
				if (status.pointbulletSpeed < 7) {
					status.pointbulletSpeed++;
					status.pointLeft--;
					GlobalData.statPoint = status.pointLeft;
					GlobalData.statProgress3 = status.pointbulletSpeed;
				}
			} else if (Input.GetKeyDown (KeyCode.Keypad4) || (Input.GetKeyDown (KeyCode.Alpha4))) {
				if (status.pointfireSpeed < 7) {
					status.pointfireSpeed++;
					status.pointLeft--;
					GlobalData.statPoint = status.pointLeft;
					GlobalData.statProgress4 = status.pointfireSpeed;
				}
			}

			Debug.Log ("Up Stat : MHP = " + status.pointMaxHp + " MS = " + status.pointMoveSpeed + " BSpd = " + status.pointbulletSpeed + " DelayReduce = " + status.pointfireSpeed);
			Debug.Log ("PointLeft = " + status.pointLeft);
		}
	}

	private void NoviceFire(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * status.bulletSpeed;
		bullet.GetComponent<Bullet> ().damage = status.damage;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		bullet.GetComponent<Bullet> ().playerId = (int)netId.Value;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}

	private void ShotGunFire(){
		GameObject[] bs = new GameObject[3];

		for (int i = 0; i < 3; i++) {
			bs [i] = (GameObject)Instantiate (
				bulletPrefab,
				bulletSpawn.position,
				bulletSpawn.rotation);

			if (i == 0) {
				bs [i].GetComponent<Rigidbody> ().velocity = Quaternion.Euler(0,-10,0) * bs[i].transform.forward * status.bulletSpeed;
			}
			else if (i == 1) {
				bs [i].GetComponent<Rigidbody> ().velocity = bs [i].transform.forward * status.bulletSpeed;
			}
			else if (i == 2) {
				bs [i].GetComponent<Rigidbody> ().velocity = Quaternion.Euler(0,10,0) *bs [i].transform.forward * status.bulletSpeed;
			}
			bs[i].GetComponent<Bullet> ().damage = status.damage;
			NetworkServer.Spawn(bs[i]);

			bs[i].GetComponent<Bullet> ().playerId = (int)netId.Value;

			Destroy(bs[i], 0.6f);


		}
	}

	private void CannonFire(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * status.bulletSpeed;
		bullet.GetComponent<Bullet> ().damage = status.damage;

		bullet.transform.localScale *= 3.0f;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		bullet.GetComponent<Bullet> ().playerId = (int)netId.Value;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.5f);
	}

	private void BladerFire(){

		if (!blade) {
			// Create the Bullet from the Bullet Prefab
			var bullet = (GameObject)Instantiate (
				bladePrefab,
				bulletSpawn.position + bulletSpawn.transform.forward * 1.5f,
				bulletSpawn.rotation);

			// Spawn the bullet on the Clients
			//bullet.transform.localScale = new Vector3(0.4f,0.4f,3.5f);
			bullet.GetComponent<BladeScript> ().damage = status.damage;
			NetworkServer.Spawn (bullet);

			foreach (Transform ch in transform.GetComponentsInChildren<Transform>()) {
				if (ch.CompareTag ("Player")) {
					bullet.transform.parent = ch;
				}
			}
			bladeObject = bullet;

			bullet.GetComponent<BladeScript> ().playerId = (int)netId.Value;

			// Destroy the bullet after 2 seconds
		}
		blade = !blade;
	}

	private void SniperFire(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			sniperBulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward;
		bullet.GetComponent<SniperBullet> ().damage = status.damage;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		bullet.GetComponent<SniperBullet> ().playerId = (int)netId.Value;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 10.0f);
	}

	public PlayerClass CurrentPlayerClass{
		get{ return playerClass; }
		set{
			playerClass = value;
			status.SetClass (value);
		}
	}

	[ClientRpc]
	public void RpcAddExp(float exp){
		if (isLocalPlayer) {
			status.exp -= exp;
			GlobalData.exp = status.exp;
		}
	}

	[ClientRpc]
	public void RpcAddScore(){
		if (isLocalPlayer) {
			score++;
			GlobalData.kill = score;
		}
	}


	public override void OnStartLocalPlayer ()
	{
		//		GetComponent<MeshRenderer>().material.color = Color.blue;
	}
}