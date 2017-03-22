using System.Collections;

public class ButtonModel {
	public enum type {direction, jump, brake};

	private type buttonType;
	private bool isActive;

	public void setButtonType(type buttonType){
		this.buttonType = buttonType;
	}

	public type getButtonType(){
		return this.buttonType;
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
