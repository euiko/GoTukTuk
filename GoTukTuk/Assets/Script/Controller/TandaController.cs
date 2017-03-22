using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TandaController : MonoBehaviour {

	GameObject go;
	Material mat;
	Vector3 startRot;

	// Use this for initialization
	void Start () {
		go = transform.parent.gameObject;
		startRot = transform.localRotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.noCommand) == false) {
//			if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.turnLeft)) {
//				mat = Resources.Load ("Materials/tanda/tandaBelokKiri", typeof(Material)) as Material;
//				transform.GetComponent<Renderer> ().material = mat;
//			} else if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.turnRight)) {
//				mat = Resources.Load ("Materials/tanda/tandaBelokKanan", typeof(Material)) as Material;
//				transform.GetComponent<Renderer> ().material = mat;
//			}

			changeMaterial (StreetProp.commandFrom.down, 0);
			changeMaterial (StreetProp.commandFrom.left, 270);
			changeMaterial (StreetProp.commandFrom.up, 180);
			changeMaterial (StreetProp.commandFrom.right, 90);
		} else {
			mat = Resources.Load ("Materials/tanda/noCmd", typeof(Material)) as Material;
			transform.GetComponent<Renderer> ().material = mat;
		}
	}

	void changeMaterial(StreetProp.commandFrom cmdFrom, int angle){
		if (go.GetComponent<StreetProp> ().turnListener (cmdFrom, StreetProp.command.turnLeft)) {
			mat = Resources.Load ("Materials/tanda/tandaBelokKiri", typeof(Material)) as Material;
			transform.GetComponent<Renderer> ().material = mat;
		
			Debug.Log ("kiri " + cmdFrom);
//			transform.localRotation.eulerAngles.x = startRot + angle;
			Vector3 r = startRot;
			if (go.name.Contains("PertigaanKanan")) {
				r.x = startRot.x - (360 - angle);
				r.y = 90;
				r.z = 90;
				Debug.Log ("zzzzz");
			}
			else if (startRot.x == 90) {
				r.x = startRot.x - (360 - angle);
				r.y = 90;
				r.z = 90;
			} else {
				r.x = startRot.x - angle;
			}
			transform.localRotation = Quaternion.Euler(r);
		} else if (go.GetComponent<StreetProp> ().turnListener (cmdFrom, StreetProp.command.turnRight)) {
			mat = Resources.Load ("Materials/tanda/tandaBelokKanan", typeof(Material)) as Material;
			transform.GetComponent<Renderer> ().material = mat;
			Debug.Log ("kanan - ");
//			transform.localRotation.eulerAngles.x = startRot + angle;

			Vector3 r = startRot;
			if (go.name.Contains("PertigaanKanan")) {
				r.x = startRot.x - (360 - angle);
				r.y = 90;
				r.z = 90;
			}
			else if (startRot.x == 90) {
				r.x = startRot.x - (360 - angle);
				r.y = 90;
				r.z = 90;
			} else {
				r.x = startRot.x - angle;
			}
			transform.localRotation = Quaternion.Euler(r);
		}
	}

}
