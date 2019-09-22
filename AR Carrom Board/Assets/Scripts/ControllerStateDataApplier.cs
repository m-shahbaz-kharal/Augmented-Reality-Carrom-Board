using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class ControllerStateDataApplier : MonoBehaviour {
	public ControllerDataCollector ControllerDataCollectorInstance;
	public PlayerStateHolder.PlayerEnum PlayerType;
	private ControllerDataCollector.ControllerAimStateEnum Prev = ControllerDataCollector.ControllerAimStateEnum.Unknown;
	private ControllerDataCollector.ControllerAimStateEnum Curr = ControllerDataCollector.ControllerAimStateEnum.Unknown;

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
			if (Curr == ControllerDataCollector.ControllerAimStateEnum.Unknown) {
				HandAnimator.SetInteger ("ShootState", 0);
			} else if (Curr == ControllerDataCollector.ControllerAimStateEnum.Idle) {
				if (Prev == ControllerDataCollector.ControllerAimStateEnum.Aim) {
					HandAnimator.SetInteger ("ShootState", 3);
				} else {
					HandAnimator.SetInteger ("ShootState", 0);
				}
			} else if (Curr == ControllerDataCollector.ControllerAimStateEnum.PrepareAim) {
				HandAnimator.SetInteger ("ShootState", 1);
			} else {
				HandAnimator.SetInteger ("ShootState", 2);
			}
			Prev = Curr;
			break;
		case PlayerStateHolder.PlayerEnum.SecondPlayer:
			Curr = ControllerDataCollectorInstance.Player2ControllerState;
			if (Curr == ControllerDataCollector.ControllerAimStateEnum.Unknown) {
				HandAnimator.SetInteger ("ShootState", 0);
			} else if (Curr == ControllerDataCollector.ControllerAimStateEnum.Idle) {
				if (Prev == ControllerDataCollector.ControllerAimStateEnum.Aim) {
					HandAnimator.SetInteger ("ShootState", 3);
				} else {
					HandAnimator.SetInteger ("ShootState", 0);
				}
			} else if (Curr == ControllerDataCollector.ControllerAimStateEnum.PrepareAim) {
				HandAnimator.SetInteger ("ShootState", 1);
			} else {
				HandAnimator.SetInteger ("ShootState", 2);
			}
			Prev = Curr;
			break;
		}
	}
}
