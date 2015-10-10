using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {
	public GameObject nearLayer;
	public GameObject spinLayer;
	public ParticleSystem shellEject;
	private float showTime = 0.0f;
	private float scaleBasis;

	// Use this for initialization
	void Start () {
		scaleBasis = nearLayer.transform.localScale.x;
	}

	public void Reset () {
		showTime = -0.1f;
		nearLayer.active = spinLayer.active = false;
	}
	
	public void Strobe () {
		showTime = 0.05f;
		if(shellEject) {
			shellEject.Emit(1);
		}
	}

	// Update is called once per frame
	void Update () {
		bool showNow = (showTime > 0.0);
		
		if(showTime > 0.0f) {
			showTime -= Time.deltaTime;
		}
		
		if(nearLayer.active != showNow) {
			nearLayer.active = showNow;
			spinLayer.active = showNow;
		}
		if(showNow) {
			nearLayer.transform.localScale = Vector3.one * Random.Range(0.7f,1.1f) * scaleBasis;
			spinLayer.transform.localScale = Vector3.one * Random.Range(0.7f,1.1f) * scaleBasis;
			spinLayer.transform.Rotate(0.0f,0.0f,Random.Range(-360.0f,360.0f));
		}
	}
}
