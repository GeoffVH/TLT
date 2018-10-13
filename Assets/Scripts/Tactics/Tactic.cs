

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//This is the main scriptable object for tactics. General things that could apply to all tactics should be added here.
//Modifiers, buffs, debuffs, ect. 
//This script will activate the unit's tactics. 


public abstract class Tactic : ScriptableObject {
	[TextArea(5,10)]
	public string description;
	public int NumberOfTargets;
	public string targetType;
	public bool allowRetalition;
	public float randomRangeStart;
	public float randomRangeEnd;	

	public abstract void activate(Waypoint Current, UnitCore ThisUnit);
}


