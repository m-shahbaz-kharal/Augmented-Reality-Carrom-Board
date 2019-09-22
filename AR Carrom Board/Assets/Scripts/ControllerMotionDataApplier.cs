using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMotionDataApplier : MonoBehaviour {
	public ControllerDataCollector ControllerDataCollectorInstance;

	public float AccNormalizationFactor = 0.00025f;
	public float GyrNormalizationFactor = 1.0f / 32768.0f;   // 32768 is max value captured during test on imu

	public float MinRotY = -65f;
	public float MaxRotY = 65f;
	float RotY = 0;

	public float MinPosX = -0.325f;
	public float MaxPosX = 0.325f;
	float PosX = 0;

	// Increase the speed/influence rotation
	public float Factor = 7;
	public float AccMultiplier = 0.025f;


	public bool EnableRotation = true;
	public bool EnableTranslation = true;

	void Update()
	{
		float ax = 0f;
		float gy = 0f;
		switch (PlayerStateHolder.PlayerType) {
		case PlayerStateHolder.PlayerEnum.FirstPlayer:
			ax = ControllerDataCollectorInstance.Player1AccZ * AccNormalizationFactor;
			gy = ControllerDataCollectorInstance.Player1GyrX * GyrNormalizationFactor;
			break;
		case PlayerStateHolder.PlayerEnum.SecondPlayer:
			ax = -ControllerDataCollectorInstance.Player2AccZ * AccNormalizationFactor;
			gy = ControllerDataCollectorInstance.Player2GyrX * GyrNormalizationFactor;
			break;
		}

		// Prevent
		if (Mathf.Abs (ax) - 1 < 0) {
			ax = 0;
		}

		if ((PosX + ax) * AccMultiplier >= MinPosX && (PosX + ax) * AccMultiplier <= MaxPosX) {
			PosX += ax;
			Debug.Log ("Position: " + PosX.ToString ());
		}

		// Noise Cancellation
		if (Mathf.Abs (gy) < 0.025f) {
			gy = 0f;
		}
			
		RotY += gy;

		if (EnableTranslation) {
			transform.position = new Vector3 (-1 * PosX * AccMultiplier, transform.position.y, transform.position.z);
		}
		if (EnableRotation) {
			transform.localRotation = Quaternion.Euler (0f, Mathf.Clamp (RotY * Factor, MinRotY, MaxRotY), 0f);
		}
	}

	public void ResetPosRot(){
		PosX = 0f;
		RotY = 0f;
	}
}
