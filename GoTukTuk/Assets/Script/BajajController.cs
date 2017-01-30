using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajajController : MonoBehaviour {

	public WheelCollider TireBL;
	public WheelCollider TireBR;
	public WheelCollider TireF;
	public float maxTorque = 50;

	private Rigidbody rbBajai;
	private Animator aBajai;
	private bool state;
	private int curAngle, targetAngle;
	private string direction = "right";

	// Use this for initialization
	void Start () {
		rbBajai = GetComponent<Rigidbody> ();
		aBajai = GetComponent<Animator> ();
		curAngle = 0; targetAngle = 0;
		Vector3 v = rbBajai.centerOfMass;
		v.y = -0.9f;
		rbBajai.centerOfMass = v;
	}

	void FixedUpdate(){

		//float speed = Input.GetAxis ("Vertical") > 0.2f ? 0.2f : Input.GetAxis ("Vertical") < -0.1f ? -0.1f : Input.GetAxis("Vertical");
		float speed = 0.2f;
		TireBL.motorTorque = maxTorque * speed;
		TireBR.motorTorque = maxTorque * speed;

		if (Input.GetKey ("right")) {
			aBajai.SetBool ("inputR", true);
			direction = "right";
			targetAngle = curAngle + 90;
		}
		if (Input.GetKey ("left")) {
			aBajai.SetBool ("inputL", true);
			direction = "left";
			targetAngle = curAngle - 90;
		}


		checkTurn (1, direction);
		aBajai.SetFloat ("inputV", speed);
	}

	// Update is called once per frame
	void Update () {

		checkTurn (1, direction);

	}

	void checkTurn (int inc, string direction = "right"){
		if (aBajai.GetBool ("inputR") || aBajai.GetBool ("inputL")) {
			curAngle += direction == "right" ? inc : -inc;
			transform.RotateAround( direction == "right" ? TireBR.transform.position : TireBL.transform.position, Vector3.up, direction == "right" ? inc : -inc);

			if(curAngle == targetAngle){
				int diff = targetAngle % 90;
				targetAngle -= diff;
				transform.rotation = Quaternion.Euler(0,targetAngle,0);
				aBajai.SetBool ("inputR", false);
				aBajai.SetBool ("inputL", false);
			}
		}
	}

	bool checkAnimationState(string animationName){
		if(this.aBajai.GetCurrentAnimatorStateInfo(0).IsName(animationName))
			this.state = true;
		else if (this.state)
			this.state = false;
		return this.state;
	}
}
