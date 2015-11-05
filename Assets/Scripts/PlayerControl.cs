using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public static PlayerControl instance;

	public bool autoLevel = false;
	public bool isDead = false;

	public float pitchSpeed = 50.0f;
	public float rollSpeed = 50.0f;
	public float yawSpeed = 50.0f;
	public float dodgeSpeed = 20.0f;
	public float maxSpeed = 90.0f;


	private float rocketReloadTime = 2.5f;
	private float rocketReloadTimeLeft = 0.0f;

	public float cooldownTimeNeeded = 0.75f;
	public float timeBetweenShots = 0.1f;
	private float gunHeat = 0.0f;
	private float gunCooldownTimeLeft = 0.0f;

	public AudioSource engineVolume;
	public Camera stretchFOV;

	public Text damageReadout;
	private int startHealth;
	private int wasHealth;
	private Shootable shootableScript;

    public Text targetReadout;
    private float targetDistance = 0;
    public GameObject missionTarget;
	private HardPointCounter hardpointRef = null;
    private int hardpointMax = 0;

	public GameObject playerExplodePrefab;
    public GameObject explodePrefabGeneral;

	public GameObject rocketPrefab;
	public GameObject rocketHardpoint;    

	private GameObject throttleReadout;
	private GameObject SpeedReadout;

	private RadarManager rmData;

	private UI_Speed UI_SpeedReadout;
	private UI_Speed UI_ThrottleReadout;

	private bool maxThrottle = false;
	private float throttle = 0.0f;
	private float throttleSmooth = 0.0f;

	private float asymThrot = 1.0f;
	private Vector3 strafeAxis = Vector3.zero;
	public GameObject spawnUponShotHit;
	private bool isTurningDampenSpeed = false;

	private float hairpinSpinAmt = 0.0f;
	private Quaternion spin180Goal;
	private Quaternion spin180From;
	private bool isHairpin180 = false;

	private int rocketsLoaded = 4;
	private int rocketSalvo = 0;

	private bool activelyStrafing = false;

	MuzzleFlash mFlash;
	private float reloadTime = 0.0f;
	private int dispThrotVal = 0;

	private string dangerBaseText = "";
	public GameObject optionalDangerHeightMeasure;
	public Text optionalDangerHeightReadout;

	private Image rocketHUD;
	public Sprite[] rocketGraphics;

    void Awake() {
		instance = this;
	}

	void Start() {
		rmData = GetComponent<RadarManager>();
		rocketHUD = GameObject.Find ("fake weapon hud").GetComponent<Image>();;
		rocketHUD.gameObject.SetActive(true);
		SpeedReadout = GameObject.Find("Speed");
		throttleReadout = GameObject.Find("Throttle");

		if(optionalDangerHeightReadout) {
			dangerBaseText = optionalDangerHeightReadout.text;
		}

		mFlash = GetComponent<MuzzleFlash>();
		mFlash.Reset();
		StartCoroutine(rocketSalvoRelease());
		shootableScript = GetComponent<Shootable>();
		wasHealth = startHealth = shootableScript.healthLimit;
		UI_ThrottleReadout = throttleReadout.GetComponent<UI_Speed> ();
		UI_SpeedReadout = SpeedReadout.GetComponent<UI_Speed> ();

		hardpointRef = missionTarget.GetComponentInChildren<HardPointCounter>();

		hardpointMax = hardpointRef.hardpointCount;
	}

	IEnumerator rocketSalvoRelease() { // also periodically updates throttle readout
		while(true) {
			if(rocketSalvo > 0) {
				rocketSalvo--;
				GameObject tempRocket = GameObject.Instantiate(rocketPrefab,
				                       rocketHardpoint.transform.position
				                       +rocketSalvo*1.5f*transform.right,
				                       rocketHardpoint.transform.rotation) as GameObject;
				RocketMotion rmScript = tempRocket.GetComponent<RocketMotion>();
				rmScript.inheritSpeedBoost( maxSpeed * throttleSmooth );

				if(rmData.nearestTargets != null && rocketSalvo < rmData.nearestTargets.Count && 
				   rmData.nearestTargets[rocketSalvo] &&
				   rmData.nearestTargets[rocketSalvo].playerLOS) {
					RocketMotion rocket = tempRocket.GetComponent<RocketMotion>();
					// Debug.Log ("Adding rocket #"+rocketSalvo+" to " +rmData.nearestTargets.Count);
					rocket.target = rmData.nearestTargets[rocketSalvo].transform;
				}
			}

			dispThrotVal = Mathf.CeilToInt(asymThrot * 239
			                               + Random.Range(0,5)*asymThrot);
			yield return new WaitForSeconds(0.15f);
		}
	}
	
    void updateTargetReadout(){
		if(missionTarget) {
	        targetDistance = Vector3.Distance(transform.position, missionTarget.transform.position);

	        string textOut = "";
	        //TARGET: VHERAIN TITAN
			textOut += "TARGET: "+missionTarget.name+"\n";
	        //DISTANCE:  573m
	        textOut += "DISTANCE: " + Mathf.FloorToInt(targetDistance) + "m \n";
	        //WEAKSPOTS 20 / 25
			textOut += "WEAKSPOTS: " + hardpointRef.hardpointCount +"/ " + hardpointMax;
			targetReadout.text = textOut; 
		}
		else {
			targetReadout.text = "MISSION COMPLETE\nTARGET DESTROYED\nRETURNING TO COMMAND...";
		}
    }

    void updateThrottleReadout() {
		bool wasMaxThrottle = maxThrottle;

		UI_SpeedReadout.currentSpeed = (int)(throttleSmooth * 10);
		UI_ThrottleReadout.counter = (int) Mathf.Floor(throttle * 10);

		maxThrottle = (UI_SpeedReadout.currentSpeed >= 9);

		if(wasMaxThrottle != maxThrottle) {
			if(maxThrottle) {
				SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.hyperSpeed, transform.position, 1.0f,
					transform);
			} else {
				SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.hyperSpeedCancel, transform.position, 1.0f,
					transform);
			}
		}
		//throttleReadout.text = textOut;
	}

	void updateHealthReadout() {
		if(wasHealth > shootableScript.healthLimit) {
			int diff = wasHealth-shootableScript.healthLimit;
			for(int i=0;i<diff;i++) {
				GameObject newGo = (GameObject)GameObject.Instantiate(
					spawnUponShotHit,
					transform.position + transform.forward * 3.0f +
					Random.Range(-1.0f,1.0f) * transform.right +
					Random.Range(-1.0f,1.0f) * transform.up,
					Random.rotation
					);
				newGo.transform.parent = transform;
			}
			wasHealth = shootableScript.healthLimit;
		}
        //Armor: 12 / 12
        damageReadout.text = "ARMOR: " + shootableScript.healthLimit + " / " + startHealth + "\n";

		damageReadout.text += "SPEED: " + dispThrotVal + " KPH \n"; 
		//Speed: 180 KPH -- changed by cdeleon from KM / H to fit easier

        //Heat: NORMAL
		if(gunCooldownTimeLeft > 0.0f) {
			damageReadout.text += "MG HEAT: HOLD\n";
		} else {
			damageReadout.text += "MG HEAT: "+ Mathf.CeilToInt(gunHeat*100) +"%\n";
		}

        //Power:  Stable
		if(rocketReloadTimeLeft <= 0.0f) {
        	damageReadout.text += "ROCKETS: READY";
		} else {
			int rocketCountdown = Mathf.CeilToInt(rocketReloadTimeLeft*40);
			if(rocketCountdown > 99) {
				rocketCountdown = 99;
			}
			damageReadout.text += "ROCKETS: <"+ rocketCountdown +">";
		}

    }

	public void PlayerDie() {
		if(isDead == false) {
			isDead = true;
			throttleReadout.transform.parent.gameObject.SetActive(false);
			rmData.radarSphere.transform.parent.gameObject.SetActive(false);
			GameObject blast = (GameObject)GameObject.Instantiate(playerExplodePrefab, transform.position + transform.forward * 5.0f,
			                       transform.rotation);
			blast.transform.parent = transform;
			Rigidbody myRB = GetComponent<Rigidbody>();
			myRB.constraints = RigidbodyConstraints.None;
			myRB.AddTorque(Random.onUnitSphere * 50.0f);
			myRB.AddForce(Random.onUnitSphere * 1500.0f);
			FinishedLevel();
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.megashipBoom, 
				Camera.main.transform.position, 1.0f,
				Camera.main.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel("Level Select");
		}

		/*if(Input.GetKeyDown(KeyCode.P)) {
			PlayerDie();
		}*/

		if(isDead) {
			return;
		}

		if(optionalDangerHeightMeasure) {
			int diff = (int)(transform.position.y - optionalDangerHeightMeasure.transform.position.y);
			if(diff > 0) {
				optionalDangerHeightReadout.text = dangerBaseText;
			} else {
				optionalDangerHeightReadout.text = "" + (-diff) + "m below detection";
			}
		}

		float wasThrottle = throttle;

		if(missionTarget == null) {
			transform.rotation = 
				Quaternion.Slerp(transform.rotation,
				                 Quaternion.LookRotation(Vector3.up), Time.deltaTime * 3.0f);
			throttle = 1.0f;
			isHairpin180 = false;
		} else {
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

		}

		if(throttle > wasThrottle) {
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.throttleUp, transform.position, 1.0f, transform);
		} else if(throttle < wasThrottle) {
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.throttleDown, transform.position, 1.0f, transform);
		}

		asymThrot = (1.0f-throttleSmooth);
		asymThrot *= asymThrot;
		asymThrot = 1.0f-asymThrot;
		engineVolume.volume = 0.1f+0.7f*asymThrot;

		if(maxThrottle) {
			asymThrot *= 4.0f;
		}

		float newFOV = 55.0f + asymThrot*10.0f;
		float kSpring = 0.95f;
		stretchFOV.fieldOfView = kSpring*stretchFOV.fieldOfView + (1.0f-kSpring)*newFOV;

		transform.position += asymThrot * transform.forward * Time.deltaTime * maxSpeed;

		float dodgeInput = Input.GetAxisRaw("Roll");

		if(activelyStrafing) {
			if(dodgeInput == 0.0f) {
				activelyStrafing = false;
			}
		} else {
			if(dodgeInput < -0.1f)  {
				strafeAxis = transform.right;
				activelyStrafing = true;
				SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.playerDodge, transform.position, 1.0f, transform);
			} else if(dodgeInput > 0.1f)  {
				strafeAxis = -transform.right;
				activelyStrafing = true;
				SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.playerDodge, transform.position, 1.0f, transform);
			}
		}

		if(rocketReloadTimeLeft > 0.0f) {
			rocketReloadTimeLeft -= Time.deltaTime;
			if(rocketReloadTimeLeft <= 0.0f || GameStateStaticProgress.cheatsOn) {
				if(rocketsLoaded < 4) {
					if(rocketsLoaded == 0) {
						rocketHUD.gameObject.SetActive(true);
					}
					rocketHUD.sprite = rocketGraphics[rocketsLoaded];
					rocketsLoaded++;
					rocketReloadTimeLeft = rocketReloadTime;
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Return)) {
			if(rocketSalvo == 0 && rocketsLoaded > 0) {
				if(GameStateStaticProgress.cheatsOn == false) {
					rocketSalvo = rocketsLoaded;
					rocketReloadTimeLeft = rocketReloadTime;
					rocketsLoaded = 0;
					rocketHUD.gameObject.SetActive(false);
				} else {
					rocketSalvo = rocketsLoaded;
				}
			} else {
				SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.laserFire, transform.position, 1.0f, transform);
			}
		}

		if(Input.GetKeyDown(KeyCode.X) && isHairpin180==false && missionTarget != null) {
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.playerDodge, transform.position, 1.0f, transform);
			isHairpin180 = true;
			hairpinSpinAmt = 0.0f;
			spin180From = transform.rotation;
			spin180Goal = transform.rotation 
					* Quaternion.AngleAxis(180.0f,Vector3.right);
		}

		if(missionTarget == null) {
			// on autopilot, ignore inputs
		} else if(isHairpin180 == false) {
			isTurningDampenSpeed = (Mathf.Abs( Input.GetAxis("Vertical") ) > 0.2f ||
		                        Mathf.Abs( Input.GetAxis("Horizontal") ) > 0.2f);

			transform.Rotate(
				Input.GetAxis("Vertical") * Time.deltaTime * pitchSpeed,
				Input.GetAxis("Horizontal") * Time.deltaTime * yawSpeed,
				(GameStateStaticProgress.uprightDodge ? 0.0f :
				Input.GetAxis("Roll") * Time.deltaTime * rollSpeed)
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
					SoundCenter.instance.PlayClipOn(
						SoundCenter.instance.throttleDown, transform.position, 1.0f, transform);

					spin180From = transform.rotation;
					spin180Goal = transform.rotation 
						* Quaternion.AngleAxis(180.0f,Vector3.forward);
				}
				transform.rotation = 
					Quaternion.Lerp(spin180From, spin180Goal, lastTurn);
			}

			if(hairpinSpinAmt > 1.0f) {
				SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.throttleUp, transform.position, 1.0f, transform);
				isHairpin180 = false;
			}
		}

		updateThrottleReadout();
		updateHealthReadout();
        updateTargetReadout();

		if(gunCooldownTimeLeft >= 0.0f) {
			gunCooldownTimeLeft -= Time.deltaTime;
		}

        if (reloadTime >= 0.0f) {
			reloadTime -= Time.deltaTime;
		} else if(Input.GetKey(KeyCode.Space) && gunCooldownTimeLeft <= 0.0f) {
			gunHeat += timeBetweenShots * 0.5f;
			if(gunHeat > 1.0f) {
				if(GameStateStaticProgress.cheatsOn) {
					gunHeat = 1.0f;
				} else {
					gunCooldownTimeLeft = cooldownTimeNeeded;
					SoundCenter.instance.PlayClipOn(
						SoundCenter.instance.throttleDown, transform.position, 1.0f, transform);
				}
			}

			mFlash.Strobe();

			SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.playerMG2, transform.position, Random.Range(0.2f,0.4f),
					transform);

			Ray playerGun = new Ray(transform.position, transform.forward);
			float missBy = 0.75f;
			Quaternion missQuat = Quaternion.AngleAxis(Random.Range(-missBy,missBy),Camera.main.transform.up);
			missQuat = missQuat * Quaternion.AngleAxis(Random.Range(-missBy,missBy),Camera.main.transform.right);
			playerGun.direction = missQuat * playerGun.direction;

			RaycastHit rhInfo;
			int layerMask = ~(1<<12);
			if( Physics.Raycast(playerGun, out rhInfo, 4000.0f, layerMask)) {
				Vector3 towardPlayer = rhInfo.point - transform.position;
				GameObject newGo = (GameObject)GameObject.Instantiate(
					spawnUponShotHit,
					rhInfo.point - towardPlayer.normalized * 2.0f,
					Quaternion.identity
					);
				newGo.transform.parent = rhInfo.collider.transform;
				Shootable thingShot = rhInfo.collider.GetComponent<Shootable>();
				SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.playerMG1, transform.position, Random.Range(0.3f,0.4f),
					transform);
				if(thingShot) {
					thingShot.DamageThis();
				}
				// Destroy( rhInfo.collider.gameObject );
			}
			reloadTime = timeBetweenShots;
		} else if(gunHeat > 0.0f) {
			gunHeat -= Time.deltaTime;
			if(gunHeat <0.0f) {
				gunHeat = 0.0f;
			}
		}
	}

	void FixedUpdate() {
		float smoothK = 0.98f;
		float dampenedIfTurning = (isTurningDampenSpeed ? throttle*0.45f : throttle);
		throttleSmooth = smoothK*throttleSmooth + (1.0f-smoothK)*dampenedIfTurning;

		if(autoLevel) {
			Vector3 fromEuler = transform.rotation.eulerAngles;
			fromEuler.z = 0.0f;
			transform.rotation = Quaternion.Slerp( transform.rotation,
			                                      Quaternion.Euler (fromEuler), Time.deltaTime );
		}

		if(activelyStrafing == false) {
			strafeAxis *= 0.97f;
		}
	}

	public void FinishedLevel() {
		StartCoroutine(ReturnToLevelSelectAfterWait());
	}

	IEnumerator ReturnToLevelSelectAfterWait() {
		yield return new WaitForSeconds(6.5f);
		Application.LoadLevel("Level Select");
	}
}
