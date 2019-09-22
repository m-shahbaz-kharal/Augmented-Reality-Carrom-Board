using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScores : MonoBehaviour {
	public GameObject TurnAndScoreScript;
	public Text FirstPlayerScoreText, SecondPlayerScoreText, QueenText;
	private PlayerStateHolder.PlayerEnum QueenPocketedBy, QueenCoveredBy;

	public Animator NotificationUI;
	private Text NotificationIcon, NotificationText;

	void Start(){
		NotificationIcon = NotificationUI.transform.GetChild (0).GetChild (0).GetChild (0).GetChild (0).GetComponent<Text> ();
		NotificationText = NotificationUI.transform.GetChild (0).GetChild (0).GetChild (0).GetChild (1).GetComponent<Text> ();
	}
	void Update () {
		switch (GameStateHolder.GameMode) {
		case GameStateHolder.GameModesEnum.SinglePlayer:
			FirstPlayerScoreText.text = TurnAndScoreScript.GetComponent<TurnAndScore> ().GetScoreFirstPlayer ().ToString ();
			SecondPlayerScoreText.text = TurnAndScoreScript.GetComponent<TurnAndScore> ().GetScoreSecondPlayer ().ToString ();
			QueenPocketedBy = TurnAndScoreScript.GetComponent<TurnAndScore> ().GetQueenPocketed ();
			QueenCoveredBy = TurnAndScoreScript.GetComponent<TurnAndScore> ().GetQueenCovered ();
			break;
		case GameStateHolder.GameModesEnum.MultiPlayer:
			FirstPlayerScoreText.text = TurnAndScoreScript.GetComponent<MPTurnAndScore> ().GetScoreFirstPlayer ().ToString ();
			SecondPlayerScoreText.text = TurnAndScoreScript.GetComponent<MPTurnAndScore> ().GetScoreSecondPlayer ().ToString ();
			QueenPocketedBy = TurnAndScoreScript.GetComponent<MPTurnAndScore> ().GetQueenPocketed ();
			QueenCoveredBy = TurnAndScoreScript.GetComponent<MPTurnAndScore> ().GetQueenCovered ();
			break;
		}

		if (QueenPocketedBy != PlayerStateHolder.PlayerEnum.Unknown && QueenCoveredBy == PlayerStateHolder.PlayerEnum.Unknown) {
			QueenText.text = QueenPocketedBy.ToString () + " (Pocketed)";
		} else if (QueenPocketedBy != PlayerStateHolder.PlayerEnum.Unknown && QueenCoveredBy != PlayerStateHolder.PlayerEnum.Unknown) {
			QueenText.text = QueenPocketedBy.ToString () + " (Pocketed + Covered)";
		} else {
			QueenText.text = "Not pocketed yet.";
		}
	}

	public enum NotificationTypeEnum { Score, Foul, Info }

	public void ShowNotification(NotificationTypeEnum NotificationType, string text){
		NotificationText.text = text;
		switch (NotificationType) {
		case NotificationTypeEnum.Score:
			NotificationIcon.text = "+";
			NotificationIcon.color = Color.green;
			break;
		case NotificationTypeEnum.Foul:
			NotificationIcon.text = "-";
			NotificationIcon.color = Color.red;
			break;
		case NotificationTypeEnum.Info:
			NotificationIcon.text = "i";
			NotificationIcon.color = Color.yellow;
			break;
		}
		NotificationUI.SetBool ("Show", true);
		StartCoroutine (NotificationUIBoolFalsing ());
	}

	IEnumerator NotificationUIBoolFalsing(){
		yield return new WaitForSeconds (1f);
		NotificationUI.SetBool ("Show", false);
	}
}
