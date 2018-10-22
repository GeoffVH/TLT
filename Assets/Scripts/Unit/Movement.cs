/*
Handles unit movement and calls for the unit's combat suite on reaching the new node.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cakeslice
{

public class Movement : MonoBehaviour {

	public float speed = 7.5f;
	Vector3 target;
	public ParticleSystem ps;
	private float placingX;
	private float placingZ;

	void Start(){
		target = transform.position;
	}

    void Update()
    {
        // Move our position a step closer to the target, If the unit is not paused.
		if(transform.position != target && GetComponent<UnitCore>().isPaused == false){
			ps.Play();
        	transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
		}
		else{
			ps.Stop();
			//transform.position = Vector3.MoveTowards(transform.position, target, speed * 0);
		}
    }

	//String side must be either Right or Left
	//All nodes have "Slots", roughly a small zone where the unit could end at. 
	//These slots stop units from piling on top of each other by accident. 
	public void MoveTo(Waypoint home, string side, int SlotRange, bool startCombat){
		if(SlotRange == 0){
			placingX = Random.Range(0,0f);
			placingZ = Random.Range(0f,0f);
		}
		if(SlotRange == 1){
			placingX = Random.Range(2,2.5f);
			placingZ = Random.Range(0.5f,1f);
		} 
		if(SlotRange == 2){
			placingX = Random.Range(2.3f,3f);
			placingZ = Random.Range(2f,2.5f);
		}
		if(SlotRange == 3){
			placingX = Random.Range(2.3f,3.5f);
			placingZ = Random.Range(-1.5f,-0.5f);
		}
		if(SlotRange == 4){
			placingX= Random.Range(3.9f,4.2f);
			placingZ = Random.Range(0.5f,1f);
		} 
		if(SlotRange == 5){
			placingX= Random.Range(3.9f,4.4f);
			placingZ = Random.Range(2f,2.5f);
		} 	
		if(SlotRange == 6){
			placingX= Random.Range(4.1f,4.3f);
			placingZ = Random.Range(-1.5f,-0.5f);
		}
		if(SlotRange >= 7){
			placingX= Random.Range(2.5f,4.4f);
			placingZ = Random.Range(-1.5f,2.5f);
		} 					

		if(side == "Right"){			
			Vector3 newtarget = new Vector3(home.transform.position.x+placingX, home.transform.position.y+0.7879867f, home.transform.position.z+placingZ);
			target = newtarget;
			if(startCombat) StartCoroutine(WaitTillArrival(newtarget, home)); 
		}
		if(side=="Left"){
			Vector3 newtarget = new Vector3(home.transform.position.x-placingX, home.transform.position.y+0.7879867f, home.transform.position.z-placingZ);
			target = newtarget;
			if(startCombat) StartCoroutine(WaitTillArrival(newtarget, home)); 
		}
	}
	
	IEnumerator WaitTillArrival(Vector3 newtarget, Waypoint home){
		yield return new WaitUntil(() => this.transform.position == newtarget);
		GetComponent<UnitCore>().ArriveAt(home);
	}
}
}