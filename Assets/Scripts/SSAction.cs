using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject {
	public bool enable = true;
	public bool destroy = false;

	public GameObject gameobject { get; set;}
	public Transform transform { get; set;}
	public ISSActionCallBack callback { get; set;}

	protected SSAction() {}
	// Use this for initialization
	public virtual void Start () {
		
	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}
}
