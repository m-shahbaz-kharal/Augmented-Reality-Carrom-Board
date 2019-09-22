using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDebug : MonoBehaviour {
	public GameStateHolder.GameModesEnum GameMode = GameStateHolder.GameModesEnum.MultiPlayer;
	public PlayerStateHolder.PlayerEnum PlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
	public PlayerStateHolder.PlayerEnum PlayerType = PlayerStateHolder.PlayerEnum.FirstPlayer;
	public string PlayerName = "XYZ";
	public int PlayerAge = 13;
	public PlayerStateHolder.PlayerHandEnum PlayerHand = PlayerStateHolder.PlayerHandEnum.LeftHand;

	public void Start () {
		GameStateHolder.GameMode = GameMode;
		GameStateHolder.PlayerTurn = PlayerTurn;
		PlayerStateHolder.PlayerType = PlayerType;
		PlayerStateHolder.PlayerName = PlayerName;
		PlayerStateHolder.PlayerAge = PlayerAge;
		PlayerStateHolder.PlayerHand = PlayerHand;
	}

}
