using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateStaticProgress : MonoBehaviour {
	static public bool cheatsOn = false; // default for testing
	static public bool uprightDodge = false;
	public GameObject clearDialogConfirmPanel;
	public GameObject hideDragon;

	void Start() {
		Toggle updateToggle = GetComponent<Toggle>();
		if(updateToggle) {
			switch(updateToggle.name) {
			case "cheatsOn":
				updateToggle.isOn = cheatsOn;
				break;
			case "uprightDodge":
				updateToggle.isOn = !uprightDodge;
				break;
			}
		}
	}

	public void FairStart() {
		cheatsOn = false;
		AutoFade.LoadLevel("Level Select", Color.white);
	}

	public void CheatStart() {
		cheatsOn = true;
		AutoFade.LoadLevel("Level Select", Color.white);
	}

	public void WipeProgress() {
		Debug.Log("Progress wiped");
		PlayerPrefs.DeleteAll();
		hideDragon.SetActive(false);
		if(clearDialogConfirmPanel) {
			clearDialogConfirmPanel.SetActive(true);
		}
	}

	public void BackToTitle() {
		AutoFade.LoadLevel("TitleLogoSplashIntro", Color.white);
	}

	public void WipeProgressAndReturnToTitle() {
		PlayerPrefs.DeleteAll();
		AutoFade.LoadLevel("TitleLogoSplashIntro", Color.white);
	}

	public void CheatToggle(bool newValue) {
		cheatsOn = newValue;
	}

	public void UprightToggle(bool newValue) {
		uprightDodge = !newValue;
	}
}
