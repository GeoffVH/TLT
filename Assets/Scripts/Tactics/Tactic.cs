

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;



public abstract class Tactic : ScriptableObject {

	[TextArea(5,7)]
	public string description;
	public int NumberOfTargets;
	public bool allowRetalition;	
	public bool isSupport;

	public abstract void onUse( Waypoint Current, 
								GameObject CallingUnit, 
								GameObject Target);
}


