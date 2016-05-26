using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BladeScript : NetworkBehaviour {

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
