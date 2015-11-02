using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Speed : MonoBehaviour {

	public Sprite[] SpeedLevels;
	public int currentSpeed = 0;
	public int last = 0;

	public Sprite[] throttleLevels;
	public int counter = 0;
	public int lastCounter = 0;




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (currentSpeed != last) {
			last = currentSpeed; 
			gameObject.GetComponent<Image>().sprite = SpeedLevels[currentSpeed];
		}

		if (counter != lastCounter) {
			lastCounter = counter; 
			gameObject.GetComponent<Image>().sprite = throttleLevels[counter];
		}
	}
}
