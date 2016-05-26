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

	public void TakeDamage(float amount)
	{
		if (!isServer)
			return;

		currentHealth -= amount;

		if (currentHealth <= 0)
		{
			if (destroyOnDeath)
			{
				Destroy(gameObject);
			} 
			else
			{
				currentHealth = maxHealth;

				// called on the Server, will be invoked on the Clients
				RpcRespawn();
			}
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