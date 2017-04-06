using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour{


	public ButtonModel.type buttonType;

	void Start(){
		GetComponent<Button> ().onClick.AddListener (onClick);
	}

	void onClick(){
		if (!GameController.gameModel.IsStarted) {
			if (buttonType == ButtonModel.type.direction && GameController.gameModel.directionCount > 0) {
				ButtonProp.buttonModel.ButtonType = ButtonModel.type.direction;
				ButtonProp.buttonModel.setActive ();
			}else if (buttonType == ButtonModel.type.jump && GameController.gameModel.jumpCount > 0) {
				ButtonProp.buttonModel.ButtonType = ButtonModel.type.jump;
				ButtonProp.buttonModel.setActive ();
			}else if (buttonType == ButtonModel.type.brake && GameController.gameModel.brakeCount > 0) {
				ButtonProp.buttonModel.ButtonType = ButtonModel.type.brake;
				ButtonProp.buttonModel.setActive ();
			}else if (buttonType == ButtonModel.type.delete) {
				ButtonProp.buttonModel.ButtonType = ButtonModel.type.delete;
				ButtonProp.buttonModel.setActive ();
			}
		}

		//Debug.Log (GameController.gameModel.IsStarted);
	}

}
