using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour {

	public static bool collectingStar;

	public GameObject valueObject;

	// Use this for initialization
	void Start () {
		if (GameController.gameModel == null)
			GameController.gameModel = new GameModel ();
		valueObject.GetComponent<TMPro.TextMeshProUGUI> ().text = "" + GameController.gameModel.collectedStar;
	}
	
	// Update is called once per frame
	void Update () {
		if (collectingStar) {
			GameController.gameModel.collectedStar = GameController.gameModel.collectedStar + 1;
			valueObject.GetComponent<TMPro.TextMeshProUGUI> ().text = "" + GameController.gameModel.collectedStar;
			//Debug.Log ("Dapet Bintang = " + GameController.gameModel.collectedStar);
			collectingStar = false;
		}
	}
}
