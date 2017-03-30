using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject sceneScopePause, sceneScopeWin, sceneScopeLose;
	public static GameModel gameModel = new GameModel();
	public bool isOnPopUp;
	public Camera mainCam, playerCam;
	public int timeInSecond;


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
				//switchCam ();
				sceneScopeWin.GetComponent<FlipWebApps.BeautifulTransitions._Demo.Transitions.Scripts.TestController> ().TransitionIn();
				Transform popUpWin = sceneScopeWin.GetComponent<FlipWebApps.BeautifulTransitions._Demo.Transitions.Scripts.TestController> ().TransitionFromButtons.transform.FindChild("Panel");
				Transform sisaWaktu = popUpWin.Find ("Text").Find("sisaWaktu");
				sisaWaktu.Find("value").GetComponent<TMPro.TextMeshProUGUI> ().text = GameController.gameModel.currentTimeMinutes + " : " + GameController.gameModel.currentTimeSecond;
				Transform star = popUpWin.transform.FindChild ("starRes");
				List<Transform> childs = new List<Transform> ();
				foreach (Transform child in star) {
					childs.Add(child);
				}
				for (int i = 0; i < GameController.gameModel.collectedStar; i++) {
					childs[i].transform.FindChild ("value").gameObject.SetActive (true);
				}
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
			this.mainCam.GetComponent<AudioListener> ().enabled = false;
			this.playerCam.enabled = true;
			this.playerCam.GetComponent<AudioListener> ().enabled = true;
		} else {
			this.mainCam.enabled = true;
			this.mainCam.GetComponent<AudioListener> ().enabled = true;
			this.playerCam.enabled = false;
			this.playerCam.GetComponent<AudioListener> ().enabled = false;
		}
	}
}
