using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISplashHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (LoadNextScene());
	}
	
	IEnumerator LoadNextScene(){
		yield return new WaitForSeconds (9f);
		SceneManager.LoadScene ("SCN_MainMenu", LoadSceneMode.Single);
	}
}
