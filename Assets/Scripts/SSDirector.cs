using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Sun-Yet-Sun Universsity,School of Software,Jiajun Zheng*/
/*From director ,we can instantiate different scenes' controllers.
The director holds the game.The scene controller holds their scene.*/
public class SSDirector :System.Object {

	private static SSDirector _instance;

	public ISceneController currentScenceController { get; set;}
	public bool running { get; set;}

	public static SSDirector getInstance() {
		if (_instance == null) {
			_instance = new SSDirector ();
		}
		return _instance;
	}

	public int getFPS() {
		return Application.targetFrameRate;
	}
	public void setFPS(int fps) {
		Application.targetFrameRate = fps;
	}
}
