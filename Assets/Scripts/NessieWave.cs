using UnityEngine;
using System.Collections;

public class NessieWave : MonoBehaviour {
	public Transform head;
	public GameObject[] nessieBones;
	public float[] relPos;

	// Use this for initialization
	void Start () {
		relPos = new float[ nessieBones.Length ];
		relPos[0] = head.transform.position.z - nessieBones[0].transform.position.z;
		float accZ = relPos[0];
		for(int i = 1; i<nessieBones.Length; i++) {
			relPos[i] = nessieBones[i].transform.position.z - nessieBones[i-1].transform.position.z;
			FlapWings fw = nessieBones[i].GetComponent<FlapWings>();
			if(fw) {
				fw.offset = accZ;
			}
			accZ += relPos[i];
		}
		Update ();
	}
	
	// Update is called once per frame
	void Update () {
		float accZ = 0.0f;
		for(int i = 0; i<nessieBones.Length; i++) {
			accZ += relPos[i];
			nessieBones[i].transform.position =
				head.position +
					Vector3.forward * accZ +
					Vector3.up * Mathf.Cos (-Time.time * 2.0f + accZ / 180.0f) * 40.0f;
		}
		for(int i = 2; i<nessieBones.Length; i++) {
			nessieBones[i-1].transform.LookAt(
				nessieBones[i].transform.position );
		}
		nessieBones[0].transform.rotation =
			nessieBones[1].transform.rotation;
		nessieBones[nessieBones.Length-1].transform.rotation =
			nessieBones[nessieBones.Length-2].transform.rotation;
		nessieBones[nessieBones.Length-1].transform.Rotate(0.0f,
		                                                   Mathf.Cos(Time.time * 5.0f) * 7.0f,
		                                                   0.0f);
	}
}
