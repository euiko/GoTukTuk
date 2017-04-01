using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour {

	public GameObject minutesText, secondsText, divider;

	private static bool _countdownStart;
	private static bool _countAction;
	private int lastTime;
	private double lastTime2;
	private bool isSetDanger;

	void Start (){
		if (GameController.gameModel == null)
			GameController.gameModel = new GameModel ();
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
				lastTime2 = System.Math.Round (Time.time, 1);
				divider.SetActive (false);
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

				if (!isSetDanger && GameController.gameModel.currentTime < 10) {
					changeColorTo ("#E54848FF");
					isSetDanger = true;
				}
			}

			if (System.Math.Round (Time.time, 1) - lastTime2 == 0.5) {
				if (divider.gameObject.activeSelf) {
					divider.gameObject.SetActive (false);
				} else {
					divider.gameObject.SetActive (true);
				}

				lastTime2 = System.Math.Round (Time.time, 1);
			}
		}
	}

	void changeColorTo(string hex){
		Color color = new Color ();
		ColorUtility.TryParseHtmlString (hex, out color);
		minutesText.GetComponent<TMPro.TextMeshProUGUI> ().color = color;
		secondsText.GetComponent<TMPro.TextMeshProUGUI> ().color = color;
		divider.GetComponent<TMPro.TextMeshProUGUI> ().color = color;
	}

	void displayTime() {
		minutesText.GetComponent<TMPro.TextMeshProUGUI> ().text = (GameController.gameModel.currentTimeMinutes < 10 ? "0" : "") + GameController.gameModel.currentTimeMinutes;
		secondsText.GetComponent<TMPro.TextMeshProUGUI> ().text = (GameController.gameModel.currentTimeSecond < 10 ? "0" : "") + GameController.gameModel.currentTimeSecond;
	}
}
