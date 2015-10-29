using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlowPulse : MonoBehaviour {
	public float phaseOffset;
	private Text logoText;
	private Color textColor;
	// Use this for initialization

	void Start () {
		logoText = GetComponent<Text>();
		textColor = logoText.color;
	}
	
	// Update is called once per frame
	void Update () {
		textColor.a = 0.7f+0.1f*Mathf.Cos (3.5f*Time.time + phaseOffset*2.0f*Mathf.PI);
		logoText.color = textColor;
		transform.rotation = Quaternion.AngleAxis( Mathf.Cos(Time.time*2.0f) * 1.0f, Vector3.forward );
		transform.localScale = Vector3.one * (2.0f + 0.01f * Mathf.Cos(Time.time*2.17f));
	}
}
