using UnityEngine;
using System.Collections;

public class HardPointCounter : MonoBehaviour {
	public int hardpointCount = 0;
	public GameObject megashipParent;
	public GameObject megashipExplosionCenter;
	public GameObject megashipDeathExplosionFire;

	public GameObject finalLevelMessage;
	public GameObject levelTextToHide;

	void Awake () { // Awake since needs to happen before player Start
		Shootable[] allShoot = GetComponentsInChildren<Shootable>();
		foreach(Shootable child in allShoot){
			if(child.gameObject.tag == "Hardpoint"){
				hardpointCount++;
			}
		}
	}

	void Start() {
		if(PlayerControl.instance == null) {
			if( GameStateStaticProgress.cheatsOn == false &&
			    PlayerPrefs.GetInt(transform.parent.name,0) == 1) {

				if(finalLevelMessage != null) {
					finalLevelMessage.SetActive(true);
				}
				if(levelTextToHide != null) {
					levelTextToHide.SetActive(false);
				}

				GameObject wreckage = (GameObject)GameObject.Instantiate(
					Resources.Load("SmashedShip"),
					transform.position,transform.rotation);
				wreckage.transform.parent = transform.parent.parent;
				Destroy(transform.parent.gameObject);
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
			if(GameStateStaticProgress.cheatsOn == false) {
				PlayerPrefs.SetInt(transform.parent.name,1);
			}
			if(PlayerControl.instance) {
				PlayerControl.instance.FinishedLevel();
			} else {
				Application.LoadLevel("Level Select");
			}
		}
	}
}
