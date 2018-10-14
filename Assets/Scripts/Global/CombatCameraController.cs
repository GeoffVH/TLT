using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//This combat controller commands the camera during fights. 
//Also pauses and unpauses the game. 
//This script must be attatched in the same places as the CombatMainController
public class CombatCameraController : MonoBehaviour {

	public Waypoint currentNode;
	public Vector3 oldCameraPosition;
	public Vector3 offset;
	public Vector3 target;
	public GameObject MainCamera;
	public GameObject EnemyUnits;
	public GameObject PlayerUnits;

	public delegate void CameraZoomedIn();
	public static event CameraZoomedIn OnDone;


	void Start(){
		MainCamera = GameObject.Find("Main Camera");
		EnemyUnits = GameObject.Find("Enemy Units");
		PlayerUnits = GameObject.Find("Player Units");
		offset = new Vector3(1.79628f, 6.673105f ,-7.21338f);
		target = new Vector3(0,0,0);
	}

	// TODO: change this to a unity event maybe? 
	void Update () {
		
	}

	private void DoWePauseGame(bool flag){
		foreach (Transform child in EnemyUnits.transform){
			child.GetComponent<UnitCore>().isPaused = flag;
			//Debug.Log("We have paused " + child.GetComponent<UnitCore>().name);
		}

		foreach (Transform child in PlayerUnits.transform){
			child.GetComponent<UnitCore>().isPaused = flag;
			//Debug.Log("We have paused " + child.GetComponent<UnitCore>().name);
		}

		if(flag){
			Debug.Log("Game Paused.");
		}
		else{
			Debug.Log("Game Unpaused.");
		}
	}

	private  void setCameraScriptsTo(bool flag){
		MainCamera.GetComponent<CameraController>().enabled = flag;
	}

	//Unit calls up the camera to move. 
	//Step 1: Pause the camera scripts.
	//Step 2: Pause the game. 
	//Step 3: Save the current camera position
	//Step 4: Zoom into the node.
	//Step 5: Run a courtine and then send a unity event when position == target??
	public void CombatStart(Waypoint node){
		setCameraScriptsTo(false);
		DoWePauseGame(true);
		oldCameraPosition = MainCamera.transform.position;
		target = node.transform.position + offset;
		MainCamera.GetComponent<MoveFromXtoY>().FromXtoY(oldCameraPosition, target, node);
		StartCoroutine(WaitforCamera());
	}

	IEnumerator WaitforCamera(){
		yield return new WaitUntil(()=> MainCamera.transform.position == target );
		//Debug.Log("Camera is in position.");
		if(OnDone != null){
			OnDone();
		}
	}

	IEnumerator WaitforCameraToReturn(){
		yield return new WaitUntil(()=> MainCamera.transform.position == oldCameraPosition );
		Debug.Log("Camera is in position.");
		setCameraScriptsTo(true);
		DoWePauseGame(false);
	}

	//Unit is done fighitng, calls up the camera to move things back to normal. 
	//Step 1: Zoom out of the node to the old camera position.
	//Step 2: Unpause the game. 
	//Step 3: Reactivate the camera scripts. 
	public void CombatEnds(){
		
		MainCamera.GetComponent<MoveFromXtoY>().FromXtoY(MainCamera.transform.position, oldCameraPosition);
		StartCoroutine(WaitforCameraToReturn());
	}
}
