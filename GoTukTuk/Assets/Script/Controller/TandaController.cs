using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TandaController : MonoBehaviour {

	GameObject go;
	Material mat;

	// Use this for initialization
	void Start () {
		go = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.noCommand) == false) {
			if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.turnLeft)) {
				mat = Resources.Load ("Materials/tanda/tandaBelokKiri", typeof(Material)) as Material;
				transform.GetComponent<Renderer> ().material = mat;
			} else if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.turnRight)) {
				mat = Resources.Load ("Materials/tanda/tandaBelokKanan", typeof(Material)) as Material;
				transform.GetComponent<Renderer> ().material = mat;
			}
		} else {
			mat = Resources.Load ("Materials/tanda/noCmd", typeof(Material)) as Material;
			transform.GetComponent<Renderer> ().material = mat;
		}
	}
}
