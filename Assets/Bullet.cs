using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet :  NetworkBehaviour{

	public float damage = 10;
	private PlayerController firingPlayer;

	void OnTriggerEnter(Collider collider)
	{
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
			Destroy(gameObject);
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

}