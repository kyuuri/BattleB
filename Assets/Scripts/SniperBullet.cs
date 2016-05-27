using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SniperBullet : NetworkBehaviour {

	public GameObject hitParticle;
	public float damage;
	private PlayerController firingPlayer;
	public AudioSource source;
	private int counter;
	[SyncVar]
	public int playerId;

	void Update(){
		if (playerId != 0 && firingPlayer == null) {
			CheckPlayer ();
		}

		if (counter < 10) {
			counter++;
			GetComponent<Rigidbody> ().velocity = transform.forward * 7;
		}
		else if (counter == 10){
			counter++;
			GetComponent<Rigidbody> ().velocity = transform.forward * 2.05f;
		}
		else if (GetComponent<Rigidbody> ().velocity.z <= 400) {
			GetComponent<Rigidbody> ().velocity *= 1.025f;
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.tag == "Player" ){
			int id = collider.GetComponent<PlayerController> ().playerId;
			if (id != playerId) {
				var hit = collider.gameObject;
				var health = hit.GetComponent<Health> ();
				if (health != null) {
					health.TakeDamage (damage, playerId);
				}
				var particle = (GameObject)Instantiate (
					              hitParticle,
					              transform.position, Quaternion.identity);

				Destroy (particle, 0.4f);
				if (!source.isPlaying) {
					source.Play ();
				}
				NetworkServer.Spawn (particle);
				Destroy (gameObject);
			}
		}
	}

	[Command]
	public void CmdSetPlayer(int id){
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < allPlayers.Length; i++) {
			PlayerController tmpPlayer = allPlayers [i].GetComponent<PlayerController> ();
			if (tmpPlayer.playerId == id) {
				Debug.Log (tmpPlayer.score);
				firingPlayer = tmpPlayer;
			}
		}
	}

	void CheckPlayer(){
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < allPlayers.Length; i++) {
			PlayerController tmpPlayer = allPlayers [i].GetComponent<PlayerController> ();

			if (tmpPlayer.playerId == playerId) {
				firingPlayer = tmpPlayer;
			}
		}
	}


}
