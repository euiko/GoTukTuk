using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour {

	public GameObject minutesText, secondsText;

	private static bool _countdownStart;
	private static bool _countAction;
	private int lastTime;

	void Start (){
		GameController.gameModel.duration = GameObject.Find("GameplaySystem").GetComponent<GameController> ().timeInSecond;
		displayTime ();
		countdownStart = true;
	}

	// Use this for initialization
	public static bool countdownStart{
		get { return _countdownStart;}
		set { _countAction = value; _countdownStart = value; }
	}
	
	// Update is called once per frame
	void Update () {
		if (_countdownStart && GameController.gameModel.currentTime > 0) {
			if (_countAction) {
				lastTime = Mathf.RoundToInt (Time.time);
				_countAction = false;
			}
			if (Mathf.RoundToInt (Time.time) - lastTime == 1) {
				GameController.gameModel.currentTime = GameController.gameModel.currentTime - 1;
				lastTime = Mathf.RoundToInt (Time.time);
				displayTime ();
				if (GameController.gameModel.currentTime == 0) {
					GameController.gameModel.isAction = true;
					GameController.gameModel.IsGameOver = true;
				}
			}
		}
	}

	void displayTime() {
		minutesText.GetComponent<TMPro.TextMeshProUGUI> ().text = (GameController.gameModel.currentTimeMinutes < 10 ? "0" : "") + GameController.gameModel.currentTimeMinutes;
		secondsText.GetComponent<TMPro.TextMeshProUGUI> ().text = (GameController.gameModel.currentTimeSecond < 10 ? "0" : "") + GameController.gameModel.currentTimeSecond;
	}
}
