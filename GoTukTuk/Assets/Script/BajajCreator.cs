using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajajCreator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject go = Resources.Load ("MadeUp/bajai_jadi_texturetest_animasi", typeof(GameObject)) as GameObject;
		if (transform.GetComponent<StreetProp>().streetType == StreetProp.type.start) {
			Vector3 v = transform.position;
			v.z -= 3;
			Instantiate(go, v, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
