using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeadlyLaserScan : MonoBehaviour {
	Quaternion startRot;
	public Image screenHeat;
	private Color screenHeatcol;
	public Transform playerPos;
	public Transform dangerHeight;
	public float dangerLimit = 2.0f;
	private float dangerTime = 0.0f;

	// Use this for initialization
	void Start () {
		startRot = transform.rotation;
		screenHeatcol = screenHeat.color;
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion targetRot;
		if(playerPos.position.y > dangerHeight.position.y) {
			dangerTime += Time.deltaTime;
			targetRot = Quaternion.LookRotation( playerPos.position - transform.position );
			if(dangerTime >= dangerLimit) {
				dangerTime = dangerLimit;
				if(screenHeatcol.a <= 0.0f) {
					SoundCenter.instance.PlayClipOn(
						SoundCenter.instance.megalaserDie, playerPos.position, Random.Range(0.8f,1.0f),
						playerPos);
				}
				if(screenHeatcol.a < 1.0f) {
					screenHeatcol.a += Time.deltaTime;
					if(screenHeatcol.a > 1.0f) {
						screenHeatcol.a = 1.0f;
						Application.LoadLevel("Level Select");
					}
					screenHeat.color = screenHeatcol;
				}
			}
		} else {
			if(screenHeatcol.a > 0.0f) {
				screenHeatcol.a -= Time.deltaTime;
				if(screenHeatcol.a < 0.0f) {
					screenHeatcol.a = 0.0f;
				}
				screenHeat.color = screenHeatcol;
			}

			dangerTime = 0.0f;

			targetRot = Quaternion.AngleAxis(
				Mathf.Sin( Time.time * 0.3f ) * 60.0f - 20.0f, Vector3.up ) * startRot;
		}
		transform.rotation = Quaternion.Slerp( transform.rotation,
		                                      targetRot, 3.0F * Time.deltaTime );
	}
}
