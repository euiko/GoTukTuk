﻿using System.Collections;
using UnityEngine;
using System;

public class PlayerDirection{
	private int[,] direction = new int[2,4]{{0, 90, 180, 270}, {90, 180, 270, 360}};
	private int currentDirection = 99;
	private int currentDirectionIndex = 3;
	private int speed = 1;
	private bool isTurning = false;

	public Vector3 getVectorDirection(int direction, int index, Vector3 currentRotation){
		currentRotation.z = this.direction [direction, index];
		return currentRotation;
	}

	public int getDirection(int direction, int index){
		return this.direction[direction,index];
	}

	public int getTargetDirection(int direction, int index){
		return this.direction [direction == 0 ? 1 : 0 , index];
	}

	public int getDirection(){
		return this.direction[this.currentDirection, this.currentDirectionIndex];
	}

	public int getTargetDirection(){
		return this.direction [this.currentDirection == 0 ? 1 : 0 , this.currentDirectionIndex];
	}

	public void setCurrentDirection(int value){
		if (this.currentDirection != value)
			isTurning = true;
		else
			isTurning = false;
		this.currentDirection = limit(value, 0, 1);
	}

	public void setCurrentDirectionIndex(int value){
		Debug.Log ("Before :" + value);
		this.currentDirectionIndex = limit(value, 0, 3);
		Debug.Log (this.currentDirectionIndex);
	}

	public int getCurrentDirection(){
		return this.currentDirection;
	}

	public int getCurrentDirectionIndex(){
		return this.currentDirectionIndex;
	}
		
	public void setSpeed(int speed){
		this.speed = speed;
	}

	public int getSpeed(){
		return (int)(this.speed * Time.deltaTime);
	}

	public int getNextDirection(){
		if (isTurning) {
			isTurning = false;
			return 0;
		}
		else if (this.currentDirection == 0)
			return 1;
		else if (this.currentDirection == 1)
			return -1;
		else
			return 0;
	}

	public float getDirectionAxis(Vector3 v){
		if (this.currentDirection == 0) {
			if (this.getTargetDirection () == direction [1, 0] || this.getTargetDirection () == direction [1, 2]) {
				//Debug.Log("x");
				return v.x;
			} else if (this.getTargetDirection () == direction [1, 1] || this.getTargetDirection () == direction [1, 3]) {
				return v.z;
			} else {
				return 0;
			}
		} else if (this.currentDirection == 1) {
			if (this.getTargetDirection () == direction [0, 0] || this.getTargetDirection () == direction [0, 2]) {
				return v.z;
			} else if (this.getTargetDirection () == direction [0, 1] || this.getTargetDirection () == direction [0, 3]) {
				return v.x;
			} else {
				return 0;
			}
		} else {
			return 0;
		}
	}

	public float getDirectionAxis(Vector3 v, int inc){
		if (this.currentDirection == 0) {
			if (this.getTargetDirection () == direction [1, 0]) {
				//Debug.Log("x");
				return v.x + inc;
			} else if (this.getTargetDirection () == direction [1, 2]) {
				return v.x - inc;
			} else if (this.getTargetDirection () == direction [1, 1]) {
				return v.z - inc;
			} else if (this.getTargetDirection () == direction [1, 3]) {
				return v.z + inc;
			} else {
				return 0;
			}
		} else if (this.currentDirection == 1) {
			if (this.getTargetDirection () == direction [0, 0]) {
				return v.z + inc;
			} else if (this.getTargetDirection () == direction [0, 2]) {
				return v.z - inc;
			} else if (this.getTargetDirection () == direction [0, 1]) {
				return v.x + inc;
			} else if (this.getTargetDirection () == direction [0, 3]) {
				return v.x - inc;
			} else {
				return 0;
			}
		} else {
			return 0;
		}
	}

	public Vector3 getTargetToward(Vector3 vS, Vector3 vT){
		if (this.currentDirection == 0) {
			if (this.getTargetDirection () == direction [1, 0] || this.getTargetDirection () == direction [1, 2]) {
				//Debug.Log("x");
				vS.z = vT.z;
			} else if (this.getTargetDirection () == direction [1, 1] || this.getTargetDirection () == direction [1, 3]) {
				vS.x = vT.x;
			}
		} else if (this.currentDirection == 1) {
			if (this.getTargetDirection () == direction [0, 0] || this.getTargetDirection () == direction [0, 2]) {
				vS.x = vT.x;
			} else if (this.getTargetDirection () == direction [0, 1] || this.getTargetDirection () == direction [0, 3]) {
				vS.z = vT.z;
			} 
		}
		return vS;
	}

	public Vector3 getRaycastPosition(Vector3 vS, int inc){
		if (this.currentDirection == 0) {
			if (this.getTargetDirection () == direction [1, 0]) {
				//Debug.Log("x");
				vS.x = vS.x + inc;
			}else if(this.getTargetDirection () == direction [1, 2]){
				vS.x = vS.x - inc;
			} else if (this.getTargetDirection () == direction [1, 1]) {
				vS.z = vS.z - inc;
			}else if(this.getTargetDirection () == direction [1, 3]){
				vS.z = vS.z + inc;
			}
		} else if (this.currentDirection == 1) {
			if (this.getTargetDirection () == direction [0, 0]) {
				vS.z = vS.z + inc;
			} else if (this.getTargetDirection () == direction [0, 2]) {
				vS.z = vS.z - inc;
			} else if (this.getTargetDirection () == direction [0, 1]) {
				vS.x = vS.x + inc;
			} else if (this.getTargetDirection () == direction [0, 3]) {
				vS.x = vS.x - inc;
			}
		}
		return vS;
	}

	private int limit(int value, int start, int end){
		int range = end - start + 1;
		Debug.Log ("Range  = " + range);
		if (value > end) {
			return value - (value / range) * range;
		} else if (value < start) {
			return value + Math.Abs(((value - end) / range) * range);
		} else{
			return value;
		}
	}

	public bool isSame(float firstValue, float secondValue, float tolerance){
		float halfTolerance = tolerance / 2;
		if (firstValue < secondValue + halfTolerance && firstValue > secondValue - halfTolerance) {
			return true;
		} else {
			return false;
		}
	}
}
