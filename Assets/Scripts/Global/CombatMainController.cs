using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//TODO: Find a way to pass unit name into combatManger.
public class CombatMainController : MonoBehaviour {
	
	public List<GameObject> CallingUnits;
	public bool CameraZoomedIn;
	
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
		Debug.Log(item.name + " now pretends it's doing something to someone.");
		StartCoroutine(debugwait());
	}

	public void SendInformation(GameObject called){
		//Debug.Log("Unit has sent it's information as " + called.name);
		CallingUnits.Add(called);
	}

	IEnumerator debugwait(){
		yield return new WaitForSeconds(1);
		this.gameObject.GetComponent<CombatCameraController>().CombatEnds();
	}
}
