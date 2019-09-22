using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameStateHolder : MonoBehaviour {
	//Game Mode Info
	public enum GameModesEnum { SinglePlayer, MultiPlayer }
	public static GameModesEnum GameMode { get; set; }
	public enum GameDifficultyEnum { NotRequired, Easy, Medium, Hard}
	public static GameDifficultyEnum PlayerGameDifficulty { get; set; }
	public static PlayerStateHolder.PlayerEnum PlayerTurn { get; set; }
}
