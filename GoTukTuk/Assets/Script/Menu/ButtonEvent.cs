using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour {
	public void switchCam(){
		GameObject.Find ("GameplaySystem").GetComponent<GameController> ().switchCam ();
	}

	public void startGame(){
		GameController.gameModel.isStart = true;
		Pause.isOnAction = true;
	}

	public void exitGame(){
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

}
