using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajajCreator : MonoBehaviour {

	public GameObject gameObject;

	// Use this for initialization
	void Start () {
		Vector3 v = transform.position;
		v.y -= 2;
		v.z -= 6;
		Instantiate(gameObject, v, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
