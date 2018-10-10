using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFromXtoY : MonoBehaviour {

	bool lookat = false;
	Waypoint node;

	void Update(){
		if(lookat) this.transform.LookAt(node.transform.position);
	}

	IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration, GameObject movedObject) {
		lookat = true;
		for (float t=0f; t<duration; t += Time.deltaTime) {
			movedObject.transform.position = Vector3.Lerp(pos1, pos2, t / duration);

			yield return 0;
		}
		movedObject.transform.position = pos2;
		lookat = false;
	}

	public void FromXtoY(GameObject item, Vector3 start, Vector3 end, Waypoint focus){
		node = focus;
		StartCoroutine(LerpFromTo(start, end, 0.4f, item) );
	}
}
