using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//The combat controller commands the camera during fights. 
public class CombatController : MonoBehaviour {

	public Waypoint currentNode;
	public UnitCore innitializerUnit;
	public List<UnitCore> RecivingUnit;

	public bool combatActive;
	public bool zoomedIn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
