using UnityEngine;
using System.Collections;

public class SerpentMoveForward : MonoBehaviour {
	void Start() {
		if(PlayerControl.instance == null) { // don't do movement when on orbit screen
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update () {
		transform.position += -transform.forward * 380.0f * Time.deltaTime;
		// transform.Rotate(0.0f,0.0f, 150.0f*Time.deltaTime);
	}
}
