using UnityEngine;
using System.Collections;

public class LaserMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject,Random.Range(4.0f,6.5f));	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up * Time.deltaTime * 390.0f;
	}
}
