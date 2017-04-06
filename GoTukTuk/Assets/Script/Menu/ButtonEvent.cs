using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour {
	public void switchCam(){
		GameObject.Find ("GameplaySystem").GetComponent<GameController> ().switchCam ();
	}
}
