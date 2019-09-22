using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class MPConrollerStateApplier : MonoBehaviour {
	public MPControllerDataCollector ControllerDataCollectorInstance;
	public PlayerStateHolder.PlayerEnum PlayerType;
	private MPControllerDataCollector.ControllerAimStateEnum Prev = MPControllerDataCollector.ControllerAimStateEnum.Unknown;
	private MPControllerDataCollector.ControllerAimStateEnum Curr = MPControllerDataCollector.ControllerAimStateEnum.Unknown;

	private Animator HandAnimator;

	void Start() {
		HandAnimator = GetComponent<Animator> ();
	}

	void Update() {
		if (ControllerDataCollectorInstance == null) {
			return;
		}

		switch (PlayerType) {
		case PlayerStateHolder.PlayerEnum.FirstPlayer:
			Curr = ControllerDataCollectorInstance.Player1ControllerState;
			if (Curr == MPControllerDataCollector.ControllerAimStateEnum.Unknown) {
				HandAnimator.SetInteger ("ShootState", 0);
			} else if (Curr == MPControllerDataCollector.ControllerAimStateEnum.Idle) {
				if (Prev == MPControllerDataCollector.ControllerAimStateEnum.Aim) {
					HandAnimator.SetInteger ("ShootState", 3);
				} else {
					HandAnimator.SetInteger ("ShootState", 0);
				}
			} else if (Curr == MPControllerDataCollector.ControllerAimStateEnum.PrepareAim) {
				HandAnimator.SetInteger ("ShootState", 1);
			} else {
				HandAnimator.SetInteger ("ShootState", 2);
			}
			Prev = Curr;
			break;
		case PlayerStateHolder.PlayerEnum.SecondPlayer:
			Curr = ControllerDataCollectorInstance.Player2ControllerState;
			if (Curr == MPControllerDataCollector.ControllerAimStateEnum.Unknown) {
				HandAnimator.SetInteger ("ShootState", 0);
			} else if (Curr == MPControllerDataCollector.ControllerAimStateEnum.Idle) {
				if (Prev == MPControllerDataCollector.ControllerAimStateEnum.Aim) {
					HandAnimator.SetInteger ("ShootState", 3);
				} else {
					HandAnimator.SetInteger ("ShootState", 0);
				}
			} else if (Curr == MPControllerDataCollector.ControllerAimStateEnum.PrepareAim) {
				HandAnimator.SetInteger ("ShootState", 1);
			} else {
				HandAnimator.SetInteger ("ShootState", 2);
			}
			Prev = Curr;
			break;
		}
	}
}
