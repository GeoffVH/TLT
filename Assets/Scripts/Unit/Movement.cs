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
	public bool isMoving;

	void Start(){
		target = transform.position;
		isMoving = false;
	}

    void Update()
    {
        //if the unit is not paused and not currently standing on it's home position, it will move. 
		if(transform.position != target && GetComponent<UnitCore>().isPaused == false){
			ps.Play();
        	transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			isMoving = true;
		}
		else{
			ps.Stop();
			isMoving = false;
		}
    }

	//All nodes have "Slots", roughly a small zone where the unit could end at. 
	//These slots stop units from piling on top of each other by accident.
	//String Side needs to be either "Right" or "Left", which will funnel the unit to said side of the node. 
	private float placingX;
	private float placingZ;
	public void MoveTo(Waypoint home, string side, int SlotRange){
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
			//StartCoroutine(WaitTillArrival(newtarget, home)); 
		}
		if(side=="Left"){
			Vector3 newtarget = new Vector3(home.transform.position.x-placingX, home.transform.position.y+0.7879867f, home.transform.position.z-placingZ);
			target = newtarget;
			//StartCoroutine(WaitTillArrival(newtarget, home)); 
		}
	}
	
	//Old combat system. 
	IEnumerator WaitTillArrival(Vector3 newtarget, Waypoint home){
		yield return new WaitUntil(() => this.transform.position == newtarget);
		GetComponent<UnitCore>().ArriveAt(home);
	}
}
}