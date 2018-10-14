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

	// Use this for initialization
	void Start () {
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
		
		if(thisunit.faction =="Player"){
			this.gameObject.GetComponent<Outline>().color = 3;
			GeneratedUI.transform.SetParent(GameObject.Find("Player Panel").transform, false);
			GetComponent<Movement>().MoveTo(home, "Right", home.Allies.Count, false);
		}		
		if(thisunit.faction =="Enemy"){
			this.gameObject.GetComponent<Outline>().color = 0;
			GeneratedUI.transform.SetParent(GameObject.Find("Enemy Panel").transform, false);
			GetComponent<Movement>().MoveTo(home, "Left", home.Enemies.Count, false);
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

	public void ArriveAt(Waypoint newHome){
		GameObject CombatManager = GameObject.Find("GameManager");
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
	}

	//This function will eventually become huge. Should unitcore hold it or the unit SO itself? 
	public Tactic selectTactic(){
		return thisunit.tactics[Random.Range(0, thisunit.tactics.Count)];
	}

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
 

	/*
	public void startCombat(Waypoint newHome){

		int size = thisunit.tactics.Length;
		int randomPick = Random.Range(0, size);
		thisunit.tactics[randomPick].onUse(newHome, this);
	}
		//TODO: Refactor code because it looks like shit right now.
		//TODO: Impliment disolve effect on removed units. 		
		//TODO: Add camera re-focusing on waypoint when selected.
		//TODO: Add more cool camera shit to do.
		//TODO: Add class lists on the unit code and use it to apply modifiers to units. 
		//TODO: Box Selection.
		//TODO: On hover over UI element, select unit but unselect when off hover.
		//TODO: On clicking over UI element, add unit to selection. 
		 */
	
}
}