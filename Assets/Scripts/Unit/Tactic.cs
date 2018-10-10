

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//This is the main scriptable object for tactics. General things that could apply to all tactics should be added here.
//Modifiers, buffs, debuffs, ect. 
//This script will activate the unit's tactics. 

[CreateAssetMenu(fileName = "New Tactic", menuName = "Tactic")]
public class Tactic : ScriptableObject {

	//public string TacticName;
	public int NumberOfTargets;
	public bool allowRetalition;	

	public void onUse(Waypoint Current, UnitCore ThisUnit){
		Tactics_Master_List TacticMasterList = GameObject.Find("GameManager").GetComponent<Tactics_Master_List>();

		if(name == "LightAttack") TacticMasterList.LightAttack(Current, ThisUnit, NumberOfTargets, allowRetalition);
		//if(name == "MedAttack") TacticMasterList.MedAttack(Current, ThisUnit, NumberOfTargets);
		//if(name == "HeavyAttack") TacticMasterList.HeavyAttack(Current, ThisUnit, NumberOfTargets);
	}
}


