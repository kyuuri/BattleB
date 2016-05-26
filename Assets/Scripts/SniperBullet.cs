using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SniperBullet : NetworkBehaviour {

	public float damage;
	private PlayerController firingPlayer;
	private int counter;

	void Update(){
		if (counter < 10) {
			counter++;
			GetComponent<Rigidbody> ().velocity = transform.forward * 7;
		}
		else if (counter == 10){
			counter++;
			GetComponent<Rigidbody> ().velocity = transform.forward * 2.1f;
		}
		else if (GetComponent<Rigidbody> ().velocity.z <= 400) {
			GetComponent<Rigidbody> ().velocity *= 1.021f;
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.tag == "Player" ){
			var hit = collider.gameObject;
			var health = hit.GetComponent<Health>();
			if (health  != null)
			{
				bool isDead = false;
				float exp = health.TakeDamage (damage, ref isDead);

				firingPlayer.status.exp -= exp / 2.5f;

				if (isDead) {
					firingPlayer.score++;
					firingPlayer.status.exp -= 30;
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
