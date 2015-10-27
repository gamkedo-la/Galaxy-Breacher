using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HideInstructionsKey : MonoBehaviour {
	private Text instructionsText;
	private string helpMessage; // grabs at scene start
	private string hiddenClue = "Press H to Show Help";
	private bool showingHelp = false;

	// Use this for initialization
	void Start () {
		instructionsText = GetComponent<Text>();
		helpMessage = instructionsText.text;
		instructionsText.text = hiddenClue;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.H)) {
			showingHelp = !showingHelp;
			if(showingHelp) {
				instructionsText.text = helpMessage;
			} else {
				instructionsText.text = hiddenClue;
			} // end of else for showingHelp
		} // end of if H key
	} // end of Update

}
