using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateStaticProgress : MonoBehaviour {
	static public bool cheatsOn = true; // default for testing

	void Start() {
		Toggle updateToggle = GetComponent<Toggle>();
		if(updateToggle) {
			updateToggle.isOn = cheatsOn;
		}
	}

	public void CheatToggle(bool newValue) {
		cheatsOn = newValue;
	}
}
