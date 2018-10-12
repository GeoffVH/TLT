using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class CombatMainController : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () {
		CombatCameraController.OnDone += OnCombatStart;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCombatStart(){
		Debug.Log("Combat Manager registers that the camera has arrived in position.");
		this.gameObject.GetComponent<CombatCameraController>().CombatEnds();
	}
}
