using UnityEngine;
using System.Collections;

public class Shootable : MonoBehaviour {
	public int healthLimit = 0; // 0 invulnerable, still case for player while testing
	public bool reportDeath = false;
	public bool canBeTarget = false;
	int timesHit = 0;
	private bool alreadyKilledInChain = false;

	public void ExplodeThis() {
		if(alreadyKilledInChain) {
			return;
		}
		alreadyKilledInChain = true;

		// Debug.Log ("KA BOOM");
		if(reportDeath) {
			SpawnTicketBooth.instance.reportDeath();
		}

		if(tag == "Hardpoint") {
			HardPointCounter hpc = transform.parent.GetComponent<HardPointCounter>();
			hpc.RemoveHardpoint();
		}

		foreach(Transform child in transform){
			if(child.gameObject.tag == "SurviveAfterParentRemoved"){
				child.parent = LaserPulseCannon.laserKeeper;
				child.GetComponent<SelfDestruct>().enabled = true;
				ParticleSystem psScript = child.GetComponent<ParticleSystem>();
				if(psScript) {
					psScript.enableEmission = false;
				}
			}
		}

		Collider [] possibleTargets = Physics.OverlapSphere(transform.position, 30.0f);
		foreach(Collider consider in possibleTargets) {
			Shootable shootableScript = consider.GetComponent<Shootable>();
			if(shootableScript && shootableScript.healthLimit > 0) {
				shootableScript.ExplodeThis();
			}
		}

		GameObject.Instantiate(
			PlayerControl.instance.explodePrefabGeneral,
			transform.position, Quaternion.identity);

		if(gameObject.GetComponent<PlayerControl>()) {
			// Application.LoadLevel( Application.loadedLevel );
			Debug.Log ("GAME OVER!!");
		} else {
			Destroy(gameObject);
		}
	}

	public void DamageThis() {
		timesHit++;
		healthLimit--;
		Debug.Log (gameObject.name + " HAS BEEN HIT "+ timesHit+" TIMES!");
		if(healthLimit == 0) {
			ExplodeThis();
		}
	}

	void OnTriggerEnter(Collider other) { // only for how laser bolts hit player
		//if(other.GetComponent<PlayerControl>()) {
			DamageThis();
		//}
	}
}
