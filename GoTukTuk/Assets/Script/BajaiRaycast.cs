using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajaiRaycast : MonoBehaviour {

	public float maxRayDistance = 25;
	public int minVal = 2;
	public GameObject currentStreet;
	public GameObject nextStreet;

	private Vector3[] direction = {Vector3.forward, Vector3.down};
	private Vector3 v;
	private bool willExecuteCurrentCommand;
	private GameObject beforeCurrentStreet = null;

	// Use this for initialization
	void Start () {
		willExecuteCurrentCommand = false;
	}
	
	// Update is called once per frame
	void Update () {
		v = BajajController.playerDirection.getRaycastPosition (transform.position, 3);
		//v.z += 3;

		Vector3 v1 = v;

		v.y -= 0.25f;
		v1.y += 3;

		int maxRay1Distance = 15;

		direction [0] = transform.TransformDirection (Vector3.forward);

		Debug.DrawLine (v, v + direction [0] * maxRay1Distance, Color.green);
		Debug.DrawLine (v, v1 + direction [1] * maxRayDistance, Color.red);

		Ray ray2 = new Ray (v1, direction [1]);
		RaycastHit hit2;
		if (Physics.Raycast (ray2, out hit2, maxRayDistance)) {
			if (hit2.collider.gameObject.name.Contains ("jalan")) {
				GameObject go = hit2.collider.gameObject;
				currentStreet = hit2.collider.gameObject;
				if (beforeCurrentStreet == null)
					beforeCurrentStreet = currentStreet;

				if (currentStreet.GetComponent<StreetProp> ().streetType == StreetProp.type.normal) {
					//Debug.Log ("ini Jalan");
					if (!go.GetComponent<StreetProp> ().isCommandExecuted) {
						if (beforeCurrentStreet.name != currentStreet.name) {
							willExecuteCurrentCommand = false;
							if (go.GetComponent<StreetProp> ().cmdFrom == StreetProp.commandFrom.down && isLessThan (v1.z, go.transform.position.z, minVal)) {
								willExecuteCurrentCommand = true;
							} else if (go.GetComponent<StreetProp> ().cmdFrom == StreetProp.commandFrom.left && isLessThan (v1.x, go.transform.position.x, minVal)) {
								willExecuteCurrentCommand = true;
							} else if (go.GetComponent<StreetProp> ().cmdFrom == StreetProp.commandFrom.up && isGreaterThan (v1.z, go.transform.position.z, minVal)) {
								willExecuteCurrentCommand = true;
							} else if (go.GetComponent<StreetProp> ().cmdFrom == StreetProp.commandFrom.right && isGreaterThan (v1.x, go.transform.position.x, minVal)) {
								willExecuteCurrentCommand = true;
							}
						}
						//Debug.Log (Mathf.Round (BajajController.playerDirection.getDirectionAxis (v1)) + " - " + Mathf.Round (BajajController.playerDirection.getDirectionAxis (go.transform.position)));
						if (Mathf.Round (BajajController.playerDirection.getDirectionAxis (v1, 3)) == Mathf.Round (BajajController.playerDirection.getDirectionAxis (go.transform.position))) {
							//Debug.Log ("position = true");
							if (willExecuteCurrentCommand) {
								if (go.GetComponent<StreetProp> ().streetType == StreetProp.type.finish) {
									GameController.gameModel.IsFinished = true;
								}
								turnBajaj (go);
								go.GetComponent<StreetProp> ().isLeftFromRoad = true;
								go.GetComponent<StreetProp> ().isCommandExecuted = true;
							}
						}
						beforeCurrentStreet = currentStreet;
					}
				}
			} else {
				currentStreet = null;
			}
		}

		Ray ray1 = new Ray (v, direction [0]);
		RaycastHit[] hits = Physics.RaycastAll (ray1, maxRay1Distance);
		v.y += (maxRay1Distance / 2);
		v = BajajController.playerDirection.getRaycastPosition(v, maxRay1Distance);
		Ray fRay1 = new Ray (v, direction [1]);
		Debug.DrawLine (v, v + direction [1] * maxRayDistance, Color.blue);
		RaycastHit[] fHits = Physics.RaycastAll (fRay1, maxRay1Distance);
		if (hits.Length > 1) {
			if (Physics.Raycast (ray1, out hits [1], maxRay1Distance)) {
				if (hits [1].collider.gameObject.name.Contains ("jalan")) {
					//Debug.Log ("Hit 1 = " + hits [1].collider.gameObject.name);
					nextStreet = hits [1].collider.gameObject;
				}
			}
		} else if (fHits.Length > 0) {
			if (Physics.Raycast (fRay1, out fHits [0], maxRay1Distance)) {
				if (fHits [0].collider.gameObject.name.Contains ("jalan")) {
					//Debug.Log ("fHit 1 = " + fHits [0].collider.gameObject.name);
					nextStreet = fHits [0].collider.gameObject;
				}
			}
		} else {
			nextStreet = null;
		}
	}

	private void turnBajaj(GameObject go){
		if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.turnRight)) {
			BajajController.cmd [0] = true;
		} else if (go.GetComponent<StreetProp> ().turnListener (StreetProp.command.turnLeft)) {
			BajajController.cmd [1] = true;
		}
	}

	public bool isLessThan(float firstValue, float secondValue, float minVal){
		return firstValue < secondValue && secondValue - firstValue > minVal ? true : false;
	}

	public bool isGreaterThan(float firstValue, float secondValue, float minVal){
		return firstValue > secondValue && firstValue - secondValue > minVal ? true : false;
	}
}
