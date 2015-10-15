using UnityEngine;
using System.Collections;

public class RadarManager : MonoBehaviour {
	public GameObject nearestTargetPuppet;
	public GameObject radarSphere;

	public GameObject megaShipHeart;
	public GameObject megaShipPuppet;

	public Transform mirrorZ;
	private Vector3 mirrorZVect;

	public float scanRange = 3000.0f;

	void Start() {
		mirrorZVect = Vector3.one;
		mirrorZVect.z = -1.0f;
	}

	// Update is called once per frame
	void Update () {
		mirrorZ.localScale = Vector3.one;

		Collider [] nearby = Physics.OverlapSphere(transform.position, scanRange);
		float nearestDist = scanRange+1.0f;
		GameObject nearestEnemy = null;
		for(int i=0;i<nearby.Length;i++) {
			if(nearby[i].GetComponentInChildren<LittleShipAI>()) {
				float distTo = Vector3.Distance(transform.position,
				                                nearby[i].transform.position);
				if(distTo < nearestDist) {
					nearestEnemy = nearby[i].gameObject;
					nearestDist = distTo;
				}
			}
		}
		if(nearestEnemy != null) {
			if(nearestTargetPuppet.activeSelf == false) {
				nearestTargetPuppet.SetActive(true);
			}
			nearestTargetPuppet.transform.rotation = nearestEnemy.transform.rotation;
			nearestTargetPuppet.transform.position = radarSphere.transform.position
				+ (nearestEnemy.transform.position - transform.position).normalized *
					radarSphere.transform.localScale.x * 0.35f;
		} else if(nearestTargetPuppet.activeSelf) {
			nearestTargetPuppet.SetActive(false);
		}

		megaShipPuppet.transform.Rotate(Time.deltaTime * 80.0f,Time.deltaTime * 30.0f, 0.0f);
		megaShipPuppet.transform.position = radarSphere.transform.position + (megaShipHeart.transform.position - transform.position).normalized *
			radarSphere.transform.localScale.x * 0.35f;

		mirrorZ.localScale = mirrorZVect;
	}
}
