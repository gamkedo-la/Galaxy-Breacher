using UnityEngine;
using System.Collections;

public class Shootable : MonoBehaviour {
	public int healthLimit = 0; // 0 invulnerable, still case for player while testing
	public bool reportDeath = false;
	int timesHit = 0;

	public void DamageThis() {
		timesHit++;
		healthLimit--;
		Debug.Log (gameObject.name + " HAS BEEN HIT "+ timesHit+" TIMES!");
		if(healthLimit == 0) {
			// Debug.Log ("KA BOOM");
			if(reportDeath) {
				SpawnTicketBooth.instance.reportDeath();
			}
			GameObject.Instantiate(
				PlayerControl.instance.explodePrefabGeneral,
				transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter (Collider other) {
		Destroy(other.gameObject);
		DamageThis();
	}
}
