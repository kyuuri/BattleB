using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {

	public float maxHealth = 100;

	public bool destroyOnDeath;

	[SyncVar(hook = "OnChangeHealth")]
	public float currentHealth = 100;

	public RectTransform healthBar;

	public void TakeDamage(float amount, int playerId)
	{
		if (!isServer)
			return;

		currentHealth -= amount;

		if (currentHealth <= 0) {
			float exp = currentHealth + amount;

			SendEXP (exp/3.0f + 20, playerId);
			SendScore (playerId);
			
			if (destroyOnDeath) {
				Destroy (gameObject);
			} else {
				currentHealth = maxHealth;

				// called on the Server, will be invoked on the Clients
				RpcRespawn ();
			}
		} else {
			SendEXP (amount / 3.0f, playerId);
		}
	}

	void SendEXP(float exp, int id){
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < allPlayers.Length; i++) {
			PlayerController tmpPlayer = allPlayers [i].GetComponent<PlayerController> ();

			if (tmpPlayer.playerId == id) {
				tmpPlayer.RpcAddExp(exp);
			}
		}
	}

	void SendScore(int id){
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < allPlayers.Length; i++) {
			PlayerController tmpPlayer = allPlayers [i].GetComponent<PlayerController> ();

			if (tmpPlayer.playerId == id) {
				tmpPlayer.RpcAddScore ();
			}
		}
	}

	void OnChangeHealth (float currentHealth)
	{
		this.currentHealth = currentHealth;
		healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{        if (isLocalPlayer)
		{
			// Set the player’s position to origin
			transform.position = new Vector3 (Random.Range (-5, 5), 0, Random.Range (-5, 5));
		}
	}
}