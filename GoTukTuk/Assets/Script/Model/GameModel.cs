using System.Collections;
using System.Collections.Generic;

public class GameModel{

	private bool _isAction;
	private bool isStarted = false;
	private bool isFinished;
	private bool isPaused;
	private bool isGameOver;
	private int _collectedStar;
	private int _encCode;

	public GameModel(){
		System.Random rndInt = new System.Random ();
		_encCode = rndInt.Next(50,100);
		_collectedStar = encryptStar(0);
	}

	private int encryptStar(int value){
		if (value > 3)
			value = 3;
		if (value < 0)
			value = 0;
		return value * this._encCode + this._encCode;
	}

	private int decryptStar(){
		return (this._collectedStar - this._encCode) / this._encCode;
	}

	public int encCode{
		get { return this._encCode; }
	}

	public int collectedStar{
		get { return decryptStar(); }
		set { this._collectedStar = encryptStar (value); }
	}

	public bool isAction {
		get { return this._isAction; }
		set { this._isAction = value;}
	}

	public bool IsStarted {
		get{ return this.isStarted;}
		set{ 
			this.isStarted = value;
		}		
	}

	public bool IsFinished {
		get{ return this.isFinished;}
		set{ 
			this.isFinished = value;
		}
	}

	public bool IsPaused {
		get{ return this.isPaused;}
		set{ 
			this.isPaused = value;
		}
	}
		
	public bool IsGameOver {
		get{ return this.isGameOver;}
		set{ 
			this.isGameOver = value;
		}
	}

}
