using UnityEngine;
using System.Collections;

public class RocketMotion : MonoBehaviour {
	private float rocketSpeed = 34.0f;
	private float accelRate = 85.0f;
	private float maxSpeed = 490.0f;
	private float crazyOffset1,crazyOffset2;

	public Transform target = null;
	Coroutine targetLooker;

	// Use this for initialization
	void Start () {
		crazyOffset1 = Random.Range(0.0f,Mathf.PI*2.0f);
		crazyOffset2 = Random.Range(0.0f,Mathf.PI*2.0f);
		transform.Rotate(Random.Range(-2.0f,2.0f), Random.Range(0.0f,360.0f),
		                 Random.Range(-2.0f,2.0f), Space.Self);

		targetLooker = StartCoroutine(targetScan());

		/*SoundCenter.instance.PlayClipOn(
			SoundCenter.instance.rocketLaunch, transform.position,
			Random.Range(0.25f,0.5f));*/
	}

	public void inheritSpeedBoost(float adjustedThrottle) {
		rocketSpeed += adjustedThrottle;
	}
	
	IEnumerator targetScan() {
		while(true) {
			yield return new WaitForSeconds( Random.Range(0.1f,0.5f) );
			if(target==null) {
				Collider [] possibleTargets = Physics.OverlapSphere(transform.position + transform.forward * 1500.0f, 1495.0f);
				foreach(Collider consider in possibleTargets) {
					Shootable shootMeScript = consider.GetComponent<Shootable>();
					if(shootMeScript && shootMeScript.healthLimit > 0 && shootMeScript.canBeTarget) {
						RaycastHit rhInfo;
						Vector3 toward = consider.transform.position-transform.position;
						int layerMask = ~(1<<12);
						if(Physics.Raycast( transform.position + toward.normalized * 5.0f,
						                   toward,
						                   out rhInfo,19999.0f,layerMask) &&
						   rhInfo.transform.gameObject == consider.gameObject) {
							target = consider.transform;
						}
					}
				}
			}
			if(target) {
				StopCoroutine(targetLooker);
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		rocketSpeed += Time.deltaTime * accelRate;
		if(rocketSpeed > maxSpeed) {
			rocketSpeed = maxSpeed;
		}
		transform.Rotate( // oscillate swivel, good effect for laser attack
		                 Mathf.Cos(crazyOffset1+Time.time*7.673f) * 25.0f * Time.deltaTime,
		                 0.0f, 
		                 Mathf.Sin(crazyOffset2-Time.time*3.1f) * 23.3f * Time.deltaTime, Space.Self);

		if(target) {
			Vector3 towardTarget = target.position - transform.position;
			transform.rotation = Quaternion.LookRotation(towardTarget);
			transform.Rotate(90.0f,0.0f,0.0f);

		}
		transform.position += transform.up * Time.deltaTime * rocketSpeed;
	}
}
