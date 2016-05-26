using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet :  NetworkBehaviour{

	public GameObject hitParticle;
	public GameObject cannonParticle;

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
			var hit = collider.gameObject;
			var health = hit.GetComponent<Health>();
			if (health  != null)
			{
				health.TakeDamage (damage, playerId);
			}

			var particle = (GameObject)Instantiate(
				hitParticle,
				transform.position, Quaternion.identity);
			
			Destroy (particle, 0.4f);

			NetworkServer.Spawn(particle);
			Destroy(gameObject);
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