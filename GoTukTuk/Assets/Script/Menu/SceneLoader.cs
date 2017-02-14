using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public string sceneName;

	void Start () {
		GetComponent<Button>().onClick.AddListener(onClick);
	}

	void onClick()
	{
		changeSceneTo (sceneName);
	}

	public void changeSceneTo(string sceneName){
		SceneManager.LoadScene (sceneName);
	}
}
