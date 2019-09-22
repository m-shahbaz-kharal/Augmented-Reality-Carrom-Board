using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[RequireComponent (typeof(BoxCollider))]
public class MPTurnAndScore : NetworkBehaviour {
	[SyncVar (hook="OnFirstTurnAllocated")]
	private bool FirstTurnAllocated = false;
	[SyncVar (hook="PrevPlayerTurnChange")]
	private int PrevPlayerTurnInt = -1;
	private bool StrikerPlacedFirstTime = false;
	private PlayerStateHolder.PlayerEnum PrevPlayerTurn;
	private bool MainCoroutineStarted = false;
	public float StopTolerance = 0.1f;

	private ArrayList FirstPlayerPocketedArray, SecondPlayerPocketedArray;
	private ArrayList FirstPlayerTempPocketedArray, SecondPlayerTempPocketedArray;
	private bool FirstPlayerQueenPocketed = false, SecondPlayerQueenPocketed = false;
	private bool FirstPlayerQueenCovered = false, SecondPlayerQueenCovered = false;
	private int FirstPlayerPenalityDue = 0, SecondPlayerPenalityDue = 0;

	public float TimeDelayBeforeBaseCoroutine = 0.5f;
	public float StrikerZeroPos = 6.75f;

	public void OnFirstTurnAllocated(bool val){
		FirstTurnAllocated = val;
	}

	public void PrevPlayerTurnChange(int val){
		PrevPlayerTurnInt = val;
		switch (PrevPlayerTurnInt) {
		case 1:
			PrevPlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
			break;
		case 2:
			PrevPlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
			break;
		}
	}

	public int GetScoreFirstPlayer(){
		if (FirstPlayerPocketedArray != null)
			return FirstPlayerPocketedArray.Count;
		else
			return 0;
	}

	public int GetScoreSecondPlayer(){
		if (SecondPlayerPocketedArray != null)
			return SecondPlayerPocketedArray.Count;
		else
			return 0;
	}

	public PlayerStateHolder.PlayerEnum GetQueenPocketed(){
		if (FirstPlayerQueenPocketed) {
			return PlayerStateHolder.PlayerEnum.FirstPlayer;
		} else if (SecondPlayerQueenPocketed) {
			return PlayerStateHolder.PlayerEnum.SecondPlayer;
		} else {
			return PlayerStateHolder.PlayerEnum.Unknown;
		}
	}

	public PlayerStateHolder.PlayerEnum GetQueenCovered(){
		if (FirstPlayerQueenCovered) {
			return PlayerStateHolder.PlayerEnum.FirstPlayer;
		} else if (SecondPlayerQueenCovered) {
			return PlayerStateHolder.PlayerEnum.SecondPlayer;
		} else {
			return PlayerStateHolder.PlayerEnum.Unknown;
		}
	}

	private DisplayScores DisplayScoreScript;

	void Start() {
		GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.Unknown;
		PrevPlayerTurn = PlayerStateHolder.PlayerEnum.Unknown;
		FirstPlayerPocketedArray = new ArrayList ();
		SecondPlayerPocketedArray = new ArrayList ();

		DisplayScoreScript = (DisplayScores)GameObject.FindObjectOfType (typeof(DisplayScores));
		PlayerPrefs.SetInt ("Winner", -1);
	}

	void Update() {
		if (isServer) {
			if (!FirstTurnAllocated) {
				//Tossing
				int val = Random.Range (1, 3);
				switch (val) {
				case 1:
					GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
					PrevPlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
					PrevPlayerTurnInt = 1;
					break;
				case 2:
					GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
					PrevPlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
					PrevPlayerTurnInt = 2;
					break;
				}
				FirstTurnAllocated = true;
			}
		}
		if (isClient) {
			if (!FirstTurnAllocated) {
				switch (PrevPlayerTurnInt) {
				case 1:
					GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
					PrevPlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
					break;
				case 2:
					GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
					PrevPlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
					break;
				}
				FirstTurnAllocated = true;
			}
		}

		if (isServer) {
			if (!StrikerPlacedFirstTime) {
				StrikerPlacedFirstTime = true;
				//Placing Striker
				GameObject Striker = GameObject.FindGameObjectWithTag ("Striker");
				switch (GameStateHolder.PlayerTurn) {
				case PlayerStateHolder.PlayerEnum.FirstPlayer:
					Striker.transform.localPosition = new Vector3 (0f, 0f, (-1) * StrikerZeroPos);
					Striker.transform.localRotation = Quaternion.identity;
					break;
				case PlayerStateHolder.PlayerEnum.SecondPlayer:
					Striker.transform.localPosition = new Vector3 (0f, 0f, StrikerZeroPos);
					Striker.transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
					break;
				}
			}
		}

		if(GameStateHolder.PlayerTurn == PlayerStateHolder.PlayerEnum.Unknown) {
			if (!MainCoroutineStarted) {
				MainCoroutineStarted = true;
				StartCoroutine (MainCoroutine ());
			}
		}
	}

	IEnumerator MainCoroutine() {
		FirstPlayerTempPocketedArray = new ArrayList ();
		SecondPlayerTempPocketedArray = new ArrayList ();
		yield return new WaitForSeconds (TimeDelayBeforeBaseCoroutine);
		while(!EveryPieceAndStrikerStopped()){
			yield return new WaitForSeconds (1f);
		}
		bool Foul = false;
		int WhitePiecesPocketed = FirstPlayerPocketedArray.Count;
		int BlackPiecesPocketed = SecondPlayerPocketedArray.Count;
		switch (PrevPlayerTurn) {
		case PlayerStateHolder.PlayerEnum.FirstPlayer:
			foreach (GameObject G in FirstPlayerTempPocketedArray) {
				//Striker Pocketting Foul
				if (G.tag.Equals ("Striker")) {
					Foul = true;
				}
				//Opponent Piece Pocketting Foul
				if (G.tag.Equals ("BlackPiece")) {
					BlackPiecesPocketed++;
					Foul = true;
				}
				//Queen Covering
				if (FirstPlayerPocketedArray.Count > 0 && G.tag.Equals ("Queen")) {
					FirstPlayerQueenPocketed = true;
				}
				if (FirstPlayerQueenPocketed && G.tag.Equals ("WhitePiece")) {
					FirstPlayerQueenCovered = true;
				}
				//All White Pieces Pocketted Before Queen => Foul
				if (G.tag.Equals ("WhitePiece")) {
					WhitePiecesPocketed++;
					if (WhitePiecesPocketed == 9 && !FirstPlayerQueenPocketed) {
						Foul = true;
					}
				}
			}
			if (Foul) {
				DisplayScoreScript.ShowNotification (DisplayScores.NotificationTypeEnum.Foul, "Player 1's Foul");
				//Opponent Last Piece Pocketting Handling
				if (BlackPiecesPocketed == 9) {
					foreach (GameObject G in FirstPlayerTempPocketedArray) {
						if (G.tag.Equals ("BlackPiece")) {
							G.transform.localPosition = Vector3.zero;
							G.GetComponent<Rigidbody> ().isKinematic = false;
							break;
						}
					}
				}
				//Own Foul Pieces Handling
				foreach (GameObject G in FirstPlayerTempPocketedArray) {
					if (G.tag.Equals ("WhitePiece")) {
						G.transform.localPosition = Vector3.zero;
						G.GetComponent<Rigidbody> ().isKinematic = false;
					}
				}
				//Giving A Penality Piece
				bool PenalityGiven = false;
				if (FirstPlayerPocketedArray.Count > 0) {
					((GameObject)FirstPlayerPocketedArray [0]).transform.localPosition = Vector3.zero;
					((GameObject)FirstPlayerPocketedArray [0]).GetComponent<Rigidbody> ().isKinematic = false;
					FirstPlayerPocketedArray.RemoveAt (0);
					PenalityGiven = true;
				}
				//Duing Piece if not Penality Given
				if (!PenalityGiven) {
					FirstPlayerPenalityDue++;
				}

				//Returning Queen if not Covered
				if (!FirstPlayerQueenCovered && FirstPlayerQueenPocketed) {
					bool QueenReturned = false;
					foreach (GameObject G in FirstPlayerTempPocketedArray) {
						if (G.tag.Equals ("Queen")) {
							G.transform.localPosition = Vector3.zero;
							G.GetComponent<Rigidbody> ().isKinematic = false;
							FirstPlayerQueenPocketed = false;
							QueenReturned = true;
							break;
						}
					}
					if (!QueenReturned) {
						foreach (GameObject G in FirstPlayerPocketedArray) {
							if (G.tag.Equals ("Queen")) {
								G.transform.localPosition = Vector3.zero;
								G.GetComponent<Rigidbody> ().isKinematic = false;
								FirstPlayerPocketedArray.Remove (G);
								FirstPlayerQueenPocketed = false;
								QueenReturned = true;
								break;
							}
						}
					}
				}
				//Changing Turn
				yield return new WaitForSeconds(1f);
				PrevPlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
				GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
				PrevPlayerTurnInt = 2;
			} 
			// No Foul
			else {
				//Counting Pieces Pocketted
				int WhitePocketedThisTurn = 0;
				foreach (GameObject G in FirstPlayerTempPocketedArray) {
					if (G.tag.Equals ("WhitePiece")) {
						WhitePocketedThisTurn++;
					}
				}
				//If No White Piece Pocketted
				if (WhitePocketedThisTurn == 0) {
					DisplayScoreScript.ShowNotification (DisplayScores.NotificationTypeEnum.Info, "No White Piece Pocketed");
					//Returning Queen if not Covered
					if (!FirstPlayerQueenCovered && FirstPlayerQueenPocketed) {
						bool QueenReturned = false;
						foreach (GameObject G in FirstPlayerTempPocketedArray) {
							if (G.tag.Equals ("Queen")) {
								G.transform.localPosition = Vector3.zero;
								G.GetComponent<Rigidbody> ().isKinematic = false;
								FirstPlayerQueenPocketed = false;
								QueenReturned = true;
								break;
							}
						}
						if (!QueenReturned) {
							foreach (GameObject G in FirstPlayerPocketedArray) {
								if (G.tag.Equals ("Queen")) {
									G.transform.localPosition = Vector3.zero;
									G.GetComponent<Rigidbody> ().isKinematic = false;
									FirstPlayerPocketedArray.Remove (G);
									FirstPlayerQueenPocketed = false;
									QueenReturned = true;
									break;
								}
							}
						}
					}
					//Changing Turn
					yield return new WaitForSeconds(1f);
					PrevPlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
					GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
					PrevPlayerTurnInt = 2;
				}
				//If Some pieces are pocketted
				else {
					DisplayScoreScript.ShowNotification (DisplayScores.NotificationTypeEnum.Score, WhitePocketedThisTurn.ToString() + " Piece(s) Pocketed");
					//Adding All White Pieces to Our Array
					foreach (GameObject G in FirstPlayerTempPocketedArray) {
						if (G.tag.Equals ("WhitePiece") || G.tag.Equals ("Queen")) {
							FirstPlayerPocketedArray.Add (G);
						}
					}
					yield return new WaitForSeconds(1f);
					PrevPlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
					GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
					PrevPlayerTurnInt = 1;
				}
				//Giving Due Penality Piece if Possible
				while (FirstPlayerPocketedArray.Count > 0) {
					if (FirstPlayerPenalityDue > 0) {
						((GameObject)FirstPlayerPocketedArray [0]).transform.localPosition = Vector3.zero;
						((GameObject)FirstPlayerPocketedArray [0]).GetComponent<Rigidbody> ().isKinematic = false;
						FirstPlayerPocketedArray.RemoveAt (0);
						FirstPlayerPenalityDue--;
					} else {
						break;
					}
				}
				if (GameStateHolder.GameMode == GameStateHolder.GameModesEnum.SinglePlayer && FirstPlayerQueenCovered && FirstPlayerPocketedArray.Count == 10) {
					int ScoresToSave = GetScores (PlayerStateHolder.PlayerName);
					switch (GameStateHolder.PlayerGameDifficulty) {
					case GameStateHolder.GameDifficultyEnum.Easy:
						ScoresToSave += 5;
						break;
					case GameStateHolder.GameDifficultyEnum.Medium:
						ScoresToSave += 10;
						break;
					case GameStateHolder.GameDifficultyEnum.Hard:
						ScoresToSave += 15;
						break;
					}
					SaveScores (PlayerStateHolder.PlayerName, ScoresToSave);
					PlayerPrefs.SetInt ("Winner", 1);
					PlayerPrefs.Save ();
					SceneManager.LoadScene ("EndGame");
				} else if (GameStateHolder.GameMode == GameStateHolder.GameModesEnum.MultiPlayer && (FirstPlayerQueenCovered && PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.FirstPlayer) && FirstPlayerPocketedArray.Count == 10) {
					int ScoresToSave = GetScores (PlayerStateHolder.PlayerName);
					ScoresToSave += 10;
					SaveScores (PlayerStateHolder.PlayerName, ScoresToSave);
					PlayerPrefs.SetInt ("Winner", 1);
					PlayerPrefs.Save ();
					SceneManager.LoadScene ("EndGame");
				}
			}
			break;
		case PlayerStateHolder.PlayerEnum.SecondPlayer:
			foreach (GameObject G in SecondPlayerTempPocketedArray) {
				//Striker Pocketting Foul
				if (G.tag.Equals ("Striker")) {
					Foul = true;
				}
				//Opponent Piece Pocketting Foul
				if (G.tag.Equals ("WhitePiece")) {
					WhitePiecesPocketed++;
					Foul = true;
				}
				//Queen Covering
				if (SecondPlayerPocketedArray.Count > 0 && G.tag.Equals ("Queen")) {
					SecondPlayerQueenPocketed = true;
				}
				if (SecondPlayerQueenPocketed && G.tag.Equals ("BlackPiece")) {
					SecondPlayerQueenCovered = true;
				}
				//All White Pieces Pocketted Before Queen => Foul
				if (G.tag.Equals ("BlackPiece")) {
					BlackPiecesPocketed++;
					if (BlackPiecesPocketed == 9 && !SecondPlayerQueenPocketed) {
						Foul = true;
					}
				}
			}
			if (Foul) {
				DisplayScoreScript.ShowNotification (DisplayScores.NotificationTypeEnum.Foul, "Player 2's Foul");
				//Opponent Last Piece Pocketting Handling
				if (WhitePiecesPocketed == 9) {
					foreach (GameObject G in SecondPlayerTempPocketedArray) {
						if (G.tag.Equals ("WhitePiece")) {
							G.transform.localPosition = Vector3.zero;
							G.GetComponent<Rigidbody> ().isKinematic = false;
							break;
						}
					}
				}
				//Own Foul Pieces Handling
				foreach (GameObject G in SecondPlayerTempPocketedArray) {
					if (G.tag.Equals ("BlackPiece")) {
						G.transform.localPosition = Vector3.zero;
						G.GetComponent<Rigidbody> ().isKinematic = false;
					}
				}
				//Giving A Penality Piece
				bool PenalityGiven = false;
				if (SecondPlayerPocketedArray.Count > 0) {
					((GameObject)SecondPlayerPocketedArray [0]).transform.localPosition = Vector3.zero;
					((GameObject)SecondPlayerPocketedArray [0]).GetComponent<Rigidbody> ().isKinematic = false;
					SecondPlayerPocketedArray.RemoveAt (0);
					PenalityGiven = true;
				}
				//Duing Piece if not Penality Given
				if (!PenalityGiven) {
					SecondPlayerPenalityDue++;
				}

				//Returning Queen if not Covered
				if (!SecondPlayerQueenCovered && SecondPlayerQueenPocketed) {
					bool QueenReturned = false;
					foreach (GameObject G in SecondPlayerTempPocketedArray) {
						if (G.tag.Equals ("Queen")) {
							G.transform.localPosition = Vector3.zero;
							G.GetComponent<Rigidbody> ().isKinematic = false;
							SecondPlayerQueenPocketed = false;
							QueenReturned = true;
							break;
						}
					}
					if (!QueenReturned) {
						foreach (GameObject G in SecondPlayerPocketedArray) {
							if (G.tag.Equals ("Queen")) {
								G.transform.localPosition = Vector3.zero;
								G.GetComponent<Rigidbody> ().isKinematic = false;
								SecondPlayerPocketedArray.Remove (G);
								SecondPlayerQueenPocketed = false;
								QueenReturned = true;
								break;
							}
						}
					}
				}
				//Changing Turn
				yield return new WaitForSeconds(1f);
				PrevPlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
				GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
				PrevPlayerTurnInt = 1;
			} 
			// No Foul
			else {
				//Counting Pieces Pocketted
				int BlackPocketedThisTurn = 0;
				foreach (GameObject G in SecondPlayerTempPocketedArray) {
					if (G.tag.Equals ("BlackPiece")) {
						BlackPocketedThisTurn++;
					}
				}
				//If No White Piece Pocketted
				if (BlackPocketedThisTurn == 0) {
					DisplayScoreScript.ShowNotification (DisplayScores.NotificationTypeEnum.Info, "No Black Piece Pocketed");
					//Returning Queen if not Covered
					if (!SecondPlayerQueenCovered && SecondPlayerQueenPocketed) {
						bool QueenReturned = false;
						foreach (GameObject G in SecondPlayerTempPocketedArray) {
							if (G.tag.Equals ("Queen")) {
								G.transform.localPosition = Vector3.zero;
								G.GetComponent<Rigidbody> ().isKinematic = false;
								SecondPlayerQueenPocketed = false;
								QueenReturned = true;
								break;
							}
						}
						if (!QueenReturned) {
							foreach (GameObject G in SecondPlayerPocketedArray) {
								if (G.tag.Equals ("Queen")) {
									G.transform.localPosition = Vector3.zero;
									G.GetComponent<Rigidbody> ().isKinematic = false;
									SecondPlayerPocketedArray.Remove (G);
									SecondPlayerQueenPocketed = false;
									QueenReturned = true;
									break;
								}
							}
						}
					}
					//Changing Turn
					yield return new WaitForSeconds(1f);
					PrevPlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
					GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.FirstPlayer;
					PrevPlayerTurnInt = 1;
				}
				//If Some pieces are pocketted
				else {
					DisplayScoreScript.ShowNotification (DisplayScores.NotificationTypeEnum.Score, BlackPocketedThisTurn.ToString() + " Piece(s) Pocketed");
					//Adding All White Pieces to Our Array
					foreach (GameObject G in SecondPlayerTempPocketedArray) {
						if (G.tag.Equals ("BlackPiece") || G.tag.Equals ("Queen")) {
							SecondPlayerPocketedArray.Add (G);
						}
					}
					yield return new WaitForSeconds(1f);
					PrevPlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
					GameStateHolder.PlayerTurn = PlayerStateHolder.PlayerEnum.SecondPlayer;
					PrevPlayerTurnInt = 2;
				}
				//Giving Due Penality Piece if Possible
				while (SecondPlayerPocketedArray.Count > 0) {
					if (SecondPlayerPenalityDue > 0) {
						((GameObject)SecondPlayerPocketedArray [0]).transform.localPosition = Vector3.zero;
						((GameObject)SecondPlayerPocketedArray [0]).GetComponent<Rigidbody> ().isKinematic = false;
						SecondPlayerPocketedArray.RemoveAt (0);
						SecondPlayerPenalityDue--;
					} else {
						break;
					}
				}
			}
			if (GameStateHolder.GameMode == GameStateHolder.GameModesEnum.MultiPlayer && (SecondPlayerQueenCovered && PlayerStateHolder.PlayerType == PlayerStateHolder.PlayerEnum.SecondPlayer) && SecondPlayerPocketedArray.Count == 10) {
				int ScoresToSave = GetScores (PlayerStateHolder.PlayerName);
				ScoresToSave += 10;
				SaveScores (PlayerStateHolder.PlayerName, ScoresToSave);
				PlayerPrefs.SetInt ("Winner", 2);
				PlayerPrefs.Save ();
				SceneManager.LoadScene ("EndGame");
			}
			break;
		}
		if (isServer) {
			//Placing Striker
			GameObject Striker = GameObject.FindGameObjectWithTag ("Striker");
			Striker.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			Striker.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			switch (GameStateHolder.PlayerTurn) {
			case PlayerStateHolder.PlayerEnum.FirstPlayer:
				Striker.transform.localPosition = new Vector3 (0f, 0f, (-1) * StrikerZeroPos);
				Striker.transform.localRotation = Quaternion.identity;
				break;
			case PlayerStateHolder.PlayerEnum.SecondPlayer:
				Striker.transform.localPosition = new Vector3 (0f, 0f, StrikerZeroPos);
				Striker.transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
				break;
			}
		}
		if (isClient) {
			yield return new WaitForSeconds (1f);
		}
		//CoroutineStarted false so that it can run again
		MainCoroutineStarted = false;
		yield return null;
	}

	bool EveryPieceAndStrikerStopped() {
		GameObject []WhitePieces = GameObject.FindGameObjectsWithTag ("WhitePiece");
		GameObject []BlackPieces = GameObject.FindGameObjectsWithTag ("BlackPiece");
		GameObject Queen = GameObject.FindGameObjectWithTag ("Queen");
		GameObject Striker = GameObject.FindGameObjectWithTag ("Striker");

		foreach (GameObject g in WhitePieces) {
			if (g.GetComponent<Rigidbody> ().velocity.magnitude > StopTolerance) {
				return false;
			}
		}

		foreach (GameObject g in BlackPieces) {
			if (g.GetComponent<Rigidbody> ().velocity.magnitude > StopTolerance) {
				return false;
			}
		}

		if (Queen.GetComponent<Rigidbody> ().velocity.magnitude > StopTolerance) {
			return false;
		}

		if (Striker.GetComponent<Rigidbody> ().velocity.magnitude > StopTolerance) {
			return false;
		}

		return true;
	}

	void OnTriggerEnter(Collider C) {
		if (C.tag.Equals ("WhitePiece") || C.tag.Equals ("BlackPiece") || C.tag.Equals ("Queen") || C.tag.Equals ("Striker")) {
			if (!C.tag.Equals ("Striker")) {
				StartCoroutine(MakeKinematic(C.gameObject.GetComponent<Rigidbody>()));
			}
			switch (PrevPlayerTurn) {
			case PlayerStateHolder.PlayerEnum.FirstPlayer:
				FirstPlayerTempPocketedArray.Add (C.gameObject);
				break;
			case PlayerStateHolder.PlayerEnum.SecondPlayer:
				SecondPlayerTempPocketedArray.Add (C.gameObject);
				break;
			}
		}
	}

	IEnumerator MakeKinematic(Rigidbody rb){
		yield return new WaitForSeconds (1f);
		rb.isKinematic = true;
	}

	public static void SaveScores(string PlayerName, int Scores){
		string past = PlayerPrefs.GetString ("Scores", "");
		if (past.Equals ("")) {
			PlayerPrefs.SetString ("Scores", PlayerName + ":" + Scores.ToString ());
		} else {
			PlayerPrefs.SetString ("Scores", "," + PlayerName + ":" + Scores.ToString ());
		}
		PlayerPrefs.Save ();
	}

	public static string GetAllScores(){
		return PlayerPrefs.GetString ("Scores", "");
	}

	public static int GetScores(string PlayerName){
		try{
			string Scores = GetAllScores ();
			string[] Score = Scores.Split (',');
			foreach (string s in Score) {
				string []part = s.Split(':');
				if(part[0].Equals(PlayerName)){
					return int.Parse(part[1]);
				}
			}
		}
		catch {
			return 0;
		}
		return 0;
	}
}
