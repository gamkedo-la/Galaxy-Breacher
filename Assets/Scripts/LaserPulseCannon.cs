using UnityEngine;
using System.Collections;

public class LaserPulseCannon : MonoBehaviour {
	public GameObject laserPrefab;
	public Transform fireFromPos;
	public float inaccuracyArcMax = 2.0f;
	public float ROF = 0.2f;
	public float maxRange = 5000.0f;
	public static Transform laserKeeper;
	public static Transform shipKeeper;

	public bool holdFire = false;
	public bool needsLOS = true;
	public bool spawnLimited = false;

	void Start() {
		if(PlayerControl.instance == null) {
			Destroy(this); // remove component, no player in scene, must be level menu
		} else {
			StartCoroutine(FireLaser());
			if(laserKeeper == null) {
				laserKeeper = GameObject.Find("LaserKeeper").transform;
			}
			if(shipKeeper == null) {
				shipKeeper = GameObject.Find("MegaShipCenter").transform;
			}
		}
	}

	IEnumerator FireLaser() {
		while(true) {
			RaycastHit rhInfo;
			int layerMask = ~(1<<9);

			if(Random.Range(0.0f,1.0f) < 0.05f) {
				yield return new WaitForSeconds(4.0f); // randomly hold to reload or whatever
			} else {
				yield return new WaitForSeconds(ROF);
			}

			if(PlayerControl.instance.isDead) {
				break;
			}

			if(holdFire) {
				continue;
			}

			if(needsLOS) {

				if(Physics.Raycast( transform.position,
				                   PlayerControl.instance.transform.position-transform.position,
				                   out rhInfo,
				                   maxRange,
				                   layerMask)) {
					if(rhInfo.collider.tag != "Player") {
						//Debug.Log ("LOOKING AT SOMETHING ELSE");
						continue; // view is blocked
					} /*else {
						Debug.Log ("CAN SEE FIRE AT WILL");
					}*/
				} else { // player was out of range or otherwise nothing was found
					//Debug.Log ("DID NOT FIND PLAYER");
					continue;
				}
			}

			float missBy = inaccuracyArcMax;

			if(Camera.main == null) {
				missBy = 4.0f;
			}

			Transform aimRel = (Camera.main ? Camera.main.transform : PlayerControl.instance.transform);
			Quaternion missQuat = Quaternion.AngleAxis(Random.Range(-missBy,missBy),aimRel.up);
			missQuat = missQuat * Quaternion.AngleAxis(Random.Range(-missBy,missBy),aimRel.right);
			Quaternion aimToFire = fireFromPos.rotation;
			aimToFire = missQuat * aimToFire;
			Vector3 skipBarrel = transform.forward;

			if(needsLOS && Physics.Raycast( fireFromPos.position,
			                   Vector3.zero + missQuat * skipBarrel * 200.0f,
			                   out rhInfo,
			                   5000.0f,
			                   layerMask) && rhInfo.collider.tag != "Player") {
				// Debug.Log ("CANNON MUZZLE BLOCKED BY " + rhInfo.collider.name);
				continue;
			}

			if(spawnLimited==false || SpawnTicketBooth.instance.requestSpawnTicket()) {		
				GameObject newGO = (GameObject)GameObject.Instantiate( laserPrefab,
				                      fireFromPos.position + missQuat * skipBarrel * 30.0f,
				                              aimToFire  * Quaternion.AngleAxis(90.0f,Vector3.right));
				if(newGO.GetComponent<LittleShipAI>() != null) {
					newGO.transform.parent = shipKeeper;
				} else {
					newGO.transform.parent = laserKeeper;
				}
				if(spawnLimited) {
					Shootable reportWhenDead = newGO.GetComponent<Shootable>();
					reportWhenDead.reportDeath = true;
				}
			}
		}
	}
}
