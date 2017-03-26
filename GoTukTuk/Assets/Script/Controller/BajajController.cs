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
	public int spd;

	private Rigidbody rbBajai;
	private Animator aBajai;
	private bool state,onCollision = false, isCentered, onTurn = false;
	private int curAngle;
	private Vector3 startPos;
	private float speed = 0;
	public enum to {right, left};

	void Awake(){
		isCentered = true;
	}

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
		}
		TireBL.motorTorque = maxTorque * speed;
		TireBR.motorTorque = maxTorque * speed;

		if (cmd[0]) {
			cmd [0] = false;
			onTurn = true;
			turnTo(to.right);
		}
		if (cmd[1]) {
			cmd [1] = false;
			onTurn = true;
			turnTo(to.left);
		}


		aBajai.SetFloat ("inputV", speed);
	}

	// Update is called once per frame
	void Update () {
		keepInPlace ();
		checkTurn();
		moveToCenterOfRoad();
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
				if (playerDirection.isSame(transform.eulerAngles.y, playerDirection.getTargetDirection(), playerDirection.getSpeed() * 4)) {
					curAngle = playerDirection.getTargetDirection () == 360? 0:playerDirection.getTargetDirection();
					//Debug.Log (curAngle);
					//transform.rotation = Quaternion.Euler (0, curAngle, 0);
					state = false;
					onTurn = false;
					isCentered = false;
				}
				break;
			case 1:
				curAngle -= playerDirection.getSpeed ();
				transform.RotateAround (TireBL.transform.position, Vector3.up, -(playerDirection.getSpeed ()));
				if (playerDirection.isSame(transform.eulerAngles.y, playerDirection.getTargetDirection(), playerDirection.getSpeed() * 4)) {
					curAngle = playerDirection.getTargetDirection () == 360? 0:playerDirection.getTargetDirection();
					//Debug.Log (curAngle);
					//transform.rotation = Quaternion.Euler (0, curAngle, 0);
					state = false;
					onTurn = false;
					isCentered = false;
				}
				break;
			default:
				break;
			}
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name.Contains("jalan")){
			startPos = transform.position;
			onCollision = true;
		}
	}

	void keepInPlace(){
		if (!GameController.gameModel.IsStarted && onCollision){
			transform.position = startPos;
		} 
	}

	void moveToCenterOfRoad(){
		BajaiRaycast rayO = GetComponent<BajaiRaycast> ();

		if (rayO.currentStreet != null) {
			//Debug.Log ("currentStreet = ada");
			GameObject go = rayO.currentStreet;
			if (Mathf.Round (BajajController.playerDirection.getDirectionAxis(transform.position)) != Mathf.Round (BajajController.playerDirection.getDirectionAxis (go.transform.position))) {
				isCentered = false;
				//Debug.Log ("isCentered = false");
				if (!isCentered && rayO.nextStreet != null) {
					//Debug.Log ("Not isCentered");
					float step = 5 * Time.deltaTime;
					Vector3 v = playerDirection.getTargetToward (transform.position, go.transform.position);
					transform.position = Vector3.MoveTowards (transform.position, v, step);
					if (Mathf.Round (BajajController.playerDirection.getDirectionAxis (transform.position)) == Mathf.Round (BajajController.playerDirection.getDirectionAxis (go.transform.position))) {
						isCentered = true;
						//Debug.Log ("isCentered");
					}
				}
			}
		}
		float angle = playerDirection.getTargetDirection () % 360;
//		Debug.Log (transform.localRotation.y + " - " + angle);
		float doubleTolerance = playerDirection.getSpeed() * 2;
		float halfSpeed = playerDirection.getSpeed () / 2;
		if(!(playerDirection.isSame(transform.eulerAngles.y, angle, doubleTolerance) || playerDirection.isSame(transform.eulerAngles.y, 360, doubleTolerance)) && !onTurn){
			Debug.Log ("Nggak lurus = " + doubleTolerance + " : " + transform.eulerAngles.y + " - " + angle);
			if (angle != 0) {
				if (transform.eulerAngles.y > angle + doubleTolerance) {
					transform.RotateAround (TireBL.transform.position, Vector3.up, -(halfSpeed));
				} else if (transform.eulerAngles.y < angle - doubleTolerance) {
					transform.RotateAround (TireBR.transform.position, Vector3.up, halfSpeed);
				}
			} else {
				if(transform.eulerAngles.y < 360 && transform.eulerAngles.y > 360 - doubleTolerance){
					transform.RotateAround (TireBR.transform.position, Vector3.up, halfSpeed);
				} else if (transform.eulerAngles.y > doubleTolerance) {
					transform.RotateAround (TireBL.transform.position, Vector3.up, -(halfSpeed));
				}
			}
		}
	}

}
