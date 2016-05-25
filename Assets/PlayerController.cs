using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	public enum PlayerClass {NOVICE, SHOTGUN, CANNON, BLADER, SNIPER};

	public PlayerClass playerClass = PlayerClass.SHOTGUN;

	public GameObject bulletPrefab;
	public GameObject bladePrefab;
	public GameObject sniperBulletPrefab;


	public Transform bulletSpawn;
	public bool blade = false;

	public GameObject bladeObject;

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

//		transform.Rotate(0, x, 0);
		transform.Translate(x,0,0);
		transform.Translate(0, 0, z);

		if (Input.GetKeyDown(KeyCode.Space))
		{
//			++shoot;
			CmdFire();
		}

		if (!blade) {
			Destroy (bladeObject);
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

	public override void OnStartLocalPlayer ()
	{
		//GetComponent<MeshRenderer>().material.color = Color.blue;
	}

	private void NoviceFire(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

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
			Debug.Log (bs[i].transform.forward);
			float z = 1;
			if(i % 2 == 0) z = 0.988f;
			bs [i].GetComponent<Rigidbody> ().velocity = new Vector3 (0.15f * (i-1), 0, z) * 6;
			Debug.Log (new Vector3 (0, 0.5f * (i-1), z));
			NetworkServer.Spawn(bs[i]);
			Destroy(bs[i], 1.0f);
		}
	}

	private void CannonFire(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
		bullet.transform.localScale *= 3.0f;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.5f);
	}

	private void BladerFire(){

		if (!blade) {
			// Create the Bullet from the Bullet Prefab
			var bullet = (GameObject)Instantiate (
				            bladePrefab,
							bulletSpawn.position + new Vector3(0,0,1.5f),
				            bulletSpawn.rotation);

			// Spawn the bullet on the Clients
			//bullet.transform.localScale = new Vector3(0.4f,0.4f,3.5f);
			NetworkServer.Spawn (bullet);

			bullet.transform.parent = transform;
			bladeObject = bullet;

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

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 10.0f);
	}
}