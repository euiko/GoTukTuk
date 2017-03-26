using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreetController : MonoBehaviour {


	void OnMouseDown(){
		//Debug.Log (GetComponent<StreetProp>().getCurCommandFrom());
		GetComponent<StreetProp>().getCurCommandFrom();
		if (!GameController.gameModel.IsStarted) {
			if (ButtonProp.buttonModel.ButtonType == ButtonModel.type.direction) {
				if (ButtonProp.buttonModel.getIsActive ()) {
					GetComponent<StreetProp> ().cmd = StreetProp.command.turnRight;
					GetComponent<StreetProp> ().isOnAction = true;
					ButtonProp.buttonModel.setInActive ();
				} else {
					if (!(GetComponent<StreetProp> ().turnListener (StreetProp.command.noCommand))) {
						if (GetComponent<StreetProp> ().turnListener (StreetProp.command.turnLeft)) {
							GetComponent<StreetProp> ().cmd = StreetProp.command.turnRight;
							GetComponent<StreetProp> ().isOnAction = true;
						} else if (GetComponent<StreetProp> ().turnListener (StreetProp.command.turnRight)) {
							GetComponent<StreetProp> ().cmd = StreetProp.command.turnLeft;
							GetComponent<StreetProp> ().isOnAction = true;
						}
					}
				}
			}else if(ButtonProp.buttonModel.ButtonType == ButtonModel.type.delete){
				if (ButtonProp.buttonModel.getIsActive ()) {
					GetComponent<StreetProp> ().cmd = StreetProp.command.noCommand;
					GetComponent<StreetProp> ().isOnAction = true;
					ButtonProp.buttonModel.setInActive ();
				}
			}
		}
	}
}
