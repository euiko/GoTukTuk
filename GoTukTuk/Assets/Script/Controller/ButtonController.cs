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
		if (GameController.gameModel.IsStarted == false) {
			ButtonProp.buttonModel.ButtonType = this.buttonType;
			ButtonProp.buttonModel.setActive ();
		}

		Debug.Log (GameController.gameModel.IsStarted);
	}

}
