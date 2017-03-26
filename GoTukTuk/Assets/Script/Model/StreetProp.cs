﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetProp : MonoBehaviour {
	public enum commandFrom {down, left, up, right};
	public enum command {noCommand, turnLeft, turnRight, jump};
	public enum type {normal, start, finish};

	public bool fromUp, fromRight, fromDown, fromLeft;
	public command cmd;
	public commandFrom cmdFrom;
	public type streetType;
	public bool isCommandExecuted = false;
	private bool _isOnAction;
	private bool _isLeft;

	void Awake(){
		if (fromDown) {
			this.cmdFrom = commandFrom.down;
		}else if (fromLeft) {
			this.cmdFrom = commandFrom.left;
		}else if (fromUp) {
			this.cmdFrom = commandFrom.up;
		}else if (fromRight) {
			this.cmdFrom = commandFrom.right;
		}
	}

	public bool isLeft{
		get{ return this._isLeft; }
		set{ this._isLeft = value; }
	}

	public bool isOnAction {
		get{ return this._isOnAction; }
		set{ this._isOnAction = value; }
	}

	public bool turnListener(command cmd){
		return this.cmd == cmd ? true : false;
	}

	public bool turnListener(commandFrom cmdFrom, command cmd){
		return this.cmdFrom == cmdFrom && this.cmd == cmd ? true : false;
	}

	public commandFrom getCurCommandFrom(){
		bool[] arrayFrom = {fromDown, fromLeft, fromUp, fromRight };
		int i = (int)cmdFrom;
		if (this.cmd == command.turnLeft) {
			int j = 0;
			while (j < arrayFrom.Length) {
				i++;
				j++;
				if (i >= arrayFrom.Length)
					i = i % 4;


				if (arrayFrom [i]) {
					cmdFrom = (commandFrom)i;
					return (commandFrom)i;	
				}
			}
			return (commandFrom)i;
		} else {
			return (commandFrom)i;
		}
	}
}
