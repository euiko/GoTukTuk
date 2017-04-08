using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectController : MonoBehaviour {

	public GameObject StageJakarta, StageSurabaya, StageBandung, StageJogja;

	// Use this for initialization
	void Start () {
		if (ES2.Exists ("game.dat?tag=stageone&encrypt=true&password=gotuktukbahagia")) {
			StageJakarta.transform.FindChild("JakartaStage").GetComponent<Image> ().color = Color.white;
			StageJakarta.transform.FindChild ("lock").gameObject.SetActive (false);
		}
	}
	
}
