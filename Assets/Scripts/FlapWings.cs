using UnityEngine;
using System.Collections;

public class FlapWings : MonoBehaviour {
	public Transform wing1;
	public Transform wing2;
	public float offset = 0.0f;
	// Use this for initialization
	void Start () {
		Update();	
	}
	
	// Update is called once per frame
	void Update () {
		if(wing1) {
			wing1.transform.rotation = 
				Quaternion.AngleAxis( 90.0f, Vector3.up) *
				Quaternion.AngleAxis( Mathf.Cos( offset + Time.time * 5.0f ) * 15.0f, Vector3.right) *
				Quaternion.AngleAxis( Mathf.Cos( offset +Time.time * 5.0f ) * -40.0f, Vector3.forward);
		}
		if(wing2) {
			wing2.transform.rotation = 
				Quaternion.AngleAxis( -90.0f, Vector3.up) *
				Quaternion.AngleAxis( Mathf.Cos( offset +Time.time * 5.0f ) * 15.0f, Vector3.right) *
				Quaternion.AngleAxis( Mathf.Cos( offset +Time.time * 5.0f ) * -40.0f, -Vector3.forward);
		}
	}
}
