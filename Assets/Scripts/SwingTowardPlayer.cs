using UnityEngine;
using System.Collections;

public class SwingTowardPlayer : MonoBehaviour {
	public bool noMissing = false;
	public float ampEffect = 1.0f;
	public float timeStretch = 1.0f;
	public bool skipPlayerTest = false;

	// Use this for initialization
	void Start () {
		if(skipPlayerTest == false && PlayerControl.instance == null) {
			Destroy(this); // remove component, no player in scene, must be level menu
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerControl.instance) {
			transform.LookAt(PlayerControl.instance.transform.position); // snap tracking
		}
		if(noMissing == false) {
			transform.Rotate( // oscillate swivel, good effect for laser attack
			                 Mathf.Cos(Time.time*7.673f * timeStretch) * 2.0f* ampEffect,
			                 Mathf.Cos(Time.time*3.1f * timeStretch) * 3.0f* ampEffect, 0.0f, Space.Self);
		}
	}
}
