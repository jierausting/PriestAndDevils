using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionOnBoard : SSAction {
	public Vector3 target;

	public static CCActionOnBoard GetSSAction(GameObject obj,Vector3 target) {
		CCActionOnBoard action = ScriptableObject.CreateInstance<CCActionOnBoard> ();
		action.gameobject = obj;
		action.target = target;
		return action;
	}
	// Use this for initialization
	public override void Start () {

	}

	// Update is called once per frame
	public override void Update () {
		gameobject.transform.localPosition = target;
		Debug.Log (gameobject.transform.name+"CCActionOnBoard "+target.x+" " +target.y+" "+target.z );
		if (gameobject.transform.localPosition == target) {
			this.destroy = true;
		}
	}
}
