using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BladeScript : NetworkBehaviour {

	public float damage;
	private PlayerController firingPlayer;

	void Damage(Collider collider){
		if(collider.transform.tag == "Player" ){
			var hit = collider.gameObject;
			var health = hit.GetComponent<Health>();
			if (health  != null)
			{
				bool isDead = false;
				float exp = health.TakeDamage (damage, ref isDead);

				firingPlayer.status.exp -= exp / 2;

				if (isDead) {
					firingPlayer.score++;
					firingPlayer.status.exp -= 50;
				}
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

	void OnTriggerEnter(Collider collider)
	{
		Damage (collider);
	}

	void OnTriggerStay(Collider collider)
	{
		Damage (collider);
	}
}
