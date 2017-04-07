using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreetController : MonoBehaviour {

	private Transform jump;
	private bool isOnAction, isJump;

	void Update(){
		if (isOnAction) {
			if (GetComponent<StreetProp> ().cmd == StreetProp.command.jump) {
				if (!isJump) {
					GameObject jumper = Resources.Load ("MadeUp/jumper", typeof(GameObject)) as GameObject;
					Vector3 pos = transform.position;
					pos.y -= 0.2f;
					jump = ((GameObject)Instantiate (jumper, pos, transform.rotation)).transform;
					jump.parent = transform;
					isJump = true;
					//Debug.Log(jump.parent.name + " += " + jump.rotation.eulerAngles);
				}
				jump.rotation = getJumpDirection ();
			} else if(GetComponent<StreetProp> ().cmd != StreetProp.command.jump && isJump) {
				Destroy (jump.gameObject);
				isJump = false;
			}
			isOnAction = false;
		}
	}

	Quaternion getJumpDirection(){
		Quaternion res;
		if (GetComponent<StreetProp> ().cmdFrom == StreetProp.commandFrom.down) {
			res = Quaternion.Euler(270, 0, 0);
		} else if (GetComponent<StreetProp> ().cmdFrom == StreetProp.commandFrom.left) {
			res = Quaternion.Euler(270, 0, 90);
		} else if (GetComponent<StreetProp> ().cmdFrom == StreetProp.commandFrom.up) {
			res = Quaternion.Euler(270, 0, 180);
		} else {
			res = Quaternion.Euler(270, 0, 270);
		}
		return res;
	}

	void OnMouseDown(){
		//Debug.Log (GetComponent<StreetProp>().getCurCommandFrom());
		GetComponent<StreetProp>().getCurCommandFrom(StreetProp.command.turnLeft);
		if (!GameController.gameModel.IsStarted && !GameController.gameModel.IsPaused) {
			if (ButtonProp.buttonModel.ButtonType == ButtonModel.type.direction) {


				if (ButtonProp.buttonModel.getIsActive ()) {
					ButtonProp._isOnAction = true;
					ButtonProp._onButtonDirection = true;
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
					delete ();
				}
			}else if(ButtonProp.buttonModel.ButtonType == ButtonModel.type.jump){
				if (ButtonProp.buttonModel.getIsActive ()) {
					ButtonProp._isOnAction = true;
					GetComponent<StreetProp> ().isOnAction = true;
					ButtonProp._onButtonJump = true;
					GetComponent<StreetProp> ().cmd = StreetProp.command.jump;
					ButtonProp.buttonModel.setInActive ();
				} else {
					GetComponent<StreetProp> ().getCurCommandFrom (StreetProp.command.jump);
				}
				isOnAction = true;
			}
		}
	}

	void delete(){
		if (GetComponent<StreetProp> ().cmd == StreetProp.command.turnLeft || GetComponent<StreetProp> ().cmd == StreetProp.command.turnRight) {
			GameController.gameModel.directionCount = GameController.gameModel.directionCount + 1;
		}else if (GetComponent<StreetProp> ().cmd == StreetProp.command.jump) {
			GameController.gameModel.jumpCount = GameController.gameModel.jumpCount + 1;
		}
		ButtonProp._isOnAction = true;
		ButtonProp._updateCount = true;
		isOnAction = true;
		GetComponent<StreetProp> ().cmd = StreetProp.command.noCommand;
		GetComponent<StreetProp> ().isOnAction = true;
		ButtonProp.buttonModel.setInActive ();
	}
}
