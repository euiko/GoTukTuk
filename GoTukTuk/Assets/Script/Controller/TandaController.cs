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
	//	transform.LookAt(new Vector3(180,180,-180));
		go = transform.parent.gameObject;
		transform.rotation = Quaternion.Euler (0, 180, 0);
		//if (go.transform.localRotation.eulerAngles.y == 270)
			
		//transform.Rotate (Vector3.up * (-go.transform.localRotation.eulerAngles.y), Space.Self);
		//transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, transform.localRotation.eulerAngles.y - go.transform.localRotation.eulerAngles.y, transform.rotation.z);
<<<<<<< HEAD
		startRot = transform.localRotation.eulerAngles;
		Debug.Log (go.name + " COK = " + go.transform.localRotation.eulerAngles);
=======
		startRot = transform.localEulerAngles;
//		Debug.Log (go.name + " COK = " + go.transform.localRotation.eulerAngles);
>>>>>>> 5c5348474b52422630cc9ee88c88f5de9a6bfa80
	}
	
	// Update is called once per frame
	void Update () {
		if (go.GetComponent<StreetProp> ().isOnAction) {
			if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.noCommand) == false) {
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
		if (go.GetComponent<StreetProp> ().isLeftFromRoad) {
			if (go.GetComponent<StreetProp> ().cmd ==  StreetProp.command.turnLeft) {
				mat = Resources.Load (materials[1,0], typeof(Material)) as Material;
				transform.GetComponent<Renderer> ().material = mat;
			} else if (go.GetComponent<StreetProp> ().cmd == StreetProp.command.turnRight) {
				mat = Resources.Load (materials[1,1], typeof(Material)) as Material;
				transform.GetComponent<Renderer> ().material = mat;
			}
			go.GetComponent<StreetProp> ().isLeftFromRoad = false;
		}
	}

	void changeMaterial(StreetProp.commandFrom cmdFrom, int angle){
		if (go.GetComponent<StreetProp> ().turnListener (cmdFrom, StreetProp.command.turnLeft)) {
			mat = Resources.Load (materials[0,0], typeof(Material)) as Material;
			transform.GetComponent<Renderer> ().material = mat;
			//Debug.Log ("kiri " + cmdFrom);
			Vector3 r = startRot;
			r.x = (startRot.y == 270 ? 180 : startRot.x) - (360 - angle);
			r.y = 90;
			r.z = 90;
			transform.localRotation = Quaternion.Euler(r);
		} else if (go.GetComponent<StreetProp> ().turnListener (cmdFrom, StreetProp.command.turnRight)) {
			mat = Resources.Load (materials[0,1], typeof(Material)) as Material;
			transform.GetComponent<Renderer> ().material = mat;
			//Debug.Log ("kanan - ");
			Vector3 r = startRot;
			r.x = (startRot.y == 270 ? 180 : startRot.x) - (360 - angle);
			r.y = 90;
			r.z = 90;
			Debug.Log (go.name + " KRIK = " + r + " ; SU = " + startRot);
			transform.localRotation = Quaternion.Euler(r);
		}
	}

}
