using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//TODO: Find a way to pass unit name into combatManger.
public class CombatMainController : MonoBehaviour {
	
	public List<GameObject> CallingUnits;
	public bool CameraZoomedIn;
	private GameObject MainUnit;
	private Waypoint currentNode;
	
	// Use this for initialization
	void Start () {
		CombatCameraController.OnDone += OnCombatStart;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Need to find a way to loop this so long as there is something in the item. 
	//Do we need a pause if we have this list?
	void OnCombatStart(){
		//Grabs the last item in the list, pops it.
		GameObject item = CallingUnits[(CallingUnits.Count - 1)];
		CallingUnits.RemoveAt((CallingUnits.Count - 1));
		MainUnit = item;
		currentNode = MainUnit.GetComponent<UnitCore>().home;
		StartCoroutine(debugwaitAttacker());
	}

	public void SendInformation(GameObject called){
		CallingUnits.Add(called);
	}

	IEnumerator debugwaitAttacker(){
		yield return new WaitForSeconds(0.5f);

		UnitCore thisUnit = MainUnit.GetComponent<UnitCore>();
		Tactic testStrat = thisUnit.selectTactic();
		Debug.Log(thisUnit.name + " now pretends it's doing something to someone.");
		testStrat.onUse(currentNode, thisUnit);
		StartCoroutine(debugwaitDefender());
	}

	IEnumerator debugwaitDefender(){
		yield return new WaitForSeconds(0.5f);
		this.gameObject.GetComponent<CombatCameraController>().CombatEnds();
	}
}
