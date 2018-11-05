/*
Handles most functions relating to a unit. 
Many different events will call functions in this script.
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace cakeslice
{

public class UnitCore : MonoBehaviour {

	public Unit thisunit;
	[HideInInspector] public bool isSelected;
	[HideInInspector] public int health;
	[HideInInspector] public int actionPoints;
	[HideInInspector] public GameObject GeneratedUI;
	[HideInInspector] public string faction;
	private Sprite artworkImage;
	[HideInInspector] public bool isPaused;
	public AnimationCurve fadeIn;
	GameObject PS_Flakes;
	ParticleSystem PS;
	bool isDead = false;
	Renderer _renderer;
	int shaderProperty;
	public Waypoint home;
	private	GameObject CombatManager;

	// Use this for initialization
	void Start () {
		CombatManager = GameObject.Find("GameManager");
		isPaused = false;
		_renderer = GetComponent<Renderer>();
		PS_Flakes = this.gameObject.transform.Find("DeathEvent").gameObject;
		PS = PS_Flakes.GetComponentInChildren<ParticleSystem>();
		shaderProperty = Shader.PropertyToID("_cutoff");
		artworkImage = thisunit.artwork;
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = artworkImage;
		this.gameObject.GetComponent<Outline>().enabled = false;
		health = thisunit.health;
		faction = thisunit.faction;
		actionPoints = thisunit.actionPoints;
		isSelected = false;
		GeneratedUI = Instantiate(thisunit.UI, new Vector3 (0,0,0), Quaternion.identity);
		GeneratedUI.GetComponent<LinkDebugToUnit>().HealthChanged(this);
		GeneratedUI.GetComponent<LinkDebugToUnit>().ColorFlash(Color.black);
		GeneratedUI.GetComponentInChildren<Image>().sprite = thisunit.portrait;		
		home = AssignToWaypoint();
		SetFactionSpecificItems();
		FilterTactics();
	}

	//initializes items specific to either player or enemy units. 
	void SetFactionSpecificItems(){
		if(thisunit.faction =="Player"){
			this.gameObject.GetComponent<Outline>().color = 3;
			GeneratedUI.transform.SetParent(GameObject.Find("Player Panel").transform, false);
			GetComponent<Movement>().MoveTo(home, "Right", home.Allies.Count);
		}		
		if(thisunit.faction =="Enemy"){
			this.gameObject.GetComponent<Outline>().color = 0;
			GeneratedUI.transform.SetParent(GameObject.Find("Enemy Panel").transform, false);
			GetComponent<Movement>().MoveTo(home, "Left", home.Enemies.Count);
		}
	}

	void update(){
		if(isDead){
			PS.Play();
			_renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate( Mathf.InverseLerp(0, 2, 1)));
		}
	}

	public Waypoint AssignToWaypoint(){
		Waypoint home = new GetClosestWaypoint().search(this.transform.position);
		if(thisunit.faction == "Player") home.Allies.Add(this.gameObject);
		if(thisunit.faction == "Enemy") home.Enemies.Add(this.gameObject);
		return home;
	}

	//Meant to be called when a unit reaches a new waypoint. 
	//This kickstarts all combat.
	//May be depreciated. 
	public void ArriveAt(Waypoint newHome){
		if(thisunit.faction == "Player" && newHome.Enemies.Count > 0 && isPaused == false) {
			//Debug.Log(this.name + " is sending information to the combat controller now.");
			CombatManager.GetComponent<CombatMainController>().SendInformation(this.gameObject);
			CombatManager.GetComponent<CombatCameraController>().CombatStart(newHome);
		}
		if(thisunit.faction == "Enemy" && newHome.Allies.Count > 0 && isPaused == false) {
			//Debug.Log(this.name + " is sending information to the combat controller now.");
			CombatManager.GetComponent<CombatMainController>().SendInformation(this.gameObject);
			CombatManager.GetComponent<CombatCameraController>().CombatStart(newHome);
		}
		home = newHome;
	}



	public List<Tactic> Supporting;
	public List<Tactic> Attacking;

	//Goes through the unit's tactics and filters them into supporting or attacking lists. 
	public void FilterTactics(){
		foreach(Tactic item in thisunit.tactics){
			if (item.isSupport) Supporting.Add(item);
			else Attacking.Add(item);
		}
	}

	//Randomly selects a unit from a list of units. 
	public GameObject selectFrom(List<GameObject> targets){
		return targets[Random.Range(0, targets.Count)];
	}

	//Randomly selects a tactic from a given list of tactics 
	public Tactic get_randomTactic(List<Tactic> items){
		return items[Random.Range(0, items.Count)];
	}

	//Randomly selects a supporting tactic from the unit's list of tactics. 
	public Tactic get_RandomSupportTactic(){
		return Supporting[Random.Range(0, Supporting.Count)];
	}

	//Randomly selects an attacking tactic from the unit's list of tactics. 
	public Tactic get_RandomAttackTactic(){
		return Attacking[Random.Range(0, Attacking.Count)];
	}

	//Gets a list of units on the current node that are opposed to this unit.
	public List<GameObject> Get_EnemyList(){
		if(thisunit.faction == "Player") return home.Enemies;
		else return home.Allies;
	}

	//Gets a list of units on the current node that are allied to the unit, excluding the unit itself. 
	public List<GameObject> Get_AllyList(){
		List<GameObject> clone = new List<GameObject>();
		if(thisunit.faction == "Player") clone.AddRange(home.Allies);
		else clone.AddRange(home.Enemies);

		clone.Remove(gameObject);
		return clone;
	}

	//TargettingPackage holds a gameobject list containing all info unit might need. 
	public struct targettingPackage{
		public GameObject TargetList;
		public Tactic SelectedTactic;

		public targettingPackage(GameObject Target, Tactic tactic){
			TargetList = Target;
			SelectedTactic = tactic;
		}
	}

	//Organizes and returns a package for the combat manager's use. 
	public targettingPackage getTargetandTactic(){ 
		Tactic chosenTactic = DecideOnTactic();
		GameObject chosenTarget = DecideOnTarget(chosenTactic);
		return new targettingPackage(chosenTarget, chosenTactic);
	}

	//Returns a tactic that the unit chooses to use.
	//Unit should be able to pick from all it's tactics, albit with weighting. 
	//Condition: Support tactics can't be used if there are no supporting units. 
	//Condition: Attacking tactics can't be used if there are no enemy units. 
	public Tactic DecideOnTactic(){

		List<Tactic> WeightedList = new List<Tactic>();
		WeightedList.AddRange(Attacking);
		WeightedList.AddRange(Supporting);
		if(thisunit.unitClass == "Support"){
			WeightedList.AddRange(Supporting);
			WeightedList.AddRange(Supporting);
		}
		else{
			WeightedList.AddRange(Attacking);
			WeightedList.AddRange(Attacking);
		}
		
		if(Get_AllyList().Count > 0 && Get_EnemyList().Count > 0){
			return WeightedList[Random.Range(0, WeightedList.Count)];
		}
		else{
			if(Get_AllyList().Count > 0 &&  Get_EnemyList().Count == 0){
				return get_RandomSupportTactic();
			}
			else{
				return get_RandomAttackTactic();
			}
		} 
	}

	//Returns a target that the unit chooses to use. 
	public GameObject DecideOnTarget(Tactic chosenTactic){
		GameObject SelectedTarget;
		if(chosenTactic.isSupport){
			SelectedTarget = selectFrom(Get_AllyList());
		}
		else{
			SelectedTarget = selectFrom(Get_EnemyList());
		}
		return SelectedTarget;
	}

	//Removes this unit from play
	public void death(){
		if(this.gameObject != null){
			Waypoint home = new GetClosestWaypoint().search(this.transform.position);
			if(thisunit.faction == "Player") home.Allies.Remove(this.gameObject);
			if(thisunit.faction == "Enemy") home.Enemies.Remove(this.gameObject);
			Destroy(GeneratedUI);
			PS.Play();
			isDead = true;
			StartCoroutine(DeleteObject());
			StartCoroutine(StopParticles());
			StartCoroutine(FadeTo(0.0f, 1.0f));
		}
	}

	IEnumerator DeleteObject(){
		yield return new WaitForSeconds(2.5f);
		Destroy(this.gameObject);
	}

	IEnumerator StopParticles(){
		yield return new WaitForSeconds(1f);
		PS.Stop();
	}

	IEnumerator FadeTo(float aValue, float aTime)
     {
         float alpha = transform.GetComponent<SpriteRenderer>().material.color.a;
         for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
         {
             Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
             transform.GetComponent<SpriteRenderer>().material.color = newColor;
             yield return null;
         }
     }
}
}