using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TandaController : MonoBehaviour {

	GameObject go;
	Material mat;
	Vector3 startRot;
	private string[,] materials = new string[2,2]{{"Materials/tanda/tandaBelokKiri", "Materials/tanda/tandaBelokKanan"}, {"Materials/tanda/tandaOffBelokKiri", "Materials/tanda/tandaOffBelokKanan"}};

	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
		go = transform.parent.gameObject;
		startRot = transform.localRotation.eulerAngles;
		//Debug.Log (go.name + " COK = " + transform.rotation.eulerAngles);
	}
	
	// Update is called once per frame
	void Update () {
		if (go.GetComponent<StreetProp> ().isOnAction) {
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
			go.GetComponent<StreetProp> ().isOnAction = false;
		}
		if (go.GetComponent<StreetProp> ().isLeft) {

			go.GetComponent<StreetProp> ().isLeft = false;
		}
	}

	void changeMaterial(StreetProp.commandFrom cmdFrom, int angle){
		if (go.GetComponent<StreetProp> ().turnListener (cmdFrom, StreetProp.command.turnLeft)) {
			mat = Resources.Load (materials[0,0], typeof(Material)) as Material;
			transform.GetComponent<Renderer> ().material = mat;
		
			Debug.Log ("kiri " + cmdFrom);
//			transform.localRotation.eulerAngles.x = startRot + angle;
			Vector3 r = startRot;
			//if (go.name.Contains("PertigaanKanan")) {
			//	r.x = startRot.x - (360 - angle);
			//	r.y = 90;
			//	r.z = 90;
			//}
			//else if (startRot.x == 90) {
				r.x = startRot.x - (360 - angle);
				r.y = 90;
				r.z = 90;
			//} else {
			//	r.x = startRot.x - angle;
			//}
			transform.localRotation = Quaternion.Euler(r);
		} else if (go.GetComponent<StreetProp> ().turnListener (cmdFrom, StreetProp.command.turnRight)) {
			mat = Resources.Load (materials[0,1], typeof(Material)) as Material;
			transform.GetComponent<Renderer> ().material = mat;
			Debug.Log ("kanan - ");
//			transform.localRotation.eulerAngles.x = startRot + angle;

			Vector3 r = startRot;
			//if (go.name.Contains("PertigaanKanan")) {
			//	r.x = startRot.x - (360 - angle);
			//	r.y = 90;
			//	r.z = 90;
			//}
			//else if (startRot.x == 90) {
				r.x = startRot.x - (360 - angle);
				r.y = 90;
				r.z = 90;
			//} else {
			//	r.x = startRot.x - angle;
			//}
			transform.localRotation = Quaternion.Euler(r);
		}
	}

}
