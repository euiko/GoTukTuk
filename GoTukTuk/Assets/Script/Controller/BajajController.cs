using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajajController : MonoBehaviour {

	public static PlayerDirection playerDirection = new PlayerDirection ();
	public static bool[] cmd = {false, false};

	public WheelCollider TireBL;
	public WheelCollider TireBR;
	public WheelCollider TireF;
	public float maxTorque = 50;
	public int spd = 6;

	private Rigidbody rbBajai;
	private Animator aBajai;
	private bool state, isCentered = false;
	private int curAngle;
	private float speed = 0;
	public enum to {right, left};

	// Use this for initialization
	void Start () {
		rbBajai = GetComponent<Rigidbody> ();
		aBajai = GetComponent<Animator> ();
		playerDirection.setCurrentDirection (0);
		playerDirection.setCurrentDirectionIndex (3);
		playerDirection.setSpeed (spd);

		curAngle = 0;
		Vector3 v = rbBajai.centerOfMass;
		v.y = -0.9f;
		rbBajai.centerOfMass = v;
	}

	void FixedUpdate(){
		if (GameController.gameModel.IsStarted) {
			this.speed = 0.2f;
			GameController.gameModel.IsStarted = false;
		}
		TireBL.motorTorque = maxTorque * speed;
		TireBR.motorTorque = maxTorque * speed;

		if (cmd[0]) {
			cmd [0] = false;
			turnTo(to.right);
		}
		if (cmd[1]) {
			cmd [1] = false;
			turnTo(to.left);
		}


		aBajai.SetFloat ("inputV", speed);
	}

	// Update is called once per frame
	void Update () {
		checkTurn();
		//moveToCenterOfRoad();
	}

	public void turnTo(BajajController.to direction){
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
					isCentered = false;
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
					isCentered = false;
				}
				break;
			default:
				break;
			}
		}
	}

	void moveToCenterOfRoad(){
		BajaiRaycast rayO = GetComponent<BajaiRaycast> ();
		if (rayO.currentStreet != null) {
			GameObject go = rayO.currentStreet;
			GameObject go1 = rayO.nextStreet;
			if (Mathf.Round (BajajController.playerDirection.getDirectionAxis(transform.position)) != Mathf.Round (BajajController.playerDirection.getDirectionAxis (go.transform.position))) {
				if (!isCentered && go1 != null) {
					Debug.Log ("Not isCentered");
					float step = 300 * Time.deltaTime;
					transform.position = Vector3.MoveTowards (transform.position, go.transform.position, step);
					if (Mathf.Round (BajajController.playerDirection.getDirectionAxis (transform.position)) == Mathf.Round (BajajController.playerDirection.getDirectionAxis (go.transform.position))) {
						isCentered = true;
						Debug.Log ("isCentered");
					}
				}
			}
		}
	}

}
