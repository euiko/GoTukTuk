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
	public int spd, explosion;

	private Rigidbody rbBajai;
	private Animator aBajai;
	private bool state, onCollision = false, isCentered, onTurn = false, isDestroyed = false;
	private int curAngle;
	private Vector3 startPos, destroyTarget;
	private float speed = 0;
	public enum to {right, left};

	void Awake(){
		isCentered = true;
	}

	// Use this for initialization
	void Start () {
		if (GameController.gameModel == null)
			GameController.gameModel = new GameModel ();
		rbBajai = GetComponent<Rigidbody> ();
		aBajai = GetComponent<Animator> ();
		playerDirection.setCurrentDirection (0);
		playerDirection.setCurrentDirectionIndex (3);
		playerDirection.setSpeed (spd);
		curAngle = 0;
		Vector3 v = rbBajai.centerOfMass;
		v.y = -0.9f;
		//v.z = 2.5f;
		rbBajai.centerOfMass = v;
	}

	void FixedUpdate(){
		if (!GameController.gameModel.IsPaused && !isDestroyed) {
			if (GameController.gameModel.IsStarted) {
				this.speed = 0.2f;
			}
			gameOverAction ();

			TireBL.motorTorque = maxTorque * speed;
			TireBR.motorTorque = maxTorque * speed;

			if (cmd [0]) {
				cmd [0] = false;
				onTurn = true;
				turnTo (to.right);
			}
			if (cmd [1]) {
				cmd [1] = false;
				onTurn = true;
				turnTo (to.left);
			}


			keepInPlace ();
			checkTurn ();
			moveToCenterOfRoad ();

			aBajai.SetFloat ("inputV", speed);
		}
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
				if (playerDirection.isSame(transform.eulerAngles.y, playerDirection.getTargetDirection(), playerDirection.getSpeed() * 6)) {
					curAngle = playerDirection.getTargetDirection () == 360? 0:playerDirection.getTargetDirection();
					Debug.Log ("Selesai Belok Kanan = " + transform.rotation);
					//transform.rotation = Quaternion.Euler (0, curAngle, 0);
					state = false;
					onTurn = false;
					isCentered = false;
				}
				break;
			case 1:
				curAngle -= playerDirection.getSpeed ();
				transform.RotateAround (TireBL.transform.position, Vector3.up, -(playerDirection.getSpeed ()));
				if (playerDirection.isSame(transform.eulerAngles.y, playerDirection.getTargetDirection(), playerDirection.getSpeed() * 6)) {
					curAngle = playerDirection.getTargetDirection () == 360? 0:playerDirection.getTargetDirection();
					Debug.Log ("Selesai Belok Kiri" + transform.rotation);
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

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name.Contains ("Star")) {
			StarController.collectingStar = true;
			Destroy (col.gameObject);
		}

		if (col.gameObject.name.Contains ("Finish")) {
			GameController.gameModel.isAction = true;
			GameController.gameModel.IsFinished = true;
		}

		if (col.gameObject.name.Contains ("Collider")) {
			GameController.gameModel.isAction = true;
			GameController.gameModel.IsGameOver = true;
			GetComponent<Animator> ().enabled = false;
		}
	}


	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name.Contains("jalan") && !onCollision){
			startPos = transform.position;
			onCollision = true;
		}

		if (col.gameObject.name.Contains ("Collider")) {
			GameController.gameModel.isAction = true;
			GameController.gameModel.IsGameOver = true;
			GetComponent<Animator> ().enabled = false;
		}
			
	}

	void keepInPlace(){
		if (!GameController.gameModel.IsStarted && onCollision){
			transform.position = startPos;
		} 
	}

	void gameOverAction(){
		if (GameController.gameModel.IsGameOver && !isDestroyed) {
			//if (destroyAction) {
				destroyTarget = new Vector3 (transform.position.x, startPos.y + explosion, transform.position.z);
			//	destroyAction = false;
			//}
			//Debug.Log ("Sedang Menghancurkan");
			transform.position = Vector3.MoveTowards (transform.position, destroyTarget, 20 * Time.deltaTime);
			Debug.Log ("Jarak = " + Vector3.Distance (transform.position, destroyTarget));
			if (Vector3.Distance(transform.position, destroyTarget) < explosion / 2) {
				isDestroyed = true;
				speed = 0;
				Debug.Log ("Hancur");
			}
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
			//Debug.Log ("Nggak lurus = " + doubleTolerance + " : " + transform.eulerAngles.y + " - " + angle);
			if (angle != 0) {
				if (transform.eulerAngles.y > angle + doubleTolerance) {
					Debug.Log ("Nggak sama ngiri");
					transform.RotateAround (TireBL.transform.position, Vector3.up, -(halfSpeed));
				} else if (transform.eulerAngles.y < angle - doubleTolerance) {
					transform.RotateAround (TireBR.transform.position, Vector3.up, halfSpeed);
					Debug.Log ("Nggak sama nganan");
				}
			} else {
				if(transform.eulerAngles.y < 360 && transform.eulerAngles.y > 360 - 2*doubleTolerance){
					transform.RotateAround (TireBR.transform.position, Vector3.up, halfSpeed);
					Debug.Log ("Nggak sama ngan");
				} else if (transform.eulerAngles.y > doubleTolerance) {
					transform.RotateAround (TireBL.transform.position, Vector3.up, -(halfSpeed));
					Debug.Log ("Nggak sama ngiri");
				}
			}
		}
	}

}
