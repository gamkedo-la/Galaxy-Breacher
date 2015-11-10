using UnityEngine;
using System.Collections;

public class LogoScale : MonoBehaviour {
	Vector3 startScale;
	float startTime;

	// Use this for initialization
	void Start () {
		startScale = transform.localScale;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = startScale * ( (Time.time-startTime)*0.04f+1.0f);
	}
}
