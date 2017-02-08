using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	public GameObject PauseButton, PanelPause;

	public void Start()
	{
		PanelPause.SetActive (false);
		PauseButton.SetActive (true);
	}

	public void pause()
	{
		PanelPause.SetActive (true);
		PauseButton.SetActive (false);
		Time.timeScale = 0;
	}

	public void resume()
	{
		PanelPause.SetActive (false);
		PauseButton.SetActive (true);
		Time.timeScale = 1;
	}
}
