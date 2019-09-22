using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraAutoFocus : MonoBehaviour {
	void Start () {
		VuforiaARController VuAR = VuforiaARController.Instance;
		VuAR.RegisterVuforiaStartedCallback (OnVuStarted);
		VuAR.RegisterOnPauseCallback (OnVuPaused);
	}

	void OnVuStarted(){
		CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	void OnVuPaused(bool Paused){
		if (!Paused) {
			CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		}
	}
}
