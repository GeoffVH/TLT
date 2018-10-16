using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[CreateAssetMenu(fileName = "Light Support", menuName = "Tactics/Light Support", order = 1)]
public class LightHealing : Tactic {

	public float heal;
	public override void onUse( Waypoint Current, GameObject CallingUnit, GameObject Target){
		heal = Random.Range(0.2f,0.3f)*CallingUnit.GetComponent<UnitCore>().health;
		UnitCore target_UnitCore = Target.GetComponent<UnitCore>();
		Action_ModifyTargetHealth((int)heal, target_UnitCore);
		Action_SendUIColor(target_UnitCore, getColor());
	}

	public Color getColor(){
		return new Color32(34, 139, 34, 255);
	}
}
