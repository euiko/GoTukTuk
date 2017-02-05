using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajaiRaycast : MonoBehaviour {

	public float maxRayDistance = 25;
	Vector3[] direction = {Vector3.forward, Vector3.down};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = transform.position;
		v.y -= 2.4f;
		direction [0] = transform.TransformDirection (Vector3.forward);

		Debug.DrawLine (v, v + direction[0] *maxRayDistance, Color.green);

		Ray ray = new Ray (v, direction[0]);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, maxRayDistance))
		{
			if (hit.collider.gameObject.name.Contains("jalan")){
				Debug.Log("HIT - " + hit.collider.gameObject.name );
			}
		}
	}
}
