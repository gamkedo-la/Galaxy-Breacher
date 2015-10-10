using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {
	void Start () {
		Destroy(gameObject, 0.65f);
	}
}
