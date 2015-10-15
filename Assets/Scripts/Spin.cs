using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	public float rate;

	// Update is called once per frame
	void Update () {
		transform.Rotate(0.0f, 0.0f, rate * Time.deltaTime);	
	}
}
