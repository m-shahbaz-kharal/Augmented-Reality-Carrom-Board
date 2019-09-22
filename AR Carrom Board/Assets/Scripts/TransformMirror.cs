using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMirror : MonoBehaviour {
	public enum MirrorModeEnum { XPosYRot, WholeTransform }
	public MirrorModeEnum MirrorMode = MirrorModeEnum.XPosYRot;
	public Transform Source;

	public void SetMirrorMode(MirrorModeEnum mode){
		MirrorMode = mode;
	}

	void Update(){
		switch (MirrorMode) {
		case MirrorModeEnum.XPosYRot:
			transform.position = new Vector3 (Source.position.x, transform.position.y, transform.position.z);
			transform.localRotation = Quaternion.Euler (transform.localRotation.eulerAngles.x, Source.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
			break;
		case MirrorModeEnum.WholeTransform:
			transform.position = Source.position;
			transform.localRotation = Source.localRotation;
			break;
		}
	}
}
