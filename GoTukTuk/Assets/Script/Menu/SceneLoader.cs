using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public SceneAsset scene;

	void Start () {
		GetComponent<Button>().onClick.AddListener(onClick);
	}

	void onClick()
	{
		changeSceneTo (scene);
	}

	public void changeSceneTo(SceneAsset sceneAsset){
		SceneManager.LoadScene (sceneAsset.name);
	}
}
