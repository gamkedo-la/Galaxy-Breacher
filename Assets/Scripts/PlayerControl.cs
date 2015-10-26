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

	public AudioSource engineVolume;
	public Camera stretchFOV;
	public Text throttleReadout;
    private bool maxThrottle = false;

	public Text damageReadout;
	private int startHealth;
	private int wasHealth;
	private Shootable shootableScript;

    public Text targetReadout;
    private float targetDistance = 0;
    public GameObject missionTarget;
	private HardPointCounter hardpointRef = null;
    private int hardpointMax = 0;

    public GameObject explodePrefabGeneral;

	public GameObject rocketPrefab;
	public GameObject rocketHardpoint;    

	private float throttle = 0.0f;
	private float throttleSmooth = 0.0f;
	private Vector3 strafeAxis = Vector3.zero;
	public GameObject spawnUponShotHit;
	private bool isTurningDampenSpeed = false;

	private float hairpinSpinAmt = 0.0f;
	private Quaternion spin180Goal;
	private Quaternion spin180From;
	private bool isHairpin180 = false;

	private int rocketSalvo = 0;

	private bool activelyStrafing = false;

	MuzzleFlash mFlash;
	private float reloadTime = 0.0f;



    void Awake() {
		instance = this;
	}

	void Start() {
		mFlash = GetComponent<MuzzleFlash>();
		mFlash.Reset();
		StartCoroutine(rocketSalvoRelease());
		shootableScript = GetComponent<Shootable>();
		wasHealth = startHealth = shootableScript.healthLimit;

		hardpointRef = missionTarget.GetComponentInChildren<HardPointCounter>();

		hardpointMax = hardpointRef.hardpointCount;
	}

	IEnumerator rocketSalvoRelease() {
		while(true) {
			if(rocketSalvo > 0) {
				rocketSalvo--;
				GameObject tempRocket = GameObject.Instantiate(rocketPrefab,
				                       rocketHardpoint.transform.position
				                       +rocketSalvo*1.5f*transform.right,
				                       rocketHardpoint.transform.rotation) as GameObject;
				RocketMotion rmScript = tempRocket.GetComponent<RocketMotion>();
				rmScript.inheritSpeedBoost( maxSpeed * throttleSmooth );
			}
			yield return new WaitForSeconds(0.15f);
		}
	}
	
    void updateTargetReadout(){
        targetDistance = Vector3.Distance(transform.position, missionTarget.transform.position);

        string textOut = "";
        //TARGET: VHERAIN TITAN
        textOut += "TARGET: VHERAIN TITAN \n";
        //DISTANCE:  573m
        textOut += "DISTANCE: " + Mathf.FloorToInt(targetDistance) + "m \n";
        //WEAKSPOTS 20 / 25
		textOut += "WEAKSPOTS: " + hardpointRef.hardpointCount +"/ " + hardpointMax;

        targetReadout.text = textOut; 

    }

    void updateThrottleReadout() {
		string textOut = "";
		bool wasMaxThrottle = maxThrottle;
		maxThrottle = true; // true unless any '.' get drawn to display
		for(float f=0.0f;f<1.0f;f+=0.08f) {
			if(throttleSmooth > f) {
				textOut += "|";
			} else {
				maxThrottle = false; // clearly haven't maxed the bar, cut hyperdrive
				textOut += ".";
			}
		}
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
		throttleReadout.text = textOut;
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

        //Speed: 180 KM / H
        damageReadout.text += "SPEED: " + Mathf.CeilToInt(throttleSmooth * 180) + " KM / H \n"; 

        //Heat: NORMAL
        damageReadout.text += "HEAT: NORMAL \n";

        //Power:  Stable
        damageReadout.text += "POWER: STABLE";

    }
	
	// Update is called once per frame
	void Update () {
		float wasThrottle = throttle;
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

		if(throttle > wasThrottle) {
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.throttleUp, transform.position, 1.0f, transform);
		} else if(throttle < wasThrottle) {
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.throttleDown, transform.position, 1.0f, transform);
		}

		float asymThrot = (1.0f-throttleSmooth);
		asymThrot *= asymThrot;
		asymThrot = 1.0f-asymThrot;
		engineVolume.volume = 0.1f+0.7f*asymThrot;

		stretchFOV.fieldOfView = 55.0f + asymThrot*10.0f;
		if(maxThrottle) {
			asymThrot *= 4.0f;
		}
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

		if(Input.GetKeyDown(KeyCode.Return) && rocketSalvo == 0) {
			rocketSalvo = 5;
		}

		if(Input.GetKeyDown(KeyCode.X) && isHairpin180==false) {
			SoundCenter.instance.PlayClipOn(
				SoundCenter.instance.playerDodge, transform.position, 1.0f, transform);
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

        if (reloadTime >= 0.0f) {
			reloadTime -= Time.deltaTime;
		} else if(Input.GetKey(KeyCode.Space)) {
			mFlash.Strobe();

			SoundCenter.instance.PlayClipOn(
					SoundCenter.instance.playerMG2, transform.position, Random.Range(0.6f,0.8f),
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
					SoundCenter.instance.playerMG1, transform.position, Random.Range(0.2f,0.5f),
					transform);
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
