using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//OBJECTIVE: 
/*This is a debug tactic not meant to make it to the end.

Tactics should store information needed by the combat manager to figure out what it wants to do. 



 */
[CreateAssetMenu(fileName = "Light Attack", menuName = "Tactics/Light Attack", order = 1)]
public class LightAttack : Tactic {

	public override void onUse(Waypoint Current, UnitCore ThisUnit){
		Debug.Log("Activated light attack tactic");
	}
}
