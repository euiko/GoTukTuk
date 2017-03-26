using System.Collections;

public class ButtonModel {
	public enum type {direction, jump, brake, delete};

	private type buttonType;
	private bool isActive;

	public type ButtonType{
		get{ return this.buttonType; }
		set{
			this.buttonType = value;
		}
	}

	public void setActive(){
		this.isActive = true;
	}

	public void setInActive(){
		this.isActive = false;
	}

	public bool getIsActive(){
		return this.isActive;
	}

	public bool buttonListener(type buttonType){
		return this.buttonType == buttonType ? true : false;
	}

}
