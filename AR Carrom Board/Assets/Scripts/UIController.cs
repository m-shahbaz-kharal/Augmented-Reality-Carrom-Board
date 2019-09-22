using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
	#region UIButtons
	private enum MenuStateEnum{
		Init,

		Play, 
		SinglePlayerPlay, SinglePlayerControllerDetectionPhase,
		MultiPlayerPlay, MultiPlayerControllerDetectionPhase,

		Settings,

		Exit
	}
	public GameObject PanelMainMenu, PanelPlay, PanelSettings, PanelExit, PanelSinglePlayer, PanelMultiPlayer, PanelStartSinglePlayer, PanelStartMultiPlayer;
	public GameObject StatusBar;
	private MenuStateEnum MenuState;

	public AudioSource BackgroundMusic, GamePlayMusic;

	public void Start(){
		MenuState = MenuStateEnum.Init;
		BackgroundMusic.volume = PlayerPrefs.GetFloat ("BackgroundMusic", 1);
		PlayerStateHolder.PlayerHand = PlayerStateHolder.PlayerHandEnum.LeftHand;
	}


	public void OnClickPlay(){
		MenuState = MenuStateEnum.Play;
		UpdateUI ();
	}

	public void OnClickSinglePlayerPlay(){
		//GameControllerCall
		GameStateHolder.GameMode = GameStateHolder.GameModesEnum.SinglePlayer;

		MenuState = MenuStateEnum.SinglePlayerPlay;
		UpdateUI ();
	}

	public void OnClickGetInfoSinglePlayer(){
		if (PanelSinglePlayer.transform.Find ("IN_Name").GetComponent<InputField> ().text.Length > 0 && PanelSinglePlayer.transform.Find ("IN_Age").GetComponent<InputField> ().text.Length > 0) {
			MenuState = MenuStateEnum.SinglePlayerControllerDetectionPhase;
			UpdateUI ();
			PlayerStateHolder.PlayerType = PlayerStateHolder.PlayerEnum.FirstPlayer;
			PlayerStateHolder.PlayerName = PanelSinglePlayer.transform.Find ("IN_Name").GetComponent<InputField> ().text;
			PlayerStateHolder.PlayerAge = int.Parse (PanelSinglePlayer.transform.Find ("IN_Age").GetComponent<InputField> ().text);
		} else {
			StartCoroutine(SetStatusBarText ("INFO: Please input your name and age!"));
		}
	}

	public void OnClickLeftHandSinglePlayer(){
		PanelStartSinglePlayer.transform.Find ("BTN_LeftHand").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("ClickedImageHolder").GetComponent<Image>().sprite;
		PanelStartSinglePlayer.transform.Find ("BTN_RightHand").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("UnClickedImageHolder").GetComponent<Image>().sprite;
		PlayerStateHolder.PlayerHand = PlayerStateHolder.PlayerHandEnum.LeftHand;
	}

	public void OnClickRightHandSinglePlayer(){
		PanelStartSinglePlayer.transform.Find ("BTN_RightHand").GetComponent<Image> ().sprite = PanelStartSinglePlayer.transform.Find ("ClickedImageHolder").GetComponent<Image>().sprite;
		PanelStartSinglePlayer.transform.Find ("BTN_LeftHand").GetComponent<Image> ().sprite = PanelStartSinglePlayer.transform.Find ("UnClickedImageHolder").GetComponent<Image>().sprite;
		PlayerStateHolder.PlayerHand = PlayerStateHolder.PlayerHandEnum.RightHand;
	}

	public void OnClickMultiPlayerPlay(){
		//GameControllerCall
		GameStateHolder.GameMode = GameStateHolder.GameModesEnum.MultiPlayer;

		MenuState = MenuStateEnum.MultiPlayerPlay;
		UpdateUI ();
	}

	public void OnClickGetInfoMultiPlayer(){
		if (PanelMultiPlayer.transform.Find ("IN_Name").GetComponent<InputField> ().text.Length > 0 && PanelMultiPlayer.transform.Find ("IN_Age").GetComponent<InputField> ().text.Length > 0) {
			MenuState = MenuStateEnum.MultiPlayerControllerDetectionPhase;
			UpdateUI ();
		} else {
			StartCoroutine(SetStatusBarText ("INFO: Please input your name and age!"));
		}
	}

	public void OnClickPlayer1MultiPlayer(){
		PanelStartMultiPlayer.transform.Find ("BTN_Player1").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("ClickedImageHolder").GetComponent<Image>().sprite;
		PanelStartMultiPlayer.transform.Find ("BTN_Player2").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("UnClickedImageHolder").GetComponent<Image>().sprite;
		PlayerStateHolder.PlayerType = PlayerStateHolder.PlayerEnum.FirstPlayer;
		PlayerStateHolder.PlayerName = PanelMultiPlayer.transform.Find ("IN_Name").GetComponent<InputField> ().text;
		PlayerStateHolder.PlayerAge = int.Parse (PanelMultiPlayer.transform.Find ("IN_Age").GetComponent<InputField> ().text);
	}

	public void OnClickPlayer2MultiPlayer(){
		PanelStartMultiPlayer.transform.Find ("BTN_Player2").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("ClickedImageHolder").GetComponent<Image>().sprite;
		PanelStartMultiPlayer.transform.Find ("BTN_Player1").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("UnClickedImageHolder").GetComponent<Image>().sprite;
		PlayerStateHolder.PlayerType = PlayerStateHolder.PlayerEnum.SecondPlayer;
		PlayerStateHolder.PlayerName = PanelMultiPlayer.transform.Find ("IN_Name").GetComponent<InputField> ().text;
		PlayerStateHolder.PlayerAge = int.Parse (PanelMultiPlayer.transform.Find ("IN_Age").GetComponent<InputField> ().text);
	}

	public void OnClickLeftHandMultiPlayer(){
		PanelStartMultiPlayer.transform.Find ("BTN_LeftHand").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("ClickedImageHolder").GetComponent<Image>().sprite;
		PanelStartMultiPlayer.transform.Find ("BTN_RightHand").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("UnClickedImageHolder").GetComponent<Image>().sprite;
		PlayerStateHolder.PlayerHand = PlayerStateHolder.PlayerHandEnum.LeftHand;
	}

	public void OnClickRightHandMultiPlayer(){
		PanelStartMultiPlayer.transform.Find ("BTN_RightHand").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("ClickedImageHolder").GetComponent<Image>().sprite;
		PanelStartMultiPlayer.transform.Find ("BTN_LeftHand").GetComponent<Image> ().sprite = PanelStartMultiPlayer.transform.Find ("UnClickedImageHolder").GetComponent<Image>().sprite;
		PlayerStateHolder.PlayerHand = PlayerStateHolder.PlayerHandEnum.RightHand;
	}

	public void OnClickStartGame(){
		SceneManager.LoadScene ("SCN_PlayerOptionalPrefs", LoadSceneMode.Single);
	}

	public void OnClickSettings(){
		MenuState = MenuStateEnum.Settings;
		UpdateUI ();
		GameObject.Find ("SLDR_BackgroundMusic").GetComponent<Slider> ().value = BackgroundMusic.volume;
	}

	public void OnValueChangeBackgroundMusic(float f){
		if (BackgroundMusic == null) {
			return;
		}
		BackgroundMusic.volume = f;
	}

	public void OnValueChangeGamePlayMusic(float f){
		if (GamePlayMusic == null) {
			return;
		}
		GamePlayMusic.volume = f;
	}

	public void OnClickSaveAndExitSettings(){
		PlayerPrefs.SetFloat ("BackgroundMusic", BackgroundMusic.volume);
		MenuState = MenuStateEnum.Init;
		UpdateUI ();
	}

	public void OnClickExit(){
		MenuState = MenuStateEnum.Exit;
		UpdateUI ();
	}

	public void OnClickYesExit(){
		Application.Quit ();
	}

	public void OnClickNoExit(){
		MenuState = MenuStateEnum.Init;
		UpdateUI ();
	}

	private void UpdateUI(){
		switch (MenuState) {
		case MenuStateEnum.Init:
			PanelMainMenu.SetActive (true);
			PanelPlay.SetActive (false);
			PanelSettings.SetActive (false);
			PanelExit.SetActive (false);
			PanelSinglePlayer.SetActive (false);
			PanelMultiPlayer.SetActive (false);
			PanelStartSinglePlayer.SetActive (false);
			PanelStartMultiPlayer.SetActive (false);
			break;
		case MenuStateEnum.Play:
			PanelMainMenu.SetActive (true);
			PanelPlay.SetActive (true);
			PanelSettings.SetActive (false);
			PanelExit.SetActive (false);
			PanelSinglePlayer.SetActive (false);
			PanelMultiPlayer.SetActive (false);
			PanelStartSinglePlayer.SetActive (false);
			PanelStartMultiPlayer.SetActive (false);
			break;
		case MenuStateEnum.SinglePlayerPlay:
			PanelMainMenu.SetActive (true);
			PanelPlay.SetActive (true);
			PanelSettings.SetActive (false);
			PanelExit.SetActive (false);
			PanelSinglePlayer.SetActive (true);
			PanelMultiPlayer.SetActive (false);
			PanelStartSinglePlayer.SetActive (false);
			PanelStartMultiPlayer.SetActive (false);
			break;
		case MenuStateEnum.SinglePlayerControllerDetectionPhase:
			PanelMainMenu.SetActive (true);
			PanelPlay.SetActive (true);
			PanelSettings.SetActive (false);
			PanelExit.SetActive (false);
			PanelSinglePlayer.SetActive (true);
			PanelMultiPlayer.SetActive (false);
			PanelStartSinglePlayer.SetActive (true);
			PanelStartMultiPlayer.SetActive (false);
			break;
		case MenuStateEnum.MultiPlayerPlay:
			PanelMainMenu.SetActive (true);
			PanelPlay.SetActive (true);
			PanelSettings.SetActive (false);
			PanelExit.SetActive (false);
			PanelSinglePlayer.SetActive (false);
			PanelMultiPlayer.SetActive (true);
			PanelStartSinglePlayer.SetActive (false);
			PanelStartMultiPlayer.SetActive (false);
			break;

		case MenuStateEnum.MultiPlayerControllerDetectionPhase:
			PanelMainMenu.SetActive (true);
			PanelPlay.SetActive (true);
			PanelSettings.SetActive (false);
			PanelExit.SetActive (false);
			PanelSinglePlayer.SetActive (false);
			PanelMultiPlayer.SetActive (true);
			PanelStartSinglePlayer.SetActive (false);
			PanelStartMultiPlayer.SetActive (true);
			break;

		case MenuStateEnum.Settings:
			PanelMainMenu.SetActive (true);
			PanelPlay.SetActive (false);
			PanelSettings.SetActive (true);
			PanelExit.SetActive (false);
			PanelSinglePlayer.SetActive (false);
			PanelMultiPlayer.SetActive (false);
			PanelStartSinglePlayer.SetActive (false);
			PanelStartMultiPlayer.SetActive (false);
			break;
		case MenuStateEnum.Exit:
			PanelMainMenu.SetActive (true);
			PanelPlay.SetActive (false);
			PanelSettings.SetActive (false);
			PanelExit.SetActive (true);
			PanelSinglePlayer.SetActive (false);
			PanelMultiPlayer.SetActive (false);
			PanelStartSinglePlayer.SetActive (false);
			PanelStartMultiPlayer.SetActive (false);
			break;
		}
	}

	IEnumerator SetStatusBarText(string status){
		StatusBar.SetActive (true);
		StatusBar.GetComponent<Text> ().text = status;
		yield return new WaitForSeconds (1f);
		StatusBar.SetActive (false);
	}
	#endregion
}
