using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MPHandInstantiator : NetworkBehaviour {
	public GameObject LeftHand, RightHand;
	private MPControllerDataCollector ControllerDataCollectorInstance;
	public Transform ModelParent;
	private Transform FirstPlayerInstantPos, SecondPlayerInstantPos;
	[SyncVar (hook = "FirstPlayerHandTypeChangeCallBack")]
	private int FirstPlayerHandTypeInt = 0;
	[SyncVar (hook = "SecondPlayerHandTypeChangeCallBack")]
	private int SecondPlayerHandTypeInt = 0;
	public PlayerStateHolder.PlayerHandEnum FirstPlayerHandType = PlayerStateHolder.PlayerHandEnum.NoData, SecondPlayerHandType =  PlayerStateHolder.PlayerHandEnum.NoData;
	void FirstPlayerHandTypeChangeCallBack(int NewHandType){
		FirstPlayerHandTypeInt = NewHandType;
		switch (FirstPlayerHandTypeInt) {
		case 1:
			FirstPlayerHandType = PlayerStateHolder.PlayerHandEnum.LeftHand;
			break;
		case 2:
			FirstPlayerHandType = PlayerStateHolder.PlayerHandEnum.RightHand;
			break;
		}
	}

	void SecondPlayerHandTypeChangeCallBack(int NewHandType){
		SecondPlayerHandTypeInt = NewHandType;
		switch (SecondPlayerHandTypeInt) {
		case 1:
			SecondPlayerHandType = PlayerStateHolder.PlayerHandEnum.LeftHand;
			break;
		case 2:
			SecondPlayerHandType = PlayerStateHolder.PlayerHandEnum.RightHand;
			break;
		}
	}

	void Start(){
		switch (FirstPlayerHandTypeInt) {
		case 1:
			FirstPlayerHandType = PlayerStateHolder.PlayerHandEnum.LeftHand;
			break;
		case 2:
			FirstPlayerHandType = PlayerStateHolder.PlayerHandEnum.RightHand;
			break;
		}
		switch (SecondPlayerHandTypeInt) {
		case 1:
			SecondPlayerHandType = PlayerStateHolder.PlayerHandEnum.LeftHand;
			break;
		case 2:
			SecondPlayerHandType = PlayerStateHolder.PlayerHandEnum.RightHand;
			break;
		}
		if (isLocalPlayer) {
			gameObject.AddComponent<ColorPickerScript> ();
			gameObject.GetComponent<ColorPickerScript> ().ColorPickerMode = ColorPickerScript.ColorPickerModeEnum.FromPlayerPrefs;
			gameObject.GetComponent<ColorPickerScript> ().ModelParent = ModelParent.gameObject;

			switch (PlayerStateHolder.PlayerType) {
			case PlayerStateHolder.PlayerEnum.FirstPlayer:
				FirstPlayerHandType = PlayerStateHolder.PlayerHand;
				switch (FirstPlayerHandType) {
				case PlayerStateHolder.PlayerHandEnum.LeftHand:
					FirstPlayerHandTypeInt = 1;
					break;
				case PlayerStateHolder.PlayerHandEnum.RightHand:
					FirstPlayerHandTypeInt = 2;
					break;
				}
				break;
			case PlayerStateHolder.PlayerEnum.SecondPlayer:
				SecondPlayerHandType = PlayerStateHolder.PlayerHand;
				switch (SecondPlayerHandType) {
				case PlayerStateHolder.PlayerHandEnum.LeftHand:
					SecondPlayerHandTypeInt = 1;
					break;
				case PlayerStateHolder.PlayerHandEnum.RightHand:
					SecondPlayerHandTypeInt = 2;
					break;
				}
				break;
			}
		}
		ControllerDataCollectorInstance = (MPControllerDataCollector)GameObject.FindObjectOfType (typeof(MPControllerDataCollector));
		FirstPlayerInstantPos = GameObject.Find ("FirstPlayerInstantPos").transform;
		SecondPlayerInstantPos = GameObject.Find ("SecondPlayerInstantPos").transform;

		if (PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer) {
			if (isLocalPlayer) {
				transform.position = FirstPlayerInstantPos.position;
				transform.localRotation = FirstPlayerInstantPos.localRotation;

				GameObject Obj;
				switch (FirstPlayerHandType) {
				case PlayerStateHolder.PlayerHandEnum.LeftHand:
					Obj = Instantiate (LeftHand) as GameObject;

					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				case PlayerStateHolder.PlayerHandEnum.RightHand:
					Obj = Instantiate (RightHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				default:
					Obj = Instantiate (RightHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				}

			} else {
				transform.position = SecondPlayerInstantPos.position;
				transform.localRotation = SecondPlayerInstantPos.localRotation;

				GameObject Obj;
				switch (SecondPlayerHandType) {
				case PlayerStateHolder.PlayerHandEnum.LeftHand:
					Obj = Instantiate (LeftHand) as GameObject;

					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer ? PlayerStateHolder.PlayerEnum.SecondPlayer : PlayerStateHolder.PlayerEnum.FirstPlayer;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				case PlayerStateHolder.PlayerHandEnum.RightHand:
					Obj = Instantiate (RightHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer ? PlayerStateHolder.PlayerEnum.SecondPlayer : PlayerStateHolder.PlayerEnum.FirstPlayer;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				default:
					Obj = Instantiate (RightHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer ? PlayerStateHolder.PlayerEnum.SecondPlayer : PlayerStateHolder.PlayerEnum.FirstPlayer;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				}
			}
		} else if (PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.SecondPlayer) {
			if (isLocalPlayer) {
				transform.position = SecondPlayerInstantPos.position;
				transform.localRotation = SecondPlayerInstantPos.localRotation;

				GameObject Obj;
				switch (SecondPlayerHandType) {
				case PlayerStateHolder.PlayerHandEnum.LeftHand:
					Obj = Instantiate (LeftHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				case PlayerStateHolder.PlayerHandEnum.RightHand:
					Obj = Instantiate (RightHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				default:
					Obj = Instantiate (RightHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				}
			} else {
				transform.position = FirstPlayerInstantPos.position;
				transform.localRotation = FirstPlayerInstantPos.localRotation;

				GameObject Obj;
				switch (FirstPlayerHandType) {
				case PlayerStateHolder.PlayerHandEnum.LeftHand:
					Obj = Instantiate (LeftHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer ? PlayerStateHolder.PlayerEnum.SecondPlayer : PlayerStateHolder.PlayerEnum.FirstPlayer;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				case PlayerStateHolder.PlayerHandEnum.RightHand:
					Obj = Instantiate (RightHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer ? PlayerStateHolder.PlayerEnum.SecondPlayer : PlayerStateHolder.PlayerEnum.FirstPlayer;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				default:
					Obj = Instantiate (RightHand) as GameObject;
					if (ControllerDataCollectorInstance != null) {
						Obj.AddComponent<MPConrollerStateApplier> ();
						Obj.GetComponent<MPConrollerStateApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
						Obj.GetComponent<MPConrollerStateApplier> ().PlayerType = PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer ? PlayerStateHolder.PlayerEnum.SecondPlayer : PlayerStateHolder.PlayerEnum.FirstPlayer;
					}
					Obj.transform.parent = ModelParent;
					Obj.transform.localPosition = Vector3.zero;
					Obj.transform.localRotation = Quaternion.identity;
					Obj.transform.localScale = Vector3.one;
					break;
				}
			}
		}
	}
}
