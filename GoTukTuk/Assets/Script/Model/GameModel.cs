using System.Collections;
using System.Collections.Generic;

public class GameModel{

	private bool isStarted = false;
	private bool isFinished;
	private bool isPaused;
	private bool isGameOver;

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
