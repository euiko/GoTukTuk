using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonBackToMainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(onClick);
    }

    void onClick()
    {
        Debug.Log("clicked");
        SceneManager.LoadScene("HomeScreen1");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
