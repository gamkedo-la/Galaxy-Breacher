using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeTextIfCheating : MonoBehaviour {
	private Text instructions;

	// Use this for initialization
	void Start () {
		instructions = GetComponent<Text>();
		if(GameStateStaticProgress.cheatsOn) {
			instructions.text = "Pick Any Stage to Practice";
		} else {
			instructions.text = "Scan for Your Next Battle";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
