using UnityEngine;
using System.Collections;

public class ConvoyMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * Time.deltaTime * -80.0f;
	}
}
