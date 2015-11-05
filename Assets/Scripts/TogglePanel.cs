using UnityEngine;
using System.Collections;

public class TogglePanel : MonoBehaviour {
	public GameObject whichPanel;
	public GameObject thingToTurnOpposite;

	void Start() {
		if(thingToTurnOpposite) {
			thingToTurnOpposite.SetActive( whichPanel.activeSelf == false );
		}
	}

	public void Toggle() {
		whichPanel.SetActive( !whichPanel.activeSelf );
		if(thingToTurnOpposite) {
			thingToTurnOpposite.SetActive( whichPanel.activeSelf == false );
		}
	}
}
