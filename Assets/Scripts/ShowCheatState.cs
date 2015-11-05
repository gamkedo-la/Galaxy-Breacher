using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowCheatState : MonoBehaviour {
	private Text cheatText;
	private string baseMessage; // grabs at scene start

	// Use this for initialization
	void Start () {
		cheatText = GetComponent<Text>();
		baseMessage = "";
		fixCheatUIPhrase();
	}

	public void fixCheatUIPhrase() {
		cheatText.text = baseMessage;
		if(GameStateStaticProgress.cheatsOn) {
			cheatText.text += "DEGAMIFIED / CHEATS";
		} else {
			cheatText.text += "";
		} // end of else for showingHelp
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.J)) {
			GameStateStaticProgress.cheatsOn =
				!GameStateStaticProgress.cheatsOn;
			fixCheatUIPhrase();
		} // end of if H key
	} // end of Update
	
}
