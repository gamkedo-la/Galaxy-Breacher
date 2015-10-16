using UnityEngine;
using System.Collections;

public class LaserPulseCannon : MonoBehaviour {
	public GameObject laserPrefab;
	public Transform fireFromPos;
	public float inaccuracyArcMax = 2.0f;
	public float ROF = 0.2f;
	public static Transform laserKeeper;

	public bool holdFire = false;
	public bool needsLOS = true;
	public bool spawnLimited = false;

	void Start() {
		StartCoroutine(FireLaser());
		if(laserKeeper == null) {
			laserKeeper = GameObject.Find("LaserKeeper").transform;
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

			if(holdFire) {
				continue;
			}

			if(needsLOS) {

				if(Physics.Raycast( transform.position,
				                   PlayerControl.instance.transform.position-transform.position,
				                   out rhInfo,
				                   5000.0f,
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
			Quaternion missQuat = Quaternion.AngleAxis(Random.Range(-missBy,missBy),Camera.main.transform.up);
			missQuat = missQuat * Quaternion.AngleAxis(Random.Range(-missBy,missBy),Camera.main.transform.right);
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
				newGO.transform.parent = laserKeeper;
				if(spawnLimited) {
					Shootable reportWhenDead = newGO.GetComponent<Shootable>();
					reportWhenDead.reportDeath = true;
				}
			}
		}
	}
}
