using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickToHide : MonoBehaviour {
	public GameObject dragonToWake = null;
	public Button buttonToEnable = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0) && gameObject.activeSelf) {
			TogglePanel tpScript = GetComponentInChildren<TogglePanel>();

			if(tpScript) {
				tpScript.Toggle();
			} else {
				gameObject.SetActive(false);
			}
			if(dragonToWake) {
				dragonToWake.SetActive( true );
			}
			if(buttonToEnable) {
				buttonToEnable.interactable = true;
			}
		}
	}
}
