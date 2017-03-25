using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionMoveBoat : SSAction {
	public Vector3 target;
	public float speed;

	public static CCActionMoveBoat GetSSAction(GameObject obj,Vector3 target,float speed) {
		CCActionMoveBoat action = ScriptableObject.CreateInstance<CCActionMoveBoat> ();
		action.gameobject = obj;
		action.target = target;
		action.speed = speed;
		return action;
	}
	// Use this for initialization
	public override void Start () {

	}

	// Update is called once per frame
	public override void Update () {
		gameobject.transform.position = Vector3.MoveTowards(gameobject.transform.position,target,speed);
		Debug.Log (gameobject.transform.name+"CCActionMoveBoat"+target.x+" " +target.y+" "+target.z);
		if (gameobject.transform.position == target) {
			this.destroy = true;
		}
	}
}
