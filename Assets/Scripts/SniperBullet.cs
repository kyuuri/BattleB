using UnityEngine;
using System.Collections;

public class SniperBullet : MonoBehaviour {

	public float damage;

	void Update(){
		if (GetComponent<Rigidbody> ().velocity.z <= 400) {
			GetComponent<Rigidbody> ().velocity *= 1.03f;
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.tag == "Player" ){
			var hit = collider.gameObject;
			var health = hit.GetComponent<Health>();
			if (health  != null)
			{
				health.TakeDamage(40);
			}

			Destroy(gameObject);
		}
	}
}
