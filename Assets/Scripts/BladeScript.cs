using UnityEngine;
using System.Collections;

public class BladeScript : MonoBehaviour {

	void Damage(Collider collider){
		if(collider.transform.tag == "Player" ){
			var hit = collider.gameObject;
			var health = hit.GetComponent<Health>();
			if (health  != null)
			{
				health.TakeDamage(4);
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
