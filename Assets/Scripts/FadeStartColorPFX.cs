using UnityEngine;
using System.Collections;

public class FadeStartColorPFX : MonoBehaviour {
	public GameObject logo1st;
	public GameObject logo2nd;

	LogoScale scale1;
	LogoScale scale2;

	ParticleSystem pfx;
	Material fadeMat;
	Color matCol;
	float timeStretch = 9.25f;
	FireFirstTimeOnly septraDustHolder;

	public Material quadMat;

	// Use this for initialization
	void Start () {
		pfx = GetComponent<ParticleSystem>();
		scale1 = logo1st.GetComponent<LogoScale>();
		scale2 = logo2nd.GetComponent<LogoScale>();
		fadeMat = GetComponent<Renderer>().sharedMaterial;
		matCol = Color.white;
		quadMat.color = Color.white;
		septraDustHolder = transform.parent.GetComponent<FireFirstTimeOnly>();
	}

	void preferThisOverThat(GameObject show, GameObject hide) {
		if(show.activeSelf == false) {
			show.SetActive(true);
		}
		if(hide.activeSelf) {
			hide.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time < timeStretch) {
			preferThisOverThat(logo1st, logo2nd);
			if(scale1.enabled == false) {
				scale1.enabled = true;
			}
		} else if(Time.time < timeStretch * 2.0) {
			preferThisOverThat(logo2nd, logo1st);
			if(scale2.enabled == false) {
				scale2.enabled = true;
			}
		} else if(pfx) {
			Destroy(logo1st);
			Destroy(logo2nd);

			septraDustHolder.forceShow = true;
			septraDustHolder.monsterToHide.SetActive(true);
			septraDustHolder.dustToHide.SetActive(true);
			Destroy(pfx);
		}

		matCol.a = (6.0f+4.0f*Mathf.Cos (Time.time * (Mathf.PI * 2.0f)/timeStretch))/10.0f;
		matCol.a *= matCol.a; // square the effect
		if(matCol.a < 0.0f) {
			matCol.a = 0.0f;
		}
		if(Time.time < timeStretch * 2.0f) {
			fadeMat.color = matCol;
		} else if(Time.time < timeStretch * 2.5) {
			quadMat.color = matCol;
		} else {
			Destroy(transform.parent.gameObject);
		}
	}
}
