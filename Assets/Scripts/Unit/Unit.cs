/*
This scriptable Object contains generic core data for any unit 
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject {
	public string faction;
	public string unitName;
	public Sprite artwork;
	public Sprite portrait;
	public GameObject UI;
	public int attack;
	public int health;
	public int actionPoints;
	//public Tactic[] tactics;
}
