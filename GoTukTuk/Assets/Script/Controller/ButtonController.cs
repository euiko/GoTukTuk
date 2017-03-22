using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour{

	public static ButtonModel buttonModel = new ButtonModel();

	public ButtonModel.type buttonType;

	void Start(){
		GetComponent<Button> ().onClick.AddListener (onClick);
	}

	void onClick(){
		buttonModel.setButtonType(this.buttonType);
		buttonModel.setActive();
	}

}
