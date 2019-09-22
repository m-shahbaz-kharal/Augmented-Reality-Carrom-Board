using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPResetRotPosFinder : MonoBehaviour {
	private GameObject []Players;

	void Update(){
		Players = (GameObject[])GameObject.FindObjectsOfType (typeof(MPControllerMotionDataApplier));
	}

	public void ResetPos(){
		if (Players != null) {
			foreach (GameObject G in Players) {
				if (G != null) {
					G.GetComponent<MPControllerMotionDataApplier> ().ResetPosRot ();
				}
			}
		}
	}
}
