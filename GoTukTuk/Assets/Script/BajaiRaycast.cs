using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajaiRaycast : MonoBehaviour {

	public float maxRayDistance = 25;
	public GameObject currentStreet;
	public GameObject nextStreet;

	Vector3[] direction = {Vector3.forward, Vector3.down};
	private Vector3 v;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		v = BajajController.playerDirection.getRaycastPosition(transform.position, 3);
		//v.z += 3;

		Vector3 v1 = v;

		v1.y += 3;

		float maxRay1Distance = 15;

		direction [0] = transform.TransformDirection (Vector3.forward);

		Debug.DrawLine (v, v + direction[0] *maxRay1Distance, Color.green);
		Debug.DrawLine (v, v1 + direction[1] *maxRayDistance, Color.red);

		Ray ray2 = new Ray (v1, direction[1]);
		RaycastHit hit2;
		if (Physics.Raycast(ray2, out hit2, maxRayDistance))
		{
			if (hit2.collider.gameObject.name.Contains ("jalan")) {
				GameObject go = hit2.collider.gameObject;
				currentStreet = hit2.collider.gameObject;
				Debug.Log ("ini Jalan");
				if (!go.GetComponent<StreetProp> ().isCommandExecuted) {
					Debug.Log (Mathf.Round (BajajController.playerDirection.getDirectionAxis (v1)) + " - " + Mathf.Round (BajajController.playerDirection.getDirectionAxis (go.transform.position)));
					if (Mathf.Round (BajajController.playerDirection.getDirectionAxis (v1)) + 3 == Mathf.Round (BajajController.playerDirection.getDirectionAxis (go.transform.position))) {
						Debug.Log ("position = true");
						if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.turnRight)) {
							BajajController.cmd [0] = true;
						} else if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.turnLeft)) {
							BajajController.cmd [1] = true;
						}
						go.GetComponent<StreetProp> ().isCommandExecuted = true;
					}
				}
			} else {
				currentStreet = null;
			}
		}

		Ray ray1 = new Ray (v, direction[0]);
		RaycastHit[] hits = Physics.RaycastAll(ray1, maxRay1Distance);
		if (hits.Length > 1) {
			if (Physics.Raycast (ray1, out hits [1], maxRay1Distance)) {
				if (hits [1].collider.gameObject.name.Contains ("jalan")) {
					//Debug.Log ("Hit 1 = " + hits [1].collider.gameObject.name);
					nextStreet = hits [1].collider.gameObject;
				} else {
					//Debug.Log ("Hit 1 = False");
					nextStreet = null;
				}
			}
		} else {
			nextStreet = null;
		}
	}
}
