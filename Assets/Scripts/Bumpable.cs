using UnityEngine;
using System.Collections;

public class Bumpable : MonoBehaviour {
	private FMOD_StudioEventEmitter fmodSource;

	void Start() {
		fmodSource = GetComponent<FMOD_StudioEventEmitter>();
	}

	void OnCollisionEnter (Collision coll) {
		/*Vector3 posDiff = coll.transform.position - transform.position;
		transform.position += posDiff.normalized * 20.0f;*/
		// Debug.Log (gameObject.name + " BUMPED OFF "+ coll.gameObject.name);

		Vector3 avgNormal = Vector3.zero;

		foreach (ContactPoint contact in coll.contacts) {
			// Debug.DrawRay(contact.point, contact.normal, Color.white);
			avgNormal += contact.normal;
		}
		transform.position += avgNormal.normalized * 15.0f;

		if(fmodSource) {
			if(PlayerControl.instance.isDead == false) {
				fmodSource.StartEvent();
			}
		} else {
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.bumpSound, transform.position,
				1.0f, ( GetComponent<PlayerControl>() ? PlayerControl.instance.transform : null ));
		}

		Shootable shootableScript = GetComponent<Shootable>();
		if(shootableScript && shootableScript.healthLimit > 0) {
			shootableScript.ExplodeThis();
		}
	}
}
