

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;



public abstract class Tactic : ScriptableObject {

	[TextArea(5,10)]
	public string description;
	public int NumberOfTargets;
	public bool allowRetalition;	

	public abstract void onUse(Waypoint Current, UnitCore ThisUnit);

	
}


