using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//OBJECTIVE: 
/*This is a debug tactic not meant to make it to the end.
 */
[CreateAssetMenu(fileName = "Light Attack", menuName = "Tactics/Light Attack", order = 1)]
public class LightAttack : Tactic {

	public override void onUse(Waypoint Current, GameObject CallingUnit, GameObject Target){
		Debug.Log("Activated light attack tactic");
	}
}
