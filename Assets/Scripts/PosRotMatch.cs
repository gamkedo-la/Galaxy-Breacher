using UnityEngine;
using System.Collections;

public class PosRotMatch : MonoBehaviour {
	public Transform alignMatch;
	public bool onlyPos = false;
	
	// Update is called once per frame
	void LateUpdate () {
		alignMatch.position = transform.position;
		if(onlyPos == false) {
			alignMatch.rotation = transform.rotation;
		}
	}
}
