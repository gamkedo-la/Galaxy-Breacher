using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateStaticProgress : MonoBehaviour {
	static public bool cheatsOn = false;

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
