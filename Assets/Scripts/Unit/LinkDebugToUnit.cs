/*
Sets the UI panel this script is attatched with to display the targetUnit's health
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using cakeslice;

public class LinkDebugToUnit : MonoBehaviour {

	private TextMeshProUGUI OurText;
	
	public void HealthChanged (UnitCore thisUnit) {
		OurText = GetComponentInChildren<TextMeshProUGUI>();
		OurText.text = thisUnit.health.ToString();
	}

	public void ColorFlash(Color pick){
		StartCoroutine(flashColor(pick));
	}

	IEnumerator flashColor(Color pick){
		//OurText.faceColor = new Color32(255, 128, 0, 255);
		OurText.faceColor = pick;
		yield return new WaitForSeconds(1);
		OurText.faceColor = Color.black;
	}
}
