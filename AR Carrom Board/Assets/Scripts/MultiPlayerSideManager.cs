using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerSideManager : MonoBehaviour {
	public Transform Player1Side, Player2Side;
	public Transform LocalPlayer, OpponentPlayer;

	void Start () {
		switch (PlayerStateHolder.PlayerType) {
		case PlayerStateHolder.PlayerEnum.FirstPlayer:
			LocalPlayer.parent = Player1Side;
			LocalPlayer.localPosition = Vector3.zero;
			LocalPlayer.localRotation = Quaternion.identity;
			LocalPlayer.localScale = Vector3.one;
			OpponentPlayer.parent = Player2Side;
			OpponentPlayer.localPosition = Vector3.zero;
			OpponentPlayer.localRotation = Quaternion.identity;
			OpponentPlayer.localScale = Vector3.one;
			break;
		case PlayerStateHolder.PlayerEnum.SecondPlayer:
			LocalPlayer.parent = Player2Side;
			LocalPlayer.localPosition = Vector3.zero;
			LocalPlayer.localRotation = Quaternion.identity;
			LocalPlayer.localScale = Vector3.one;
			OpponentPlayer.parent = Player1Side;
			OpponentPlayer.localPosition = Vector3.zero;
			OpponentPlayer.localRotation = Quaternion.identity;
			OpponentPlayer.localScale = Vector3.one;
			break;
		}
	}
}
