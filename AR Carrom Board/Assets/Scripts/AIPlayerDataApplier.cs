using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class AIPlayerDataApplier : MonoBehaviour {
	//Parent is necessary as the ShootScript is connected with parent
	private Transform Parent;

	//Anim Vars
	private Animator HandAnimator;

	private bool Play;

	private GameObject SuitablePiece;
	private Transform SuitablePocket;
	[Header ("Pocketed Pieces Identification")]
	//Minimum Y position for a piece to be suitable to strike.
	public float MinY = -5f;
	private Transform Pocket1, Pocket2, Pocket3, Pocket4;

	[Header ("Position Range for Placing Striker")]
	private const float SuitableXPosNotFound = -3500f;
	private float SuitableXPos = SuitableXPosNotFound;
	public float MinX = -0.3f, MaxX = 0.3f;
	public int MaxPositions = 5;
	public float XTranslationDelta = 0.1f;
	public float XTranslationTolerance = 0.1f;

	[Header ("Rotation Range for Aiming Striker")]
	private Quaternion SuitableRotation = Quaternion.identity;
	public float YRotationDelta = 0.1f;
	public float YRotationTolerance = 0.005f;
	public float HitPointDelta = 0.068f;

	[Header ("Speed Range for Shooting Striker")]
	public float SpeedMulAlpha = 1.5f;
	public float SpeedMulBeta = 1.5f;
	private float SuitableSpeed = SuitableSpeedNotFound;
	private const float SuitableSpeedNotFound = -3500f;
	public float SpeedIncreaseDelta = 1f;
	public float SpeedTolerance = 0.1f;

	void Start(){
		Parent = transform.parent;
		HandAnimator = GetComponent<Animator> ();
		Pocket1 = GameObject.FindGameObjectWithTag ("Pocket1").transform;
		Pocket2 = GameObject.FindGameObjectWithTag ("Pocket2").transform;
		Pocket3 = GameObject.FindGameObjectWithTag ("Pocket3").transform;
		Pocket4 = GameObject.FindGameObjectWithTag ("Pocket4").transform;
	}

	void Update(){
		if(GameStateHolder.PlayerTurn == PlayerStateHolder.PlayerEnum.SecondPlayer) {
			Play = true;
		} else {
			Play = false;
		}
		if (Play) {
			int CurrState = HandAnimator.GetInteger ("ShootState");
			switch (CurrState) {
			case 0: //Idle
				FindSuitablePiece ();
				FindSuitableStrikerXPos ();
				Parent.position = new Vector3 (Mathf.Lerp (Parent.position.x, SuitableXPos, XTranslationDelta), Parent.position.y, Parent.position.z);
				if (Mathf.Abs (Parent.position.x - SuitableXPos) < XTranslationTolerance) {
					HandAnimator.SetInteger ("ShootState", 1);
				}
				break;
			case 1: //Prepare Aim
				FindSuitableRotation ();
				Parent.localRotation = Quaternion.Slerp (Parent.localRotation, SuitableRotation, YRotationDelta);
				if (Mathf.Abs(Parent.localRotation.eulerAngles.y - SuitableRotation.eulerAngles.y) < YRotationTolerance) {
					HandAnimator.SetInteger ("ShootState", 2);
				}
				break;
			case 2: //Aim
				FindSuitableForce();
				if((GetComponent<PlayerShootScript>().ShootSpeed - SuitableSpeed) > SpeedTolerance) {
					HandAnimator.SetInteger ("ShootState", 3);
				}
				break;
			case 3: //Shoot
				//Handled by PlayerShootScript => Resetting ShootState of Animator
				break;
			}
		}
	}

	void FindSuitablePiece(){
		//If some suitable piece and pocket is already there no need to find. (Please make these pocket and piece null after every strike)
		if (SuitablePiece != null && SuitablePocket != null) {
			return;
		}
		//Get all pieces
		GameObject[] BlackPieces = GameObject.FindGameObjectsWithTag ("BlackPiece");
		for (int i = 0; i < BlackPieces.Length; i++) {
			GameObject Piece = BlackPieces [i];
			//if a piece is already pocketed just don't make it suitable piece to shoot.
			if (Piece.transform.position.y < MinY) {
				continue;
			}
			//finding if there is any piece which can pe put directly to the pocket. (i.e. no objects between the piece and pocket)
			//as pocket 3 and 4 are opposite check them first.
			if (!Physics.Raycast (Piece.transform.position, (Pocket3.position - Piece.transform.position))) {
				SuitablePiece = Piece;
				SuitablePocket = Pocket3;
				break;
			}
			if (!Physics.Raycast (Piece.transform.position, (Pocket4.position - Piece.transform.position))) {
				SuitablePiece = Piece;
				SuitablePocket = Pocket4;
				break;
			}
			if (!Physics.Raycast (Piece.transform.position, (Pocket1.position - Piece.transform.position))) {
				SuitablePiece = Piece;
				SuitablePocket = Pocket1;
				break;
			}
			if (!Physics.Raycast (Piece.transform.position, (Pocket2.position - Piece.transform.position))) {
				SuitablePiece = Piece;
				SuitablePocket = Pocket2;
				break;
			}


			//if none found suitable make the last piece and any random pocket suitable.
			SuitablePiece = Piece;
			int RandomVal = Random.Range (1, 5);
			switch (RandomVal) {
			case 1:
				SuitablePocket = Pocket1;
				break;
			case 2:
				SuitablePocket = Pocket2;
				break;
			case 3:
				SuitablePocket = Pocket3;
				break;
			case 4:
				SuitablePocket = Pocket4;
				break;
			}
		}
	}

	void FindSuitableStrikerXPos(){
		if (SuitableXPos != SuitableXPosNotFound) {
			return;
		}
		Transform Striker = GameObject.FindGameObjectWithTag ("Striker").transform;
		for (int i = 0; i < MaxPositions; i++) {
			float XPos = MinX + ((((MaxX - MinX)*1.0f) / MaxPositions) * (i + 1));
			if (!Physics.Raycast (new Vector3 (XPos, Striker.position.y, Striker.position.z), SuitablePiece.transform.position)) {
				SuitableXPos = XPos;
				Debug.Log ("XPOS: "+SuitableXPos);
				return;
			}
		}
		SuitableXPos = 0f;
		Debug.Log ("XPOS: "+SuitableXPos);
	}

	void FindSuitableRotation(){
		if (SuitableRotation != Quaternion.identity) {
			return;
		}
		Transform StrikerTransform = GameObject.FindGameObjectWithTag ("Striker").transform;
		SuitablePiece.transform.LookAt(SuitablePocket.position);
		GameObject AimPoint = new GameObject ();
		AimPoint.transform.SetPositionAndRotation (SuitablePiece.transform.position, SuitablePiece.transform.localRotation);
		AimPoint.transform.position += (-AimPoint.transform.forward) * (HitPointDelta);
		GameObject StrikerCopy = new GameObject ();
		StrikerCopy.transform.SetPositionAndRotation (StrikerTransform.position, StrikerTransform.rotation);
		StrikerCopy.transform.LookAt (AimPoint.transform.position);
		SuitableRotation = StrikerCopy.transform.localRotation;
		GameObject.Destroy (StrikerCopy);
		GameObject.Destroy (AimPoint);
	}

	void FindSuitableForce(){
		if (SuitableSpeed != SuitableSpeedNotFound) {
			return;
		}
		SuitableSpeed = (Vector3.Distance (SuitablePocket.position, SuitablePiece.transform.position) * SpeedMulAlpha) + (Vector3.Distance (SuitablePiece.transform.position, GameObject.FindGameObjectWithTag ("Striker").transform.position) * SpeedMulBeta);
	}
}
