	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerHandInstantiator : MonoBehaviour {
	public GameObject LeftHand, RightHand;
	public ControllerDataCollector ControllerDataCollectorInstance;
	public Transform ModelParent;
	void Start(){
		GameObject Obj;
		switch (PlayerStateHolder.PlayerHand) {
		case PlayerStateHolder.PlayerHandEnum.LeftHand:
			Obj = Instantiate (LeftHand, this.transform.localPosition, this.transform.localRotation) as GameObject;

			if (ControllerDataCollectorInstance != null) {
				Obj.AddComponent<ControllerStateDataApplier> ();
				Obj.GetComponent<ControllerStateDataApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
				Obj.GetComponent<ControllerStateDataApplier> ().PlayerType = PlayerStateHolder.PlayerType;
			}
			Obj.transform.parent = ModelParent;
			Obj.transform.localPosition = Vector3.zero;
			Obj.transform.localRotation = Quaternion.identity;
			Obj.transform.localScale = Vector3.one;
			break;
		case PlayerStateHolder.PlayerHandEnum.RightHand:
			Obj = Instantiate (RightHand, this.transform.localPosition, this.transform.localRotation) as GameObject;
			if (ControllerDataCollectorInstance != null) {
				Obj.AddComponent<ControllerStateDataApplier> ();
				Obj.GetComponent<ControllerStateDataApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
				Obj.GetComponent<ControllerStateDataApplier> ().PlayerType = PlayerStateHolder.PlayerType;
			}
			Obj.transform.parent = ModelParent;
			Obj.transform.localPosition = Vector3.zero;
			Obj.transform.localRotation = Quaternion.identity;
			Obj.transform.localScale = Vector3.one;
			break;
		}
	}
}
