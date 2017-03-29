using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BitBenderGames;

public class Pause : MonoBehaviour {

	public GameObject PauseButton, PauseBlock;
	private GameObject Character, Cam;
	private Vector3 currentSpeed;

	public void Start()
	{
		Cam = GameObject.FindGameObjectWithTag ("MainCamera");
		Character = GameObject.Find ("bajai_jadi_texturetest_animasi(Clone)");
		PauseBlock.SetActive (false);
		PauseButton.SetActive (true);
	}

	public void Update(){
		if (Character == null) {
			Character = GameObject.Find ("bajai_jadi_texturetest_animasi(Clone)");
		}
	}

	public void pause()
	{
		Cam.GetComponent<TouchInputController> ().enabled = false;
		Cam.GetComponent<MobileTouchCamera> ().enabled = false;
		Character.GetComponent<Animator> ().enabled = false;
		currentSpeed = Character.GetComponent<Rigidbody> ().velocity;
		TimerController.countdownStart = false;
		Character.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		GameController.gameModel.IsPaused = true;
		GameObject.Find ("GameplaySystem").GetComponent<GameController> ().isOnPopUp = true;
		PauseBlock.SetActive (true);
		PauseButton.SetActive (false);
	}

	public void resume()
	{
		Cam.GetComponent<TouchInputController> ().enabled = true;
		Cam.GetComponent<MobileTouchCamera> ().enabled = true;
		Character.GetComponent<Animator> ().enabled = true;
		Character.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		Character.GetComponent<Rigidbody> ().velocity = currentSpeed;
		GameController.gameModel.IsPaused = false;
		TimerController.countdownStart = true;
		GameObject.Find ("GameplaySystem").GetComponent<GameController> ().isOnPopUp = false;
		PauseBlock.SetActive (false);
		PauseButton.SetActive (true);
	}
}
