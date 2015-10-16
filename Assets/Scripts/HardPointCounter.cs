using UnityEngine;
using System.Collections;

public class HardPointCounter : MonoBehaviour {
	public int hardpointCount = 0;
	public GameObject megashipParent;
	public GameObject megashipExplosionCenter;
	public GameObject megashipDeathExplosionFire;

	// Use this for initialization
	void Start () {
		foreach(Transform child in transform){
			if(child.gameObject.tag == "Hardpoint"){
				hardpointCount++;
			}
		}
	}
	
	public void RemoveHardpoint() {
		hardpointCount--;
		Debug.Log("Megaship hardpoints remaining: " + hardpointCount);
		if(hardpointCount<=0) {
			GameObject.Instantiate(megashipDeathExplosionFire,
			                       megashipExplosionCenter.transform.position,
			                       megashipExplosionCenter.transform.rotation);
			Destroy( megashipParent, 0.3f );
		}
	}
}
