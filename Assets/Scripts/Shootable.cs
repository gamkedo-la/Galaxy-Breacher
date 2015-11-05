using UnityEngine;
using System.Collections;

public class Shootable : MonoBehaviour {
	public int healthLimit = 0; // 0 invulnerable, still case for player while testing
	public bool reportDeath = false;
	public bool canBeTarget = false;
	int timesHit = 0;
	public bool isMegaShipHardPart = false;
	private bool alreadyKilledInChain = false;

	public Vector3 lockDisplay = Vector3.zero;
	public bool playerLOS = false;

	private bool isPlayer;

	void Start() {
		isPlayer = (gameObject.GetComponent<PlayerControl>() != null);
	}

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
			HardPointCounter hpc = transform.GetComponentInParent<HardPointCounter>();
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

		if(isPlayer) {
			// Application.LoadLevel( Application.loadedLevel );
			if(GameStateStaticProgress.cheatsOn) {
				alreadyKilledInChain = false; // so can later die if cheats toggled
			} else {
				PlayerControl.instance.PlayerDie();
			}
		} else {
			// explosions always at max volume, high priority gameplay event, play on camera
			if(isMegaShipHardPart) {
				SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.megashipDamage, 
					Camera.main.transform.position, Random.Range(0.4f,0.7f),
					Camera.main.transform);
			} else {
				/* SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.explodeGeneric, 
					Camera.main.transform.position, Random.Range(0.25f,0.5f),
					Camera.main.transform); */
			}
			Destroy(gameObject);
		}
	}

	public void DamageThis() {
		timesHit++;
		healthLimit--;
		// Debug.Log (gameObject.name + " HAS BEEN HIT "+ timesHit+" TIMES!");

		if(healthLimit <= 0) {
			ExplodeThis();
		} else if(isPlayer) {
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.playerHurt, transform.position, 1.0f, transform);
		}
	}

	void OnTriggerEnter(Collider other) { // only for how laser bolts hit player
		//if(other.GetComponent<PlayerControl>()) {
			DamageThis();
		//}
	}
}
