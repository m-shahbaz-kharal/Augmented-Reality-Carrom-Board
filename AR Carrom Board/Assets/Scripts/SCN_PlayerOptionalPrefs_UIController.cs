using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCN_PlayerOptionalPrefs_UIController : MonoBehaviour {
	public void OnTapForStart(){
		switch (GameStateHolder.GameMode) {
		case GameStateHolder.GameModesEnum.SinglePlayer:
			SceneManager.LoadScene ("SCN_SinglePlayerGamePlay", LoadSceneMode.Single);
			break;
		case GameStateHolder.GameModesEnum.MultiPlayer:
			SceneManager.LoadScene ("SCN_MultiPlayerGamePlay", LoadSceneMode.Single);
			break;
		}
	}
}
