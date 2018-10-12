using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This function moves the camera from X to Y and keeps it looking at a given waypoint object. 

//It needs to be attatched to the camera gameobject to be used. 
//It requires a waypoint item that the camera will be looking at as it moves. 
//It requires a vector3 starting point and ending point.
//To use, the FromXtoY function needs to be called. 
public class MoveFromXtoY : MonoBehaviour {

	bool lookat = false;
	Waypoint node;
	Vector3 yOffset = new Vector3(0.0f, 1.0f, 0.0f);

	//We want to keep the camera looking at the same target, but while the script is active there's a good chance the 
	//camera scripts are inactive. This is why we have this lookat function take over. It still needs to be turned off
	//so that when the camera scripts are re-activated they can move the camera and focus on other nodes unheeded. 
	void Update(){
		if(lookat) this.transform.LookAt(node.transform.position+yOffset);
	}

	IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration) {
		lookat = true;
		for (float t=0f; t<duration; t += Time.deltaTime) {
			this.transform.position = Vector3.Lerp(pos1, pos2, t / duration);

			yield return 0;
		}
		this.transform.position = pos2;
		lookat = false;
	}

	IEnumerator LerpFromToWithFocusSet(Vector3 pos1, Vector3 pos2, float duration) {
		lookat = true;
		for (float t=0f; t<duration; t += Time.deltaTime) {
			this.transform.position = Vector3.Lerp(pos1, pos2, t / duration);

			yield return 0;
		}
		this.transform.position = pos2;
		lookat = false;
	}

	public void FromXtoY(Vector3 start, Vector3 end){
		StartCoroutine(LerpFromToWithFocusSet(start, end, 2f) );
	}

	public void FromXtoY(Vector3 start, Vector3 end, Waypoint focus){
		node = focus;
		StartCoroutine(LerpFromTo(start, end, 0.4f) );
	}
}
