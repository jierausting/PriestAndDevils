using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAction : SSAction {
	public Vector3 target;

	public static CCAction GetSSAction(Vector3 target) {
		CCAction action = ScriptableObject.CreateInstance<CCAction> ();
		action.target = target;
		return action;
	}
	// Use this for initialization
	public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
		this.transform.localPosition = target;
		if (this.transform.localPosition == target) {
			this.destroy = true;
		}
	}
}
