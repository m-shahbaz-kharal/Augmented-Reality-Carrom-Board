using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideSoundPlay : MonoBehaviour {
	void OnCollisionEnter(){
		GetComponent<AudioSource> ().Play ();
	}
}
