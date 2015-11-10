using UnityEngine;
using System.Collections;

public class MatchOrientationTo : MonoBehaviour {
	public GameObject to;

	// Update is called once per frame
	void LateUpdate () {
		to.transform.rotation = transform.rotation;
	}
}
