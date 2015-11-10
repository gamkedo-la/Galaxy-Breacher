using UnityEngine;
using System.Collections;

public class EstablishingView : MonoBehaviour {
	Canvas UICanvas;
	GameObject Radar3D;
	GameObject skyCamMatch;
	GameObject lerpAnchor;
	Quaternion wasSkyCamAng;
	public Vector3 slideAmt;
	public Vector3 spinAmt;
	static public EstablishingView instance;
	GameObject snowFall;
	Vector3 snowOffset;

	float totalTime = 9.0f;
	float transition = 2.0f;
	bool forcedDone = false;

	// Use this for initialization
	void Start () {
		snowFall = GameObject.Find ("snowfall");
		if(snowFall) {
			/*snowOffset = snowFall.transform.localPosition;
			snowFall.transform.parent = transform;
			snowFall.transform.position = transform.position+snowOffset;*/
			snowFall.SetActive(false);
		}
		skyCamMatch = GameObject.Find ("Player Main Camera");
		Radar3D = GameObject.Find ("3D UI Radar etc");
		UICanvas = GameObject.Find ("UI Canvas").GetComponent<Canvas>();
		lerpAnchor = new GameObject();
		lerpAnchor.transform.parent = transform.parent;
		UICanvas.enabled = false;
		Radar3D.SetActive(false);
		wasSkyCamAng = skyCamMatch.transform.rotation;
		skyCamMatch.SetActive(false);
		if(instance) {
			DestroyImmediate(instance);
		}
		instance = this;
	}

	void LateUpdate() {
		if(forcedDone == false) {
			if(Input.anyKeyDown || Time.timeSinceLevelLoad > (totalTime-0.7f)) {
				forcedDone = true;
				AutoFade.FadeOverCamera(Color.white);
			}
		}

		if(Time.timeSinceLevelLoad < (totalTime-transition)) {
			transform.position += Time.deltaTime * (
				transform.right * slideAmt.x +
				transform.up * slideAmt.y +
				transform.forward * slideAmt.z);
			transform.Rotate(Time.deltaTime * spinAmt.x,
			                 Time.deltaTime * spinAmt.y,
			                 Time.deltaTime * spinAmt.z);
			lerpAnchor.transform.position = transform.position;
			lerpAnchor.transform.rotation = transform.rotation;
		} else {
			float interpAmt = (Time.timeSinceLevelLoad - (totalTime-transition))/transition;
			transform.position = Vector3.Lerp(lerpAnchor.transform.position,
			                                   skyCamMatch.transform.position,
			                                   interpAmt);
			transform.rotation = Quaternion.Slerp(lerpAnchor.transform.rotation,
			                                   skyCamMatch.transform.parent.rotation,
			                                   interpAmt);
		}
		skyCamMatch.transform.rotation = transform.rotation;
	}

	public void CameraSwap() {
		if(snowFall) {
			/*snowFall.transform.parent = PlayerControl.instance.transform;
			snowFall.transform.position = PlayerControl.instance.transform.position
				+ snowOffset;*/
			snowFall.SetActive(true);
		}
		UICanvas.enabled = true;
		skyCamMatch.SetActive(true);
		Radar3D.SetActive(true);
		Destroy(gameObject);
		skyCamMatch.transform.rotation = wasSkyCamAng;
	}
}
