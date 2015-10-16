using UnityEngine;
using System.Collections;

public class SpawnTicketBooth : MonoBehaviour {
	public float numSpawned = 0;
	public float numAllowed = 5;

	public static SpawnTicketBooth instance;

	void Start() {
		if(instance != null) {
			Destroy(instance);
		}
		instance = this;
	}

	public void reportDeath() {
		numSpawned--;
	}

	public bool requestSpawnTicket() {
		if(numSpawned < numAllowed) {
			numSpawned++;
			return true;
		}
		return false;
	}
}
