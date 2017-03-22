using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameModel gameModel = new GameModel();

	// Use this for initialization
	void Start () {
		gameModel.IsStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
