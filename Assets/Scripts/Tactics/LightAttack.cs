using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//OBJECTIVE: 
/*This is a debug tactic not meant to make it to the end.
 */
[CreateAssetMenu(fileName = "Light Attack", menuName = "Tactics/Light Attack", order = 1)]
public class LightAttack : Tactic {

	public float damage;
	public override void onUse(Waypoint Current, GameObject CallingUnit, GameObject Target){
		damage = Random.Range(0.2f,0.3f)*CallingUnit.GetComponent<UnitCore>().health;
		UnitCore target_UnitCore = Target.GetComponent<UnitCore>();
		Attack_Target((int)damage, target_UnitCore);
		Action_SendUIColor(target_UnitCore, getColor());
	}

	public float getDamage(){
		return damage;
	}

	public Color getColor(){
		return new Color32(255, 255, 51, 255);
	}
}
