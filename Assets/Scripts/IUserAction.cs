using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Sun-Yet-Sun Universsity,School of Software,Jiajun Zheng*/
/*IUserAction can be a bridge between UI and Controller
It tells the Controller it what it wants(Interface)*/
public interface IUserAction {
	
	bool MoveBoat ();
	bool OnBoard ();
	bool OffBoard(); 
	void Restart ();
	int getCurrentState();
	float getTimeLeft ();
}
