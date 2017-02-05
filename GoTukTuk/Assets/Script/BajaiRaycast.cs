using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajaiRaycast : MonoBehaviour {

	public float maxRayDistance = 25;
	Vector3[] direction = {Vector3.forward, Vector3.down};
	private Vector3 v;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		v = transform.position;
		v.z += 4;

		direction [0] = transform.TransformDirection (Vector3.forward);

		Debug.DrawLine (v, v + direction[0] *maxRayDistance, Color.green);
		Debug.DrawLine (v, v + direction[1] *maxRayDistance, Color.red);

		Ray ray1 = new Ray (v, direction[0]);
		RaycastHit hit1;
		if (Physics.Raycast(ray1, out hit1, maxRayDistance))
		{
			if (hit1.collider.gameObject.name.Contains("jalan")){
				Debug.Log("HIT1 - " + hit1.collider.gameObject.name );
			}
		}

		Ray ray2 = new Ray (v, direction[1]);
		RaycastHit hit2;
		if (Physics.Raycast(ray2, out hit2, maxRayDistance))
		{
			if (hit2.collider.gameObject.name.Contains("jalan")){
				Debug.Log("HIT2 - " + hit2.collider.gameObject.name );
			}
		}
	}
}
