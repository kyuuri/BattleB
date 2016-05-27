using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BladeScript : NetworkBehaviour {

	public GameObject hitParticle;
	public float damage;
	private PlayerController firingPlayer;
	[SyncVar]
	public int playerId;
	public GameObject target;



	void Update(){
		if (target) {
			Vector3 destination = new Vector3(target.transform.position.x, 0, target.transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, destination, 200 * Time.deltaTime);
			Vector3 angle = target.transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler(angle.x, angle.y - 90, angle.z);
			//LookAtMouse ();
		}

		if (playerId != 0 && firingPlayer == null) {
			CheckPlayer ();
		}
	}

	void Damage(Collider collider){
		if(collider.transform.tag == "Player" ){
			int id = collider.GetComponent<PlayerController> ().playerId;
			if (id != playerId) {
				var hit = collider.gameObject;
				var health = hit.GetComponent<Health> ();
				if (health != null) {
					health.TakeDamage (damage, playerId);

					var particle = (GameObject)Instantiate (
						              hitParticle,
						              collider.transform.position - collider.transform.up * 0.7f, Quaternion.identity);

					Destroy (particle, 0.2f);
					NetworkServer.Spawn (particle);
				}
			}
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

	void CheckPlayer(){
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < allPlayers.Length; i++) {
			PlayerController tmpPlayer = allPlayers [i].GetComponent<PlayerController> ();

			if (tmpPlayer.playerId == playerId) {
				firingPlayer = tmpPlayer;
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

	void LookAtMouse(){
		// Generate a plane that intersects the transform's position with an upwards normal.
		Plane playerPlane = new Plane(Vector3.up, transform.position);

		// Generate a ray from the cursor position
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		// Determine the point where the cursor ray intersects the plane.
		// This will be the point that the object must look towards to be looking at the mouse.
		// Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
		//   then find the point along that ray that meets that distance.  This will be the point
		//   to look at.
		float hitdist = 0.0f;
		// If the ray is parallel to the plane, Raycast will return false.
		if (playerPlane.Raycast (ray, out hitdist)) 
		{
			// Get the point along the ray that hits the calculated distance.
			Vector3 targetPoint = ray.GetPoint(hitdist);

			// Determine the target rotation.  This is the rotation if the transform looks at the target point.
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			targetRotation = Quaternion.Euler (0, targetRotation.eulerAngles.y, 90);
			//			targetRotation = Quaternion.Euler (0, targetRotation.eulerAngles.y + 90, 0);
			//Debug.Log (targetRotation.eulerAngles);

			// Smoothly rotate towards the target point.
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7 * Time.deltaTime);
		}

	}
}
