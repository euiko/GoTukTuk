using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour{

	public static bool isOnAction, afterTurn, afterJump, afterDelete;

	public Toggle tugel1, tugel2;
	public ButtonModel.type buttonType;

	void Start(){
		GetComponent<Toggle> ().onValueChanged.AddListener(onValueChange);
	}

	void Update(){
		if (isOnAction) {
			if (afterTurn) {
				GameObject.Find ("ButtonTurn").GetComponent<Toggle> ().isOn = false;
				afterTurn = false;
			} else if (afterJump) {
				GameObject.Find ("ButtonJump").GetComponent<Toggle> ().isOn = false;
				afterJump = false;
			}else if (afterDelete) {
				GameObject.Find ("ButtonDelete").GetComponent<Toggle> ().isOn = false;
				afterDelete = false;
			}
			isOnAction = false;
		}
	}

	void onValueChange(bool val){
		if (!GameController.gameModel.IsStarted) {
			if (buttonType == ButtonModel.type.direction && GameController.gameModel.directionCount > 0) {
				if (val) {
					tugel1.GetComponent<Toggle> ().isOn = false;
					tugel2.GetComponent<Toggle> ().isOn = false;
					ButtonProp.buttonModel.ButtonType = ButtonModel.type.direction;
					ButtonProp.buttonModel.setActive ();
				} else {
					ButtonProp.buttonModel.setInActive ();
				}
			}else if (buttonType == ButtonModel.type.jump && GameController.gameModel.jumpCount > 0) {
				if (val) {
					tugel1.GetComponent<Toggle> ().isOn = false;
					tugel2.GetComponent<Toggle> ().isOn = false;
					ButtonProp.buttonModel.ButtonType = ButtonModel.type.jump;
					ButtonProp.buttonModel.setActive ();
				} else {
					ButtonProp.buttonModel.setInActive ();
				}
			}else if (buttonType == ButtonModel.type.delete) {
				if (val) {
					tugel1.GetComponent<Toggle> ().isOn = false;
					tugel2.GetComponent<Toggle> ().isOn = false;
					ButtonProp.buttonModel.ButtonType = ButtonModel.type.delete;
					ButtonProp.buttonModel.setActive ();
				} else {
					ButtonProp.buttonModel.setInActive ();
				}
			}
		}

		//Debug.Log (GameController.gameModel.IsStarted);
	}

}
