using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour {
	public void switchCam(){
		GameObject.Find ("GameplaySystem").GetComponent<GameController> ().switchCam ();
	}

	public void startGame(){
		GameController.gameModel.isStart = true;
		Pause.isOnAction = true;
	}
}
