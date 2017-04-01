using System.Collections.Generic;

public class Level{
	private bool _isLevelCompleted;
	private int _time;
	private int _star;

	public bool isLevelCompleted{
		get{ return this._isLevelCompleted;}
		set{ this._isLevelCompleted = value;}
	}

	public int time{
		get{ return this._time;}
		set{ this._time = value;}
	}

	public int star{
		get{ return this._star >= 0 && this._star <= 3 ? this._star : 0;}
		set{ this._star = value >= 0 && value <= 3 ? value : 0;	}
	}

	public static Level Save(bool isLevelCompleted, int time, int star)	{
		Level level = new Level();
		level.isLevelCompleted = isLevelCompleted;
		level.time = time;
		level.star = star;
		return level;
	}
}