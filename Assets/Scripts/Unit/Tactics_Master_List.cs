using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Tactics_Master_List : MonoBehaviour {

	GameObject MainCamera;
	void Start(){
		MainCamera = GameObject.Find("Main Camera");
	}

	//Check if the target is under 0 health.
	private  bool Check_isDead(UnitCore target){
		bool isDead = false;
		if(target.health < 0) isDead = true;
		return isDead;
	}

	//Checks if a unit's health is in good enough standing to retaliate. 
	//Further conditions should be added here specific to retaliation. 
	public  bool check_retaliate(UnitCore target){
		if(target.health > 0) canRetaliate = true;
		return canRetaliate;
	}

/************************************************************************************************               Get 
These functions are built to return a value of some kind.
When building get functions, try to keep them as specific as possible. 
*/ 


	//Returns the expected damage from a light attack given a unit. 
	public  int get_DamageLight(UnitCore CallingUnit){
		return (int) (CallingUnit.health * Random.Range(0,0.3f));
	}

	//Returns the expected damage from a medium attack given a unit. 
	public  int get_DamageMed(UnitCore CallingUnit){
		return (int) (CallingUnit.health * Random.Range(0.2f,0.5f));
	}

	//Returns the expected damage from a heavy attack given a unit. 
	public  int get_DamageHeavy(UnitCore CallingUnit){
		return (int)( CallingUnit.health * Random.Range(0.5f,0.7f));
	}

	//Gets the damage number of a standard retaliation from a target. 
	public  int get_RetaliationDamage(UnitCore retaliatingUnit){
		return (int) (retaliatingUnit.health * Random.Range(0,0.2f));
	}

	//Returns a list of targets that are opposed to the CallingUnit.
	public  List<GameObject> get_TargetList(Waypoint CurrentWaypoint, UnitCore CallingUnit){
		List<GameObject> targetList;
		if(CallingUnit.faction == "Player") targetList = CurrentWaypoint.Enemies;
		else targetList = CurrentWaypoint.Allies;
		return targetList;
	}

	//Given a List, returns a random UnitCore among the list.
	public List<UnitCore> get_RandomTarget(List<GameObject> targetList, int NumberOfTargets ){
		int index;
		List<UnitCore> targets = new List<UnitCore>();
		for(int i = 0; i<NumberOfTargets; i++){
			index = Random.Range(0,targetList.Count);
			targets.Add( targetList[index].GetComponent<UnitCore>() );
		}
		return targets;
	}


/************************************************************************************************               Attack 
These functions are built to assist any attack
*/

	//Deals set damage to UnitCore target
	public  void Attack_Target(int damage, UnitCore target){
		target.health -= damage;
		Debug.Log("Dealt " + damage + " to " + target.name);
	}	

	//If the unit is in good standing health wise, then it will retaliate against the recivingUnit.
	public  void Attack_Retaliation(UnitCore RetaliatingUnit, UnitCore recivingUnit){
		bool canTargetRetaliate = check_retaliate(RetaliatingUnit);
		if(canTargetRetaliate) Retaliation(RetaliatingUnit, recivingUnit);
	}


/************************************************************************************************               Action 
These functions do things and combine get, set, check and attack.
If your created function does not fit any of the above category, it can be put as an action.
*/


	//triggers the target unit's death. 
	private  void Action_CallUnitDeath(UnitCore target){
		target.death();
	}

	//This function will choose if the target remains on the field.
	private  void Action_IfDeadKillUnit(UnitCore target){
		if(Check_isDead(target)){
			Action_CallUnitDeath(target);
		}
	}

	//Calls the UI to use it's colorflash given a specific color.
	public  void Action_SendUIColor(UnitCore target, Color color){
		target.GeneratedUI.GetComponent<LinkDebugToUnit>().ColorFlash(color);
	}

	//Calls the target's UI to update it's health.
	private  void Action_UpdateUIHealth(UnitCore target){
		target.GeneratedUI.GetComponent<LinkDebugToUnit>().HealthChanged(target);
	}

	//Turns off the script forcing the camera to be fixed around orbit. 
	private  void Action_TurnOffCameraScript(){
		helper_CameraOnOff(false);
	}

	//Turns on the script forcing the camera to be fixed around orbit. 
	private  void Action_TurnOnCameraScript(){
		helper_CameraOnOff(true);
	}


	//Pauses units so they freeze in place. 
	private  void Action_PauseGame(){
		helper_PauseAndUnpause(true);
	}

	//Unpauses units so they continue to move.
	private  void Action_UnPauseGame(){
		helper_PauseAndUnpause(false);
	}

/************************************************************************************************               Helper 
These functions help the above functions out. We want to keep function names clear and easy to read for the tactics. 
It's better to have two functions, one that specifically turns on something and another that specifically turns off something. 
When creating tactics it'll be easier to debug and to read what the function does step by step. 
*/

	private  void helper_PauseAndUnpause(bool flag){
		GameObject EnemyUnits = GameObject.Find("Enemy Units");
		foreach (Transform child in EnemyUnits.transform){
			child.GetComponent<UnitCore>().isPaused = flag;
			//Debug.Log("We have paused " + child.GetComponent<UnitCore>().name);
		}

		GameObject PlayerUnits = GameObject.Find("Player Units");
		foreach (Transform child in PlayerUnits.transform){
			child.GetComponent<UnitCore>().isPaused = flag;
			//Debug.Log("We have paused " + child.GetComponent<UnitCore>().name);
		}
	}

	private  void helper_CameraOnOff(bool flag){
		MainCamera.GetComponent<CameraController>().enabled = flag;
	}







	//Target: Specific
	//Handles retaliation
	private  void Retaliation(UnitCore retaliatingUnit, UnitCore recivingUnit){
		int damage = get_RetaliationDamage(retaliatingUnit);
		if(check_retaliate(retaliatingUnit)){
			Attack_Target(damage, recivingUnit);
			Action_UpdateUIHealth(recivingUnit);
			Action_SendUIColor(recivingUnit, new Color32(255, 255, 51, 255));
			Action_IfDeadKillUnit(recivingUnit);
		}
	}


/*

	//Target: Random Enemy
	//Medium Damage, Retaliation, Orange UI
	public  void MedAttack (Waypoint CurrentWaypoint, UnitCore CallingUnit, int NumberOfTargets){
		int damage = get_DamageMed(CallingUnit);
		List<GameObject> targetList = get_TargetList(CurrentWaypoint, CallingUnit);
		UnitCore target = get_RandomTarget(targetList);
		Attack_Target(damage, target);
		Action_UpdateUIHealth(target);
		Action_SendUIColor(target,  new Color32(255, 128, 0, 255));
		Attack_Retaliation(target, CallingUnit);  
		Action_IfDeadKillUnit(target);		
	}

	//Target: Random Enemy
	//Heavy Damage, Retaliation, Red UI
	public  void HeavyAttack (Waypoint CurrentWaypoint, UnitCore CallingUnit, int NumberOfTargets ) {
		int damage = get_DamageHeavy(CallingUnit);
		List<GameObject> targetList = get_TargetList(CurrentWaypoint, CallingUnit);
		List<UnitCore> targets = get_RandomTarget(targetList, NumberOfTargets);
		Attack_Target(damage, target);
		Action_UpdateUIHealth(target);
		Action_SendUIColor(target,  new Color32(255, 0, 0, 255));
		Attack_Retaliation(target, CallingUnit);  
		Action_IfDeadKillUnit(target);			
	}


 */


/*
Master list of tactics.
Tactics should be constructed like lego bricks. Each line should do one specific action. 
Even if it might save space to condense common elements of tactics, they should still be written line by line as is for legibility.
It's better to have a long list of calls, each being simple to understand vs a short list of dense code. 
If in doubt, better to side on being legible rather than smart. 
*/

	private bool canRetaliate, ZoomedIn, ZoomedOut, CombatDone;


//We need to really consider just what we're going to do for attacks. 
//Tactics should be able to target ally units

	//Target: Random Enemy
	//Light Damage, No retaliate, Yellow UI
	public  void LightAttack(Waypoint CurrentWaypoint, UnitCore CallingUnit, int NumberOfTargets, bool canRetaliate){

		

		//NEW SETUP. Calculate everything first, and then send an order sequence to the camera so that it can do it's job. 
		CleanBools();
		List<GameObject> targetList = get_TargetList(CurrentWaypoint, CallingUnit);
		List<UnitCore> target = get_RandomTarget(targetList, NumberOfTargets);
		int damage, index=0;
		foreach( UnitCore troop in target){
			index++;
			damage = get_DamageLight(CallingUnit);
			Attack_Target(damage, troop);										
			if(canRetaliate) Attack_Retaliation(troop, CallingUnit); 
			if(Check_isDead(CallingUnit)) break;	//If the retaliation marks the unit for death, then break out of loop. 
		}


		//So now what do we actually have? 
		/*All of these units have had their health reduced, but only that. We know where the loop stopped.
		
		Let's assume the countine is a required element. We don't know what the end system will be but we can describe some aspects:
		-Countine ladder downwards
		-Modular code
		-Calculates things inside the countine

		//OUTSIDE COUNTINE
		-Targets can be calculated outside.
		-Who retaliates? 
		
		 
		-*/
	}

	private void CleanBools(){
		ZoomedIn = false;
		ZoomedOut = false; 
		CombatDone = false;
	}

	IEnumerator CycleThroughUnits(int damage, List<UnitCore> targets){
		//Wait until the camera has zoomed in on the waypoint.
		yield return new WaitUntil(() => ZoomedIn == true );
		Debug.Log("Begining loop through units.");
		foreach(UnitCore item in targets){
			Attack_Target(damage, item);
			Action_UpdateUIHealth(item);
			Action_SendUIColor(item, new Color32(255, 255, 51, 255));
		}
	}

	//Gets the old camera position and then moves the camera to the waypoint. 
	private  void Action_ZoomInCamera(Waypoint CurrentWaypoint, List<UnitCore> targets){
		Action_PauseGame();
		Action_TurnOffCameraScript();

		Vector3 OldCameraPosition = MainCamera.transform.position;
		Vector3 offset = new Vector3(1.79628f, 6.673105f ,-7.21338f);
		helper_MoveFromXtoYSmoothly(OldCameraPosition, CurrentWaypoint.transform.position+offset, MainCamera, CurrentWaypoint);
		Debug.Log("We are entering the ZoomOutCameraFromWaypoint.");
		StartCoroutine(WaitUntilCameraIsInPosition(CurrentWaypoint.transform.position+offset, OldCameraPosition, CurrentWaypoint));
	}

	private  void helper_MoveFromXtoYSmoothly(Vector3 X, Vector3 Y, GameObject Camera, Waypoint focus){
		Camera.GetComponent<MoveFromXtoY>().FromXtoY(Camera, X, Y, focus);
	}

	IEnumerator WaitUntilCameraIsInPosition(Vector3 CameraPosition, Vector3 OldCameraPosition, Waypoint CurrentWaypoint){
		//Wait until the camera has zoomed in on the waypoint.
		yield return new WaitUntil(()=> MainCamera.transform.position == CameraPosition );
		Debug.Log("Camera is in position.");
		ZoomedIn = true;
		Debug.Log("ZoomedIn is: " + ZoomedIn);
	}

	IEnumerator WaitSeconds(int time, Vector3 OldCameraPosition, Waypoint CurrentWaypoint){
		yield return new WaitForSeconds(time);
		//Once unit combat has completed, trigger camera to begin moving back to it's location.
		Debug.Log("Two seconds have passed.");
		Vector3 offset = new Vector3(1.79628f, 6.673105f ,-7.21338f);
		helper_MoveFromXtoYSmoothly(MainCamera.transform.position, OldCameraPosition, MainCamera, CurrentWaypoint);
		StartCoroutine(ReactivateScripts(OldCameraPosition));
	}

	IEnumerator ReactivateScripts(Vector3 OldCameraPosition){
		yield return new WaitUntil(()=> MainCamera.transform.position == OldCameraPosition );
		Debug.Log("Camera is in position, we are turning on the scripts again.");
		Action_TurnOnCameraScript();
		Action_UnPauseGame();
	}

	//Pause or unpause the game


		/*What actually needs to happen to make this a reality?
			-COMPLETE- Units pause units moving. 
			-COMPLETE- Camera script needs to be turned off. 
			-COMPLETE But needs refactoring- zoom in the camera to the waypoint. 
			


			zoom both attacking and defending sprites to the camera.
			turn off their facethecamera script. 
			pan the camera on the attacking side first.
			Deal damage. 

			pan the camera on the defending side next if retaliate. 
			turn on the facethecamera script for both sprites. 
			move the sprites back to their old position.
			move camera back to it's old position.
			turn camera script back on. 
			unpause units. 

		What info do we need to make this happen?
			Pause/unpause bool for all units. 
			Camera object reference. 
			waypoint object reference. 
			Reference to both attacking and defending sprites. 
			
		What other events might happen in the future that we need to think about scale-wise?
			Rangers have overwatch if no enemy on node, when an enemy goes into their range they should attack without retaliation. 
			Events might be triggered when certain conditions happen. 
			Tactics will be modified by buffs, debuffs and redirections. 
			Cards will be played on units changing their states. 




		*/
}
