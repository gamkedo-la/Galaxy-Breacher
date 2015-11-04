using UnityEngine;
using System.Collections;

public class TogglePanel : MonoBehaviour {
	public GameObject whichPanel;
	public void Toggle() {
		whichPanel.SetActive( !whichPanel.activeSelf );
	}
}
