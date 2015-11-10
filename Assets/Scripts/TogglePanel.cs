using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TogglePanel : MonoBehaviour {
	public GameObject whichPanel;
	public GameObject thingToTurnOpposite;
	public bool disableSelf;

	void Start() {
		if(thingToTurnOpposite) {
			thingToTurnOpposite.SetActive( whichPanel.activeSelf == false );
		}
	}

	public void Toggle() {
		whichPanel.SetActive( !whichPanel.activeSelf );

		bool oppositeState = (whichPanel.activeSelf == false);

		if(disableSelf) {
			Button thisButton = gameObject.GetComponent<Button>();
			thisButton.interactable = oppositeState;
		}
		if(thingToTurnOpposite) {
			thingToTurnOpposite.SetActive( oppositeState );
		}
	}
}
