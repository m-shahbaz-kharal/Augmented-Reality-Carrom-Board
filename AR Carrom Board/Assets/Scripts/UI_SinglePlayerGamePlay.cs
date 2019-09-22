using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class UI_SinglePlayerGamePlay : MonoBehaviour {
	public GameObject PauseMenu;
	private bool Paused = false;

	public UnityEngine.UI.Image ToggleSoundImage;
	public UnityEngine.UI.Image MuteImage, UnMuteImage;
	private float PrevVol = 0f;
	private bool Mute = false;

	public Text PlayerTurnNotify;

	private bool FlashOn = false;
	public UnityEngine.UI.Image FlashUI;
	public UnityEngine.UI.Image OnFlash, OffFlash;

	void Start(){
		PrevVol = AudioListener.volume;
	}

	void Update(){
		UpdatePlayerTurnUI ();
	}

	public void PauseResume(){
		if (Paused) {
			Time.timeScale = 1;
			PauseMenu.SetActive (false);
		} else {
			Time.timeScale = 0;
			PauseMenu.SetActive (true);
		}
		Paused = !Paused;
	}

	public void SoundToggle(){
		Mute = !Mute;
		if (Mute) {
			AudioListener.volume = 0f;
			ToggleSoundImage.sprite = UnMuteImage.sprite;
		} else {
			AudioListener.volume = PrevVol;
			ToggleSoundImage.sprite = MuteImage.sprite;
		}
	}

	public void ExitToMainMenu(){
		//TODO: Stop all the vuforia behaviours here
		Time.timeScale = 1f;
		SceneManager.LoadScene ("SCN_MainMenu", LoadSceneMode.Single);
	}

	public void ToggleCamerFlash(){
		FlashOn = !FlashOn;
		CameraDevice.Instance.SetFlashTorchMode (FlashOn);
		if (FlashOn) {
			FlashUI.sprite = OnFlash.sprite;
		} else {
			FlashUI.sprite = OffFlash.sprite;
		}
	}

	void UpdatePlayerTurnUI(){
		switch (GameStateHolder.PlayerTurn) {
		case PlayerStateHolder.PlayerEnum.Unknown:
			PlayerTurnNotify.text = "WAITING ...";
			break;
		case PlayerStateHolder.PlayerEnum.FirstPlayer:
			PlayerTurnNotify.text = "FIRST PLAYER'S TURN";
			break;
		case PlayerStateHolder.PlayerEnum.SecondPlayer:
			PlayerTurnNotify.text = "SECOND PLAYER'S TURN";
			break;
		}
	}
}
