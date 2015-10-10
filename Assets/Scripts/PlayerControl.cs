using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public static PlayerControl instance;

	public float pitchSpeed = 50.0f;
	public float rollSpeed = 50.0f;
	public float yawSpeed = 50.0f;
	public float dodgeSpeed = 20.0f;
	public float maxSpeed = 90.0f;

	public Camera stretchFOV;
	public Text throttleReadout;

	public GameObject explodePrefabGeneral;

	private float throttle = 0.0f;
	private float throttleSmooth = 0.0f;
	private Vector3 strafeAxis = Vector3.zero;
	public GameObject spawnUponShotHit;
	private bool isTurningDampenSpeed = false;

	private float hairpinSpinAmt = 0.0f;
	private Quaternion spin180Goal;
	private Quaternion spin180From;
	private bool isHairpin180 = false;

	private bool activelyStrafing = false;

	MuzzleFlash mFlash;
	private float reloadTime = 0.0f;

	void Start() {
		instance = this;
		mFlash = GetComponent<MuzzleFlash>();
		mFlash.Reset();
	}

	void updateThrottleReadout() {
		string textOut = "";
		for(float f=0.0f;f<1.0f;f+=0.08f) {
			if(throttleSmooth > f) {
				textOut += "|";
			} else {
				textOut += ".";
			}
		}
		throttleReadout.text = textOut;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			throttle = 0.0f;
		} else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			throttle = 0.1f;
		} else if(Input.GetKeyDown(KeyCode.Alpha3)) {
			throttle = 0.2f;
		} else if(Input.GetKeyDown(KeyCode.Alpha4)) {
			throttle = 0.3f;
		} else if(Input.GetKeyDown(KeyCode.Alpha5)) {
			throttle = 0.4f;
		} else if(Input.GetKeyDown(KeyCode.Alpha6)) {
			throttle = 0.5f;
		} else if(Input.GetKeyDown(KeyCode.Alpha7)) {
			throttle = 0.6f;
		} else if(Input.GetKeyDown(KeyCode.Alpha8)) {
			throttle = 0.7f;
		} else if(Input.GetKeyDown(KeyCode.Alpha9)) {
			throttle = 0.8f;
		} else if(Input.GetKeyDown(KeyCode.Alpha0)) {
			throttle = 1.0f;
		}


		float asymThrot = (1.0f-throttleSmooth);
		asymThrot *= asymThrot;
		asymThrot = 1.0f-asymThrot;
		transform.position += asymThrot * transform.forward * Time.deltaTime * maxSpeed;
		stretchFOV.fieldOfView = 55.0f + asymThrot*10.0f;

		float dodgeInput = Input.GetAxisRaw("Roll");

		if(activelyStrafing) {
			if(dodgeInput == 0.0f) {
				activelyStrafing = false;
			}
		} else {
			if(dodgeInput < -0.1f)  {
				strafeAxis = transform.right;
				activelyStrafing = true;
			} else if(dodgeInput > 0.1f)  {
				strafeAxis = -transform.right;
				activelyStrafing = true;
			}
		}

		if(Input.GetKeyDown(KeyCode.X) && isHairpin180==false) {
			isHairpin180 = true;
			hairpinSpinAmt = 0.0f;
			spin180From = transform.rotation;
			spin180Goal = transform.rotation 
					* Quaternion.AngleAxis(180.0f,Vector3.right);
		}

		if(isHairpin180 == false) {
			isTurningDampenSpeed = (Mathf.Abs( Input.GetAxis("Vertical") ) > 0.2f ||
		                        Mathf.Abs( Input.GetAxis("Horizontal") ) > 0.2f);

			transform.Rotate(
				Input.GetAxis("Vertical") * Time.deltaTime * pitchSpeed,
				Input.GetAxis("Horizontal") * Time.deltaTime * yawSpeed,
				Input.GetAxis("Roll") * Time.deltaTime * rollSpeed
				);

			transform.position += strafeAxis * Time.deltaTime * dodgeSpeed;
		} else {
			bool wasBelow = (hairpinSpinAmt < 0.5f);
			hairpinSpinAmt += Time.deltaTime * 0.7f;

			isTurningDampenSpeed = true;

			if(hairpinSpinAmt < 0.5f) {
				float halfTurn = hairpinSpinAmt * 2.0f;
				transform.rotation = 
					Quaternion.Lerp(spin180From, spin180Goal, halfTurn);
			} else {
				float lastTurn = hairpinSpinAmt * 2.0f-1.0f;
				if(wasBelow) {
					spin180From = transform.rotation;
					spin180Goal = transform.rotation 
						* Quaternion.AngleAxis(180.0f,Vector3.forward);
				}
				transform.rotation = 
					Quaternion.Lerp(spin180From, spin180Goal, lastTurn);
			}

			if(hairpinSpinAmt > 1.0f) {
				isHairpin180 = false;
			}
		}

		updateThrottleReadout();

		if(reloadTime >= 0.0f) {
			reloadTime -= Time.deltaTime;
		} else if(Input.GetKey(KeyCode.Space)) {
			mFlash.Strobe();

			Ray playerGun = new Ray(transform.position, transform.forward);
			float missBy = 3.0f;
			Quaternion missQuat = Quaternion.AngleAxis(Random.Range(-missBy,missBy),Camera.main.transform.up);
			missQuat = missQuat * Quaternion.AngleAxis(Random.Range(-missBy,missBy),Camera.main.transform.right);
			playerGun.direction = missQuat * playerGun.direction;

			RaycastHit rhInfo;
			if( Physics.Raycast(playerGun, out rhInfo)) {
				Vector3 towardPlayer = rhInfo.point - transform.position;
				GameObject newGo = (GameObject)GameObject.Instantiate(
					spawnUponShotHit,
					rhInfo.point - towardPlayer.normalized * 2.0f,
					Quaternion.identity
					);
				newGo.transform.parent = rhInfo.collider.transform;
				Shootable thingShot = rhInfo.collider.GetComponent<Shootable>();
				if(thingShot) {
					thingShot.DamageThis();
				}
				// Destroy( rhInfo.collider.gameObject );
			}
			reloadTime = 0.1f;
		}
	}

	void FixedUpdate() {
		float smoothK = 0.98f;
		float dampenedIfTurning = (isTurningDampenSpeed ? throttle*0.45f : throttle);
		throttleSmooth = smoothK*throttleSmooth + (1.0f-smoothK)*dampenedIfTurning;

		if(activelyStrafing == false) {
			strafeAxis *= 0.97f;
		}
	}
}
