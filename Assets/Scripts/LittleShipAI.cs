using UnityEngine;
using System.Collections;

public class LittleShipAI : MonoBehaviour {

	Vector3 strafeVector = Vector3.zero;
	float attackDist;

	public float ignoreRange = 1500.0f;

	void Start() {
		StartCoroutine( circleStrafeUpdate() );
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
	
	// Update is called once per frame
	void Update () {
		transform.position += strafeVector * Time.deltaTime * 30.0f;

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
		}
	}
}
