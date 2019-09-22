using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Collider))]
public class PlayerShootScript : MonoBehaviour {
	private ControllerMotionDataApplier ControllerMotionDataApplierInstance;
	private bool StrikerIsClose = false;
	private bool Play = false;

	private Animator PlayerHandAnimator;

	private GameObject Striker = null;

	public float ShootSpeedIncreasePerSecond = 1f;
	public float MaxShootSpeed = 10f;
	[HideInInspector]
	public float ShootSpeed = 0f;

	//As PlayerShootScript will be on HandModel and HandModel will be child of either LocalPlayerImageTarget or OpponentPlayer
	private Transform Parent;

	//VisionAssist
	private bool VisionAssistBit = false;
	public float NormalTransparency = 1f;
	public float AssistTransparency = 0.5f;

	void Start() {
		Parent = transform.parent;
		PlayerHandAnimator = GetComponent<Animator> ();
		ControllerMotionDataApplierInstance = GetComponentInParent<ControllerMotionDataApplier> ();
	}

	void Update(){
		//I changed GameStateHolder.PlayerTurn == PlayerPrefs.PlayerType if any error come have a look at it.
		if (StrikerIsClose && GameStateHolder.PlayerTurn != PlayerStateHolder.PlayerEnum.Unknown) {
			Play = true;
		} else {
			Play = false;
		}

		if (Play) {
			int CurrAnimState = PlayerHandAnimator.GetInteger ("ShootState");
			switch (CurrAnimState) {
			case 0: //IDLE
				if (ControllerMotionDataApplierInstance != null) {
					ControllerMotionDataApplierInstance.EnableTranslation = true;
					ControllerMotionDataApplierInstance.EnableRotation = false;
				}
				MoveStrikerWithHand ();
				ShootSpeed = 0f;
				break;
			case 1: //PREPARE AIM
				if (ControllerMotionDataApplierInstance != null) {
					ControllerMotionDataApplierInstance.EnableTranslation = false;
					ControllerMotionDataApplierInstance.EnableRotation = true;
				}
				DirectStrikerWithHand ();
				ShootSpeed = 0f;
				break;
			case 2: //AIM
				if (ControllerMotionDataApplierInstance != null) {
					ControllerMotionDataApplierInstance.EnableTranslation = false;
					ControllerMotionDataApplierInstance.EnableRotation = false;
				}
				VisionAssist (false);
				ShootSpeed += ShootSpeedIncreasePerSecond * Time.deltaTime;
				if (ShootSpeed > MaxShootSpeed) {
					ShootSpeed = MaxShootSpeed;
				}
				break;
			case 3: //SHOOT
				if (ControllerMotionDataApplierInstance != null) {
					ControllerMotionDataApplierInstance.EnableTranslation = false;
					ControllerMotionDataApplierInstance.EnableRotation = false;
					ControllerMotionDataApplierInstance.ResetPosRot ();
				}
				VisionAssist (false);
				Striker.GetComponent<Rigidbody> ().velocity = Striker.transform.forward * ShootSpeed;
				ShootSpeed = 0f;
				GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.Unknown;
				PlayerHandAnimator.SetInteger ("ShootState", 0);
				break;
			}
		}
	}

	void VisionAssist(bool val){
		if (VisionAssistBit != val) {
			VisionAssistBit = val;
			if (val) {
				transform.GetChild(0).GetChild(0).GetComponent<Renderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, AssistTransparency);
			} else {
				transform.GetChild(0).GetChild(0).GetComponent<Renderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, NormalTransparency);
			}
		}
	}

	void MoveStrikerWithHand(){
		if (Striker != null && Parent != null) {
			VisionAssist (true);
			Striker.transform.position = new Vector3 (Mathf.Clamp(Parent.position.x, -0.3f, 0.3f), Striker.transform.position.y, Striker.transform.position.z);
		} else {
			Debug.Log ("Striker is null.");
		}
	}

	void DirectStrikerWithHand() {
		if (Striker != null && Parent != null) {
			VisionAssist (true);
			Striker.transform.localRotation = Quaternion.Euler (Striker.transform.localRotation.x, Parent.eulerAngles.y, Striker.transform.localRotation.z);
		} else {
			Debug.Log ("Striker is null.");
		}
	}

	void OnTriggerEnter(Collider C){
		if(C.tag.Equals("Striker")) {
			StrikerIsClose = true;
			Striker = C.gameObject;
		}
	}

	void OnTriggerExit(Collider C) {
		if(C.tag.Equals("Striker")) {
			StrikerIsClose = false;
			Striker = null;
		}
	}
}
