using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonProp : MonoBehaviour{

	public static bool _isOnAction, _onButtonDirection, _onButtonJump, _onButtonBrake, _updateCount;
	public static ButtonModel buttonModel = new ButtonModel();
	public GameObject buttonDirection, buttonJump, buttonBrake;

	void Start(){
		if (GameController.gameModel == null)
			GameController.gameModel = new GameModel ();
		
		GameController.gameModel.directionCount = GameObject.Find ("GameplaySystem").GetComponent<GameController> ().jumlahBelok;
		GameController.gameModel.jumpCount = GameObject.Find ("GameplaySystem").GetComponent<GameController> ().jumlahLompat;
		GameController.gameModel.brakeCount = GameObject.Find ("GameplaySystem").GetComponent<GameController> ().jumlahRem;

		setText (GameController.gameModel.directionCount, GameController.gameModel.jumpCount, GameController.gameModel.brakeCount);
	}

	void Update(){
		if (_isOnAction) {
			if (_onButtonDirection) {
				GameController.gameModel.directionCount = GameController.gameModel.directionCount - 1;
				setText (buttonDirection, GameController.gameModel.directionCount);
				_onButtonDirection = false;
			} else if (_onButtonJump) {
				GameController.gameModel.jumpCount = GameController.gameModel.jumpCount - 1;
				setText (buttonJump, GameController.gameModel.jumpCount);
				_onButtonJump = false;
			} else if (_onButtonBrake) {
				GameController.gameModel.brakeCount = GameController.gameModel.brakeCount - 1;
				setText (buttonBrake, GameController.gameModel.brakeCount);
				_onButtonBrake = false;
			} else if (_updateCount) {
				setText (GameController.gameModel.directionCount, GameController.gameModel.jumpCount, GameController.gameModel.brakeCount);
				_updateCount = false;
			}
			_isOnAction = false;
		}
	}

	void setText(int i1, int i2, int i3){
		buttonDirection.transform.FindChild ("count").FindChild ("value").GetComponent<TMPro.TextMeshProUGUI> ().text = "" + i1;
		buttonJump.transform.FindChild ("count").FindChild ("value").GetComponent<TMPro.TextMeshProUGUI> ().text = "" + i2;
		buttonBrake.transform.FindChild ("count").FindChild ("value").GetComponent<TMPro.TextMeshProUGUI> ().text = "" + i3;
	}

	void setText(GameObject go, int val){
		go.transform.FindChild ("count").FindChild ("value").GetComponent<TMPro.TextMeshProUGUI> ().text = "" + val;
	}
}
