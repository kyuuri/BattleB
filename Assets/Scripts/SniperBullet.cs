using UnityEngine;
using System.Collections;

public class SniperBullet : MonoBehaviour {

	void Update(){
		if (GetComponent<Rigidbody> ().velocity.z <= 400) {
			GetComponent<Rigidbody> ().velocity *= 1.025f;
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
