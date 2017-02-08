using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreetController : MonoBehaviour {

	void OnMouseDown(){
		if (ButtonController.buttonModel.getIsActive ()) {
			if (ButtonController.buttonModel.buttonListener (ButtonModel.type.direction)) {
				GetComponent<StreetProp> ().cmd = StreetProp.command.turnRight;
				ButtonController.buttonModel.setInActive ();
			}
		} else {
			if (!(GetComponent<StreetProp> ().turnListener (StreetProp.command.noCommand))) {
				if (GetComponent<StreetProp> ().turnListener (StreetProp.command.turnLeft)) {
					GetComponent<StreetProp> ().cmd = StreetProp.command.turnRight;
				} else if (GetComponent<StreetProp> ().turnListener (StreetProp.command.turnRight)) {
					GetComponent<StreetProp> ().cmd = StreetProp.command.turnLeft;
				}
			}
		}
	}
}
