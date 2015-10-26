using UnityEngine;
using System.Collections;

public class HardPointCounter : MonoBehaviour {
	public int hardpointCount = 0;
	public GameObject megashipParent;
	public GameObject megashipExplosionCenter;
	public GameObject megashipDeathExplosionFire;

	void Awake () { // Awake since needs to happen before player Start
		foreach(Transform child in transform){
			if(child.gameObject.tag == "Hardpoint"){
				hardpointCount++;
			}
		}
	}
	
	public void RemoveHardpoint() {
		hardpointCount--;
		// Debug.Log("Megaship hardpoints remaining: " + hardpointCount); // now on HUD!
		if(hardpointCount<=0) {
			GameObject.Instantiate(megashipDeathExplosionFire,
			                       megashipExplosionCenter.transform.position,
			                       megashipExplosionCenter.transform.rotation);
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.megashipBoom, 
				Camera.main.transform.position, 1.0f,
				Camera.main.transform);
			Destroy( megashipParent, 0.45f );
		}
	}
}
