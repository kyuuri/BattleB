using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerTest : NetworkBehaviour {
	public const int maxHealth = 100;

	public bool destroyOnDeath;

	public GameObject[] list;

	[SyncVar]
	public int currentHealth = maxHealth;

	[SyncVar]
	public int count = 0;

	public RectTransform healthBar;

	public void Start(){
		list = GameObject.FindGameObjectsWithTag ("Player");
	}

	public void Update(){
		if (isServer) {

		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			list = GameObject.FindGameObjectsWithTag ("Player");
			++count;
		}
	}

	public void TakeDamage(int amount)
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

	[ClientRpc]
	void RpcRespawn()
	{        if (isLocalPlayer)
		{
			// Set the player’s position to origin
			transform.position = Vector3.zero;
		}
	}
}