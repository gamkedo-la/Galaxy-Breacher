using UnityEngine;
using System.Collections;

public class LittleShipAI : MonoBehaviour {

	Vector3 strafeVector = Vector3.zero;
	float attackDist;
	bool dodgingMegaShip = false;
	Transform runFrom;

	public float ignoreRange = 1500.0f;
	public float escapeMarginTime = 0.0f;

	void Start() {
		StartCoroutine( circleStrafeUpdate() );
		escapeMarginTime = 4.0f;
	}

	IEnumerator circleStrafeUpdate() {
		while(true) {
			attackDist = Random.Range(400.0f, 1200.0f);

			switch( Random.Range(0,4) ) {
			case 0:
				strafeVector = Vector3.right;
				break;
			case 1:
				strafeVector = -Vector3.right;
				break;
			case 2:
				strafeVector = Vector3.up;
				break;
			case 3:
				strafeVector = -Vector3.up;
				break;
			}
			yield return new WaitForSeconds(Random.Range(0.5f,2.5f));
		}
	}

	void OnTriggerEnter(Collider other) {
		runFrom = other.transform;
		if(dodgingMegaShip == false) {
			LaserPulseCannon lpc = GetComponent<LaserPulseCannon>();
			lpc.holdFire = true;
		}
		dodgingMegaShip = true;
	}

	void OnTriggerExit (Collider other) {
		if(dodgingMegaShip) {
			LaserPulseCannon lpc = GetComponent<LaserPulseCannon>();
			lpc.holdFire = false;
			escapeMarginTime = 3.0f;
		}
		dodgingMegaShip = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(dodgingMegaShip || escapeMarginTime > 0.0f) {
			escapeMarginTime -= Time.deltaTime;
			if(runFrom == null) {
				runFrom = RadarManager.megaShipHeart.transform;
			}
			Vector3 awayFromMegaShip = transform.position-runFrom.position;
			transform.rotation = Quaternion.Slerp(transform.rotation, 
			                                      Quaternion.LookRotation(awayFromMegaShip),
			                                      Time.deltaTime);
			transform.position += transform.forward * Time.deltaTime * 120.0f;

			return;
		}

		float distFrom = Vector3.Distance( transform.position,
		                                  PlayerControl.instance.transform.position );
		// Debug.Log(distFrom);
		if(distFrom < ignoreRange) {
			Vector3 towardTarget = PlayerControl.instance.transform.position - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation, 
			                                      Quaternion.LookRotation(towardTarget,
			                        PlayerControl.instance.transform.up),
			                                      Time.deltaTime);
			if(distFrom < attackDist) {
				transform.position -= transform.forward * Time.deltaTime * 20.0f;
			} else {
				transform.position += transform.forward * Time.deltaTime * 40.0f;
			}
		} else {
			transform.position += transform.forward * Time.deltaTime * 30.0f;
		}
	}
}
