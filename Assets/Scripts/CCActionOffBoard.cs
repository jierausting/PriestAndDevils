using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionOffBoard : SSAction {
	public Vector3 target;

	public static CCActionOffBoard GetSSAction(GameObject obj,Vector3 target) {
		CCActionOffBoard action = ScriptableObject.CreateInstance<CCActionOffBoard> ();
		action.gameobject = obj;
		action.target = target;
		return action;
	}
	// Use this for initialization
	public override void Start () {

	}

	// Update is called once per frame
	public override void Update () {
		gameobject.transform.position = target;
		Debug.Log (gameobject.transform.name+"CCActionOffBoard"+target.x+" " +target.y+" "+target.z);
		if (gameobject.transform.position == target) {
			this.destroy = true;
		}
	}
}
