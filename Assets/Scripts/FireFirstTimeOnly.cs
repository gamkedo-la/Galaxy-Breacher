using UnityEngine;
using System.Collections;

public class FireFirstTimeOnly : MonoBehaviour {
	static bool hasRun = false;
	public GameObject monsterToHide;
	public GameObject dustToHide;
	public bool forceShow = false;

	// Use this for initialization
	void Start () {
		if(hasRun) {
			CloseOpening();
		}
		hasRun = true;
	}

	void Update() {
		if(forceShow == false) {
			if(monsterToHide.activeSelf) {
				monsterToHide.SetActive(false);
			}
			if(dustToHide.activeSelf) {
				dustToHide.SetActive(false);
			}
		}
	}

	void CloseOpening() {
		Destroy(gameObject);
		monsterToHide.SetActive(true);
		dustToHide.SetActive(true);
	}
}
