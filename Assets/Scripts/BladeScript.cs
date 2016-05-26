using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BladeScript : NetworkBehaviour {

	public GameObject hitParticle;
	public float damage;
	private PlayerController firingPlayer;
	[SyncVar]
	public int playerId;

	void Update(){
		if (playerId != 0 && firingPlayer == null) {
			CheckPlayer ();
		}
	}

	void Damage(Collider collider){
		if(collider.transform.tag == "Player" ){
			var hit = collider.gameObject;
			var health = hit.GetComponent<Health>();
			if (health  != null)
			{
				health.TakeDamage (damage, playerId);

				var particle = (GameObject)Instantiate (
					hitParticle,
					collider.transform.position - collider.transform.up*0.7f, Quaternion.identity);

				Destroy (particle, 0.2f);
				NetworkServer.Spawn(particle);
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

	void OnTriggerEnter(Collider collider)
	{
		Damage (collider);
	}

	void OnTriggerStay(Collider collider)
	{
		Damage (collider);
	}
}
