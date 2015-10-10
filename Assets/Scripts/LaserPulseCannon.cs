using UnityEngine;
using System.Collections;

public class LaserPulseCannon : MonoBehaviour {
	public GameObject laserPrefab;
	public Transform fireFromPos;
	public float inaccuracyArcMax = 2.0f;
	public float ROF = 0.2f;
	private Transform laserKeeper;

	void Start() {
		StartCoroutine(FireLaser());
		laserKeeper = GameObject.Find("LaserKeeper").transform;
	}

	IEnumerator FireLaser() {
		while(true) {
			yield return new WaitForSeconds(ROF);

			int layerMask = ~(1<<9);
			RaycastHit rhInfo;

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

			float missBy = inaccuracyArcMax;
			Quaternion missQuat = Quaternion.AngleAxis(Random.Range(-missBy,missBy),Camera.main.transform.up);
			missQuat = missQuat * Quaternion.AngleAxis(Random.Range(-missBy,missBy),Camera.main.transform.right);
			Quaternion aimToFire = fireFromPos.rotation;
			aimToFire = missQuat * aimToFire;
			Vector3 skipBarrel = transform.up;
			
			GameObject newGO = (GameObject)GameObject.Instantiate( laserPrefab, fireFromPos.position
			                       - missQuat * skipBarrel * 30.0f, aimToFire);
			newGO.transform.parent = laserKeeper;
		}
	}
}
