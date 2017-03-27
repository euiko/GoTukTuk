using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject sceneScopePause, sceneScopeWin, sceneScopeLose;
	public static GameModel gameModel = new GameModel();
	public bool isOnPopUp;
	public Camera mainCam, playerCam;

	// Use this for initialization
	void Start () {
		mainCam.enabled = true;
		gameModel.IsStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameModel.isAction) {
			if (gameModel.IsFinished && !isOnPopUp) {
				isOnPopUp = true;
				switchCam ();
				sceneScopeWin.GetComponent<FlipWebApps.BeautifulTransitions._Demo.Transitions.Scripts.TestController> ().TransitionIn();
			} else if (gameModel.IsGameOver && !isOnPopUp) {
				isOnPopUp = true;
				sceneScopeLose.GetComponent<FlipWebApps.BeautifulTransitions._Demo.Transitions.Scripts.TestController> ().TransitionIn();
			}
			gameModel.isAction = false;
		}
	}

	public void switchCam(){
		if (this.mainCam.enabled) {
			this.mainCam.enabled = false;
			this.playerCam.enabled = true;
		} else {
			this.mainCam.enabled = true;
			this.playerCam.enabled = false;
		}
	}
}
