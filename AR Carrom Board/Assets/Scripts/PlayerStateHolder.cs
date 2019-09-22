using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHolder : MonoBehaviour {
	public enum PlayerEnum { FirstPlayer, SecondPlayer, Unknown }
	private static PlayerEnum playertype = PlayerEnum.Unknown;
	public static PlayerEnum PlayerType
	{
		get { return playertype; }
		set {
			switch (value) {
			case PlayerEnum.FirstPlayer:
				playertype = PlayerEnum.FirstPlayer;
				PlayerPiece = PlayerPieceEnum.White;
				break;
			case PlayerEnum.SecondPlayer:
				playertype = PlayerEnum.SecondPlayer;
				PlayerPiece = PlayerPieceEnum.Black;
				break;
			}
		}
	}
	public enum PlayerHandEnum { LeftHand, RightHand, NoData }
	public static PlayerHandEnum PlayerHand { get; set; }
	public enum PlayerPieceEnum { Black, White }
	public static PlayerPieceEnum PlayerPiece { get; private set; }
	public static string PlayerName { get; set; }
	private static int playerage;
	public static int PlayerAge{
		get { return playerage; }
		set {
			playerage = value;
			if (GameStateHolder.GameMode == GameStateHolder.GameModesEnum.MultiPlayer) {
				GameStateHolder.PlayerGameDifficulty = GameStateHolder.GameDifficultyEnum.NotRequired;
			} else {
				if (value < 12) {
					GameStateHolder.PlayerGameDifficulty = GameStateHolder.GameDifficultyEnum.Easy;
				} else if (value < 19) {
					GameStateHolder.PlayerGameDifficulty = GameStateHolder.GameDifficultyEnum.Medium;
				} else {
					GameStateHolder.PlayerGameDifficulty = GameStateHolder.GameDifficultyEnum.Hard;
				}
			}
		}
	}
}
