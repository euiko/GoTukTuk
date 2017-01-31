using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajaiRaycast : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
<<<<<<< Updated upstream
		Vector3 direction = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		Debug.DrawRay(transform.position, direction * 2, Color.green);
		if (Physics.Raycast(transform.position, direction, out hit, 2))
		{

			if (hit.collider.gameObject.name.Contains("jalan"))
			{
				Debug.Log("HIT");
			}
		}
=======
		
>>>>>>> Stashed changes
	}
}
