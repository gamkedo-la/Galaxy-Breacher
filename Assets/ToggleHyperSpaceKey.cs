using UnityEngine;
using System.Collections;

public class ToggleHyperSpaceKey : MonoBehaviour {
	void Start() {
		if(GameStateStaticProgress.cheatsOn) {
			gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown( KeyCode.Alpha0 )) {
			gameObject.SetActive(false);
		}
	}
}
