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

	public float TakeDamage(float amount, ref bool isDead)
	{
		if (!isServer)
			return -1;

		currentHealth -= amount;

		if (currentHealth <= 0) {
			float exp = currentHealth + amount;
			if (destroyOnDeath) {
				Destroy (gameObject);
			} else {
				currentHealth = maxHealth;

				// called on the Server, will be invoked on the Clients
				RpcRespawn ();
			}
			isDead = true;
			return exp;
		} else {
			isDead = false;
			return amount;
		}
	}

	void OnChangeHealth (float currentHealth)
	{
		healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{        if (isLocalPlayer)
		{
			// Set the player’s position to origin
			transform.position = Vector3.zero;
		}
	}
}