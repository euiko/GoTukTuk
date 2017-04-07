using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlayController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (onClick);
	}

	void onClick(){
		GameController.gameModel.IsStarted = true;
		TimerController.countdownStart = false;
		BajajController.onAction = true;
		//GameObject.Find ("GameplaySystem").GetComponent<GameController> ().switchCam ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
