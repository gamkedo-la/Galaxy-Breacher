using UnityEngine;
using System.Collections;

public class SwingTowardPlayer : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(PlayerControl.instance.transform.position); // snap tracking
		transform.Rotate( // oscillate swivel, good effect for laser attack
			Mathf.Cos(Time.time*7.673f) * 3.0f,
			Mathf.Cos(Time.time*3.1f) * 5.0f, 0.0f, Space.Self);
	}
}
