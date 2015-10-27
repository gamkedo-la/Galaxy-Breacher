using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateStaticProgress : MonoBehaviour {
	static public bool cheatsOn = true; // default for testing
	static public bool uprightDodge = false;

	void Start() {
		Toggle updateToggle = GetComponent<Toggle>();
		if(updateToggle) {
			switch(updateToggle.name) {
			case "cheatsOn":
				updateToggle.isOn = cheatsOn;
				break;
			case "uprightDodge":
				updateToggle.isOn = uprightDodge;
				break;
			}
		}
	}

	public void CheatToggle(bool newValue) {
		cheatsOn = newValue;
	}

	public void UprightToggle(bool newValue) {
		uprightDodge = newValue;
	}
}
