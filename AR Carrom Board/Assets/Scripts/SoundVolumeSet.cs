using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeSet : MonoBehaviour {
	public AudioSource []BackgroundMusics;
	public AudioSource []GamePlayMusics;
	void Start () {
		float BackgroundMusicVol = PlayerPrefs.GetFloat ("BackgroundMusic", 1f);
		float GamePlayMusicVol = PlayerPrefs.GetFloat ("GamePlayMusic", 1f);
		foreach (AudioSource AS in BackgroundMusics) {
			AS.volume = BackgroundMusicVol;
		}
		foreach (AudioSource AS in GamePlayMusics) {
			AS.volume = GamePlayMusicVol;
		}
	}
}
