

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

	//Deals set damage to UnitCore target
	public void Attack_Target(int damage, UnitCore target){
		target.health -= damage;
		Debug.Log("Dealt " + damage + " to " + target.name);
		Action_UpdateUIHealth(target);
		Action_IfDeadKillUnit(target);
	}	

	//Calls the UI to use it's colorflash given a specific color.
	public  void Action_SendUIColor(UnitCore target, Color color){
		target.GeneratedUI.GetComponent<LinkDebugToUnit>().ColorFlash(color);
	}

	//Calls the target's UI to update it's health.
	public  void Action_UpdateUIHealth(UnitCore target){
		target.GeneratedUI.GetComponent<LinkDebugToUnit>().HealthChanged(target);
	}

	//This function will choose if the target remains on the field.
	public  void Action_IfDeadKillUnit(UnitCore target){
		if(target.health < 0){
			target.death();
		}
	}
}


