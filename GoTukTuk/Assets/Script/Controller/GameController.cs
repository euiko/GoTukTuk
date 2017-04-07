using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public enum EStage {one = 1, two = 2 , three = 3, four = 4};
	public enum ELevel {one = 1, two = 2, three = 3, four = 4, five = 5, six = 6, seven = 7, eight = 8};

	public GameObject sceneScopePause, sceneScopeWin, sceneScopeLose;
	public static GameModel gameModel;
	public bool isOnPopUp;
	public Camera mainCam, playerCam;
	public int timeInSecond;
	public EStage stage;
	public ELevel level;
	public int jumlahBelok, jumlahLompat, jumlahRem;

	// Use this for initialization
	void Start () {
		if (gameModel == null)
			gameModel = new GameModel ();
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
				sisaWaktu.Find("value").GetComponent<TMPro.TextMeshProUGUI> ().text = (GameController.gameModel.currentTimeMinutes < 10 ? "0" + GameController.gameModel.currentTimeMinutes : "" + GameController.gameModel.currentTimeMinutes) + " : " + (GameController.gameModel.currentTimeSecond < 10 ? "0" + GameController.gameModel.currentTimeSecond : "" + GameController.gameModel.currentTimeSecond );
				Transform star = popUpWin.transform.FindChild ("starRes");
				List<Transform> childs = new List<Transform> ();
				foreach (Transform child in star) {
					childs.Add(child);
				}
				for (int i = 0; i < GameController.gameModel.collectedStar; i++) {
					childs[i].transform.FindChild ("value").gameObject.SetActive (true);
				}
				saveGame ();
			} else if (gameModel.IsGameOver && !isOnPopUp) {
				isOnPopUp = true;
				sceneScopeLose.GetComponent<FlipWebApps.BeautifulTransitions._Demo.Transitions.Scripts.TestController> ().TransitionIn();
			}
			gameModel.isAction = false;
		}
	}

	public void saveGame(){
		string saveName = "stage" + stage;
		bool willSave = false;
		Dictionary<string, Level> stageData;
		if (ES2.Exists ("game.dat?tag=" + saveName + "&encrypt=true&password=gotuktukbahagia")) {
			stageData = ES2.LoadDictionary<string, Level> ("game.dat?tag=" + saveName + "&encrypt=true&password=gotuktukbahagia");
			if (stageData.ContainsKey ("" + level)) {
				if (gameModel.currentTime <= stageData ["" + level].time && gameModel.collectedStar >= stageData ["" + level].star)
					willSave = true;
			} else {
				willSave = true;
			}
		} else {
			stageData = new Dictionary<string, Level> ();
			willSave = true;
		}

		if (willSave) {
			stageData ["" + level] = Level.Save (true, gameModel.currentTime, gameModel.collectedStar);
			ES2.Save (stageData, "game.dat?tag=" + saveName + "&encrypt=true&password=gotuktukbahagia");
		}
		/* Load data with encryption */
		//int i = ES2.Load<int>("file.es?encrypt=true&password=pass");
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
