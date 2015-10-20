using UnityEngine;
using System.Collections;

public class RadarManager : MonoBehaviour {
	public GameObject nearestTargetPuppet;
	public GameObject radarSphere;

	public static GameObject megaShipGO;
	public static GameObject megaShipHeart;
	public GameObject megaShipPuppet;

	public Transform mirrorZ;
	private Vector3 mirrorZVect;

	public float scanRange = 3000.0f;
	private float megaShipHeartSizeDefault;

	public Texture aimTexture;
	public Texture aimTextureNearest;

	IEnumerator FindNearestHardpoint() {
		megaShipGO = GameObject.Find("TargetHardpoints");

		float closestMatch;
		while(megaShipGO != null) {
			if(megaShipGO == null) {
				Debug.Log("megaShipGO named TargetHardpoints is null, bailing on hardpoint search");
				break;
			}
			megaShipHeart = null;
			closestMatch = 9999999.0f;
			foreach (Transform child in megaShipGO.transform){
				if(child.gameObject.tag == "Hardpoint"){
					float distTo = Vector3.Distance( transform.position,
					                                child.transform.position);
					if(distTo < closestMatch) {
						megaShipHeart = child.gameObject;
						closestMatch = distTo;
					}
				}
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	void Start() {
		megaShipHeartSizeDefault = megaShipPuppet.transform.localScale.x;
		mirrorZVect = Vector3.one;
		mirrorZVect.z = -1.0f;
		StartCoroutine( FindNearestHardpoint() );
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

		if(megaShipHeart) {
			if(megaShipPuppet.activeSelf == false) {
				megaShipPuppet.SetActive(true);
			}

			float crashDanger = 1.0f;
			float distToMega = Vector3.Distance( transform.position, megaShipHeart.transform.position );
			crashDanger *= 500.0f / distToMega;
			crashDanger *= crashDanger; // square the effect
			if(crashDanger > 3.5f) {
				crashDanger = 3.5f;
			}
			if(crashDanger < 0.5f) {
				crashDanger = 0.5f;
			}
			megaShipPuppet.transform.localScale = Vector3.one * crashDanger * megaShipHeartSizeDefault;
			megaShipPuppet.transform.Rotate(Time.deltaTime * 80.0f,Time.deltaTime * 30.0f, 0.0f);
			megaShipPuppet.transform.position = radarSphere.transform.position + (megaShipHeart.transform.position - transform.position).normalized *
				radarSphere.transform.localScale.x * 0.35f;
		} else if(megaShipPuppet.activeSelf) {
			megaShipPuppet.SetActive(false);
		}

		mirrorZ.localScale = mirrorZVect;
	}

	void OnGUI() {
		if(megaShipGO) {
			foreach (Transform child in megaShipGO.transform){
				if(child.gameObject.tag == "Hardpoint"){
					Vector3 position = Camera.main.WorldToScreenPoint(child.transform.position);
					if(position.z > 0.0f) { // ignore if behind us
						position.y = Screen.height - position.y;
						float size = 40.0f;
						GUI.DrawTexture(new Rect((position.x - (size/2)), (position.y - (size/2)),
						                         size, size), 
						                (child.gameObject == megaShipHeart ? aimTextureNearest : aimTexture));
					}
				}
			}
		}
	}
}
