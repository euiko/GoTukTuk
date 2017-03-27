using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public enum eType {normal, reverse};
	public enum eAxis {x, z};

	public eAxis axis;
	public eType type;

	private int rot;

	// Use this for initialization
	void Start () {
		Vector3 v = transform.position;
		Vector3 v2 = v;
		if (type == eType.normal) {
			if (axis == eAxis.x) {
				v.x -= 3;
				v2.x += 3;
				rot = 90;
			}else if(axis == eAxis.z){
				v.z -= 3;
				v2.z += 3;
				rot = 0;
			}
		}else if(type == eType.reverse){
			if (axis == eAxis.x) {
				v.x += 3;
				v2.x -= 3;
				rot = 270;
			}else if(axis == eAxis.z){
				v.z += 3;
				v2.z -= 3;
				rot = 180;
			}
		}

		if (GetComponent<StreetProp> ().streetType == StreetProp.type.start) {
			GameObject go = Resources.Load ("MadeUp/bajai_jadi_texturetest_animasi", typeof(GameObject)) as GameObject;
			go.GetComponentInChildren<Camera> ().enabled = false;
			GameObject startSign = Resources.Load ("MadeUp/TandaJalanStart", typeof(GameObject)) as GameObject;
			Instantiate (go, v, getRotation(Quaternion.identity));
			Instantiate (startSign, v2, getRotation(Quaternion.identity));
			GameObject.Find ("GameplaySystem").GetComponent<GameController> ().playerCam = GameObject.Find ("bajai_jadi_texturetest_animasi(Clone)").GetComponentInChildren<Camera> ();
		} else if (GetComponent<StreetProp> ().streetType == StreetProp.type.finish) {
			GameObject finishSign = Resources.Load ("MadeUp/TandaJalanFinish", typeof(GameObject)) as GameObject;
			Instantiate (finishSign, v2, getRotation(Quaternion.identity));
		}
	}

	private Quaternion getRotation(Quaternion currentRot){
		currentRot = Quaternion.Euler(new Vector3(currentRot.eulerAngles.x, rot, currentRot.eulerAngles.z));
		return currentRot;
	}

}
