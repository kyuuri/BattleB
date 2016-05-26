using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float damage = 10;

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.tag == "Player" ){
			var hit = collider.gameObject;
			var health = hit.GetComponent<Health>();
			if (health  != null)
			{
				health.TakeDamage(damage);
			}
			Destroy(gameObject);
		}
	}
}