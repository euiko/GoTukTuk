using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajajController : MonoBehaviour {

	public static PlayerDirection playerDirection = new PlayerDirection ();

	public WheelCollider TireBL;
	public WheelCollider TireBR;
	public WheelCollider TireF;
	public float maxTorque = 50;

	private Rigidbody rbBajai;
	private Animator aBajai;
	private bool state;
	private int curAngle;
	private enum to {right, left};

	// Use this for initialization
	void Start () {
		rbBajai = GetComponent<Rigidbody> ();
		aBajai = GetComponent<Animator> ();
		playerDirection.setCurrentDirection (0);
		playerDirection.setCurrentDirectionIndex (3);
		playerDirection.setSpeed (3);

		curAngle = 0;
		Vector3 v = rbBajai.centerOfMass;
		v.y = -0.9f;
		rbBajai.centerOfMass = v;
	}

	void FixedUpdate(){

		float speed = 0.2f;
		TireBL.motorTorque = maxTorque * speed;
		TireBR.motorTorque = maxTorque * speed;

		if (Input.GetKeyUp ("right")) {
			turnTo(to.right);
		}
		if (Input.GetKeyUp ("left")) {
			turnTo(to.left);
		}


		aBajai.SetFloat ("inputV", speed);
	}

	// Update is called once per frame
	void Update () {
		checkTurn();
	}

	void turnTo(BajajController.to direction){
		playerDirection.setCurrentDirection((int) direction);
		playerDirection.setCurrentDirectionIndex (playerDirection.getCurrentDirectionIndex () + playerDirection.getNextDirection());
		transform.rotation = Quaternion.Euler(0,playerDirection.getDirection(),0);
		curAngle = playerDirection.getDirection ();
		state = true;
	}

	void checkTurn(){
		if (state) {
			switch (playerDirection.getCurrentDirection ()) {
			case 0:
				curAngle += playerDirection.getSpeed ();
				transform.RotateAround (TireBR.transform.position, Vector3.up, playerDirection.getSpeed ());
				if (curAngle.Equals(playerDirection.getTargetDirection ())) {
					curAngle = playerDirection.getTargetDirection () == 360? 0:playerDirection.getTargetDirection();
					Debug.Log (curAngle);
					transform.rotation = Quaternion.Euler (0, curAngle, 0);
					state = false;
				}
				break;
			case 1:
				curAngle -= playerDirection.getSpeed ();
				transform.RotateAround (TireBL.transform.position, Vector3.up, -(playerDirection.getSpeed ()));
				if (curAngle.Equals(playerDirection.getTargetDirection ())) {
					curAngle = playerDirection.getTargetDirection () == 360? 0:playerDirection.getTargetDirection();
					Debug.Log (curAngle);
					transform.rotation = Quaternion.Euler (0, curAngle, 0);
					state = false;
				}
				break;
			default:
				break;
			}
		}
	}

}
