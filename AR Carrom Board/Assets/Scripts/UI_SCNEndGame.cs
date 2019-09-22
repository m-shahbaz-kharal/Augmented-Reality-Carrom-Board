using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_SCNEndGame : MonoBehaviour {
	public Text Winner;
	public Text Details;

	void Start(){
		int WinnerInt = PlayerPrefs.GetInt ("Winner", -1);
		if (WinnerInt == 1) {
			Winner.text = "Player ONE";
			if (PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer) {
				Details.text = PlayerStateHolder.PlayerName + " now has total " + TurnAndScore.GetScores (PlayerStateHolder.PlayerName) + " scores. Congratulations!!!";
			} else {
				Details.gameObject.SetActive (false);
			}
		} else if(WinnerInt == 2) {
			Winner.text = "Player TWO";
			if (PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.SecondPlayer) {
				Details.text = PlayerStateHolder.PlayerName + " now has total " + TurnAndScore.GetScores (PlayerStateHolder.PlayerName) + " scores. Congratulations!!!";
			} else {
				Details.gameObject.SetActive (false);
			}
		}
	}

	public void GoToMainMenu(){
		SceneManager.LoadScene ("SCN_MainMenu");
	}
}
