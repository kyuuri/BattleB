using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet :  NetworkBehaviour{

	public GameObject hitParticle;
	public GameObject cannonParticle;
	public AudioSource source;

	public float damage = 10;
	private PlayerController firingPlayer;
	[SyncVar]
	public int playerId;

	void Update(){
		if (playerId != 0 && firingPlayer == null) {
			CheckPlayer ();
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
				if (transform.transform.lossyScale.x <= 0.3f) {
					var particle = (GameObject)Instantiate (
						              hitParticle,
						              transform.position, Quaternion.identity);
			
					Destroy (particle, 0.4f);
					if (!source.isPlaying) {
						source.Play ();
					}
					NetworkServer.Spawn (particle);
				} else {
					var particle = (GameObject)Instantiate (
						              cannonParticle,
						              transform.position, Quaternion.identity);

					Destroy (particle, 0.4f);
					if (!source.isPlaying) {
						source.Play ();
					}
					NetworkServer.Spawn (particle);
				}
				Destroy (gameObject);
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

	[ClientRpc]
	public void RpcChangeBulletSize (float size){
		transform.localScale *= size;
	}

}