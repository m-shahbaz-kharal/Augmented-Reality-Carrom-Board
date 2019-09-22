using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MPNameCanvas : NetworkBehaviour {
	public Text Identity;

	void Start(){
		if (isLocalPlayer) {
			Identity.text = "You";
		} else {
			Identity.text = "He";
		}
	}
}
