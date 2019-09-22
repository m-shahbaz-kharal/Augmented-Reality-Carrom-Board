using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MPNetworkTransformApplier : MonoBehaviour {
	public float InterpolationDelta = 0.1f;
	public MPNetworkHandler MPNetworkHandlerInstance;

	void Update() {
		try {
			this.transform.localPosition = new Vector3(Mathf.Lerp(this.transform.localPosition.x, MPNetworkHandlerInstance.OpponentPosX, InterpolationDelta), this.transform.localPosition.y, this.transform.localPosition.z);
			this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y, this.transform.localRotation.eulerAngles.z);
		}
		catch (Exception e) {
			Debug.Log (e.Message);
		}
	}
}
