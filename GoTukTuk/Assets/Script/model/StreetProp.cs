﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetProp : MonoBehaviour {

	public enum commandFrom {up, right, down, left};
	public enum command {noCommand, turnLeft, turnRight, jump};
	public enum type {normal, start, finish};

	public commandFrom cmdFrom;
	public command cmd;
	public type streetType;
	public bool isCommandExecuted = false;

	public bool turnListener(command cmd){
		return this.cmd == cmd ? true : false;
	}
}
