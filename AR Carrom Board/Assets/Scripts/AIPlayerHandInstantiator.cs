using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerHandInstantiator : MonoBehaviour {
	public GameObject LeftHand, RightHand;

	void Start(){
		GameObject Obj;
		int RandomVal = Random.Range (1, 3);
		switch (RandomVal) {
		case 1:
			Obj = Instantiate (LeftHand, this.transform.localPosition, this.transform.localRotation) as GameObject;
			Obj.AddComponent<AIPlayerDataApplier> ();
			Obj.transform.parent = this.transform;
			Obj.transform.localPosition = Vector3.zero;
			Obj.transform.localRotation = Quaternion.identity;
			Obj.transform.localScale = Vector3.one;
			break;
		case 2:
			Obj = Instantiate (RightHand, this.transform.localPosition, this.transform.localRotation) as GameObject;
			Obj.AddComponent<AIPlayerDataApplier> ();
			Obj.transform.parent = this.transform;
			Obj.transform.localPosition = Vector3.zero;
			Obj.transform.localRotation = Quaternion.identity;
			Obj.transform.localScale = Vector3.one;
			break;
		}
	}
}
