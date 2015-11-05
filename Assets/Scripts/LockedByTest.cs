using UnityEngine;
using System.Collections;

public class LockedByTest : MonoBehaviour {
	public GameObject mustBeatThatToPlayThisOne;

	// Use this for initialization
	void Start () {
		if( PlayerPrefs.GetInt(mustBeatThatToPlayThisOne.name,0) == 0 &&
		    GameStateStaticProgress.cheatsOn == false) {
			transform.FindChild("MegaShipCenter").name = "IgnoreThis";
		}
	}
}
