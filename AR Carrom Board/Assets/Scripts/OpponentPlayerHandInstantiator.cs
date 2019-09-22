using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentPlayerHandInstantiator : MonoBehaviour {
	public GameObject LeftHand, RightHand;
	public ControllerDataCollector ControllerDataCollectorInstance;
	public MPNetworkHandler MPNetworkHandlerInstance;
	public Transform ModelParent;

	private bool Instantiated = false;
	void Update(){
		if (MPNetworkHandlerInstance.OpponentPlayerHand == PlayerStateHolder.PlayerHandEnum.NoData) {
			Instantiated = false;
			try {
				GameObject.Destroy(transform.GetChild(0));
			}
			catch {}
			return;
		}
		if (!Instantiated) {
			GameObject Obj;
			switch (MPNetworkHandlerInstance.OpponentPlayerHand) {
			case PlayerStateHolder.PlayerHandEnum.LeftHand:
				Obj = Instantiate (LeftHand, this.transform.localPosition, this.transform.localRotation) as GameObject;
				Obj.AddComponent<ControllerStateDataApplier> ();
				Obj.GetComponent<ControllerStateDataApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
				Obj.GetComponent<ControllerStateDataApplier> ().PlayerType = (PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer ? PlayerStateHolder.PlayerEnum.SecondPlayer : PlayerStateHolder.PlayerEnum.FirstPlayer);
				Obj.transform.parent = ModelParent;
				Obj.transform.localPosition = Vector3.zero;
				Obj.transform.localRotation = Quaternion.identity;
				Obj.transform.localScale = Vector3.one;
				break;
			case PlayerStateHolder.PlayerHandEnum.RightHand:
				Obj = Instantiate (RightHand, this.transform.localPosition, this.transform.localRotation) as GameObject;
				Obj.AddComponent<ControllerStateDataApplier> ();
				Obj.GetComponent<ControllerStateDataApplier> ().ControllerDataCollectorInstance = ControllerDataCollectorInstance;
				Obj.GetComponent<ControllerStateDataApplier> ().PlayerType = (PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer ? PlayerStateHolder.PlayerEnum.SecondPlayer : PlayerStateHolder.PlayerEnum.FirstPlayer);
				Obj.transform.parent = ModelParent;
				Obj.transform.localPosition = Vector3.zero;
				Obj.transform.localRotation = Quaternion.identity;
				Obj.transform.localScale = Vector3.one;
				break;
			}
			Instantiated = true;
		}
	}
}
