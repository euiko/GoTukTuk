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
	private int curAngle, targetAngle;
	private string direction = "right";
	private enum to {right, left};

	// Use this for initialization
	void Start () {
		rbBajai = GetComponent<Rigidbody> ();
		aBajai = GetComponent<Animator> ();
		playerDirection.setCurrentDirection (0);
		playerDirection.setCurrentDirectionIndex (3);
		playerDirection.setSpeed (3);

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

		if (Input.GetKeyUp ("right")) {
		//	aBajai.SetBool ("inputR", true);
		//	direction = "right";
		//	transform.rotation = Quaternion.Euler(0,curAngle,0);
		//	targetAngle = curAngle + 90;
			turnTo(to.right);
		}
		if (Input.GetKeyUp ("left")) {
		//	aBajai.SetBool ("inputL", true);
		//	direction = "left";
		//	transform.rotation = Quaternion.Euler(0,curAngle,0);
		//	targetAngle = curAngle - 90;
			turnTo(to.left);
		}


		aBajai.SetFloat ("inputV", speed);
	}

	// Update is called once per frame
	void Update () {
	//	checkTurn (5, direction);
		checkTurn();
	}

	int roundTo90(int num){
		int res;
		int cNum = num / 90;
		int diff = num % 90;
		if (diff >= 45) {
			cNum += 1;
		}
		res = cNum * 90;
		return res;
	}

	void checkTurn (int inc, string direction = "right"){
		if (aBajai.GetBool ("inputR") || aBajai.GetBool ("inputL")) {
			curAngle += direction == "right" ? inc : -inc;
			transform.RotateAround( direction == "right" ? TireBR.transform.position : TireBL.transform.position, Vector3.up, direction == "right" ? inc : -inc);

			if(curAngle == targetAngle){
				curAngle = roundTo90 (targetAngle);
				Debug.Log (curAngle);
				transform.rotation = Quaternion.Euler(0,curAngle,0);
				aBajai.SetBool ("inputR", false);
				aBajai.SetBool ("inputL", false);
			}
		}
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
//					playerDirection.setCurrentDirectionIndex (playerDirection.getCurrentDirectionIndex() + 1);
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
					//playerDirection.setCurrentDirectionIndex (playerDirection.getCurrentDirectionIndex () - 1);
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
