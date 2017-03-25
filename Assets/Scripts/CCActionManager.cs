using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CCActionManager : SSActionManager,ISSActionCallBack {
	public FirstSceneController sceneController;


	// Use this for initialization
	protected new void Start () {
		sceneController = (FirstSceneController)SSDirector.getInstance ().currentScenceController;
		sceneController.actionManager = this;
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update ();
	}

	public void SSActionEvent(SSAction source,SSActionEventType events = SSActionEventType.Completed,
		int intParam = 0,
		string strParam = null,
		Object objectParam = null){
	}
}
