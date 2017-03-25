using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Sun-Yet-Sun Universsity,School of Software,Jiajun Zheng*/
/*UI is the View of MVC, In this project,UI just draw the button.User interact with the
game by the mean of UI.UI do not contain any detail.And it can not modify the Model.*/

public class UserGUI : MonoBehaviour {
	private IUserAction action;
	// Use this for initialization
	void Start () {
		action = SSDirector.getInstance ().currentScenceController as IUserAction;
	}
	void Update() {
		if (Input.GetMouseButtonUp(0)) {
			if (action.getCurrentState () == 1) {
				if (!action.OffBoard ()) {
					action.OnBoard ();
				};
			}
		}
	}
	
	void OnGUI() {

		/*Restart*/
		if (GUI.Button (new Rect (325,50,160,30), "RESTART")) {
			action.Restart ();
		}

		/*GO ->MoveBoat, and this can only be done when the gamestate is playing*/
		if (GUI.Button (new Rect (325, 90, 160, 30), "GO!")) {
			if (action.getCurrentState () == 1) {
				action.MoveBoat ();
			}

		}

		/*When click the mouse check the rayhit event.The same as above , it need the 
		gamestate to be playing*/



		/*TextArea to tell the player the state of the game*/
		if (action.getCurrentState () == 0) {
			GUI.Label (new Rect (520, 90, 60, 30), "Failed");
		} else if (action.getCurrentState () == 1) {
			GUI.Label (new Rect (520, 90, 60, 30), "Playing");
		} else {
			GUI.Label (new Rect (520, 90, 60, 30), "Succeed");
		}

		/*Time count down ,because action.getTimeLeft()is float we need to convert it to int*/
		GUI.Label (new Rect(520,60,100,30),"TimeLeft: "+(int)action.getTimeLeft());
	}
}
