using UnityEngine;
using System.Collections;

public class PosRotMatch : MonoBehaviour {
	public Transform alignMatch;
	
	// Update is called once per frame
	void LateUpdate () {
		alignMatch.position = transform.position;
		alignMatch.rotation = transform.rotation;
	}
}
