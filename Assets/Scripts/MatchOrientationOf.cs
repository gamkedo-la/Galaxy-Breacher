using UnityEngine;
using System.Collections;

public class MatchOrientationOf : MonoBehaviour {
	public GameObject of;

	// Update is called once per frame
	void LateUpdate () {
		transform.rotation = of.transform.rotation;
	}
}
