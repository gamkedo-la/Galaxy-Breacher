using UnityEngine;
using System.Collections;

public class LaserMove : MonoBehaviour {
	public float laserSpeed = 390.0f;

	// Use this for initialization
	void Start () {
		Destroy(gameObject,Random.Range(4.0f,6.5f));
		/*float distTo = Vector3.Distance( PlayerControl.instance.transform.position, transform.position);
		float distVol = 1.0f - distTo / 1500.0f;
		if( distVol > 0.0f ) {
			distVol *= distVol; // square falloff effect
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.laserFire, transform.position,
				Random.Range(0.05f,0.2f) * distVol, transform);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up * Time.deltaTime * laserSpeed;
	}
}
