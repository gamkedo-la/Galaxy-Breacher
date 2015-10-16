using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {
	private float timeAlive = 10.0f;
	private float initialRange;

	public float lightFadeTime = 0.5f;
	private float timeLeft = 0.0f;
	private Light fadeLight;

	void Start () {
		Destroy(gameObject, 10.0f);
		fadeLight = GetComponent<Light>();
		if(fadeLight) {
			initialRange = fadeLight.range;
			timeLeft = lightFadeTime;
		}
	}

	void Update() {
		if(fadeLight && timeLeft > 0.0f) {
			timeLeft-= Time.deltaTime;
			fadeLight.range = initialRange*(timeLeft/lightFadeTime);
			if(timeLeft <= 0.0f) {
				fadeLight.enabled = false;
			}
		}
	}
}
