/*
This script handles selection and movement based on the mouse.  
It contains a list of all currently selected units. 
It will call unit movement. 
It will add and remove units at waypoints.

This really needs to get refactored. 
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cakeslice
{
	public class OnSelect : MonoBehaviour {


		public GameObject CameraOrbit;
		public GameObject CameraFocus;
		public GameObject TheCamera;
		private UnitCore display;

		[SerializeField]
		private LayerMask Clickable;

		[SerializeField]
		private LayerMask Moveable;

		private List<GameObject> selected = new List<GameObject>(); 

		public float unitSpeed = 3f;

		IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration, GameObject movedObject) {

			for (float t=0f; t<duration; t += Time.deltaTime) {
				movedObject.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
				yield return 0;
			}
			movedObject.transform.position = pos2;
		}




		void Update()
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;

			//Modify the selected list. 
			if( Input.GetMouseButtonDown(0) )
			{

				//if click on a waypoint, move camera to that waypoint
				if( Physics.Raycast( ray, out hit, Mathf.Infinity, Moveable) ){
					Debug.Log("We clicked a waypoint");
					GameObject node = hit.transform.gameObject;
					Vector3 yOffset = new Vector3(0.0f, 1.0f, 0.0f);
					StartCoroutine(LerpFromTo(CameraOrbit.transform.position, node.transform.position, 1f, CameraOrbit) );
					StartCoroutine(LerpFromTo(CameraFocus.transform.position, node.transform.position+yOffset, 1f, CameraFocus) );
				}

				//if click hits a selectable unit, add it to the list of selected units.
				if( Physics.Raycast( ray, out hit, Mathf.Infinity, Clickable ) )
				{
					GameObject target = hit.transform.gameObject;
					display = target.GetComponent<UnitCore>();
					if(!display.isSelected){
						target.GetComponent<Outline>().enabled = true;
						selected.Add(target);
						display.isSelected = true;
					}
					
				}

				//Unselect all units out there.
				else
				{
					foreach(GameObject item in selected){
						item.GetComponent<Outline>().enabled = false;
						item.GetComponent<UnitCore>().isSelected = false;
					}
					selected.Clear();
				}
			}

			//Move units to right clicked node. 
			if (Input.GetMouseButtonDown(1)){

				if( Physics.Raycast( ray, out hit, Mathf.Infinity, Moveable ) )
				{
					GameObject node = hit.transform.gameObject;
					Vector3 yOffset = new Vector3(0.0f, 1.0f, 0.0f);
					StartCoroutine(LerpFromTo(CameraOrbit.transform.position, node.transform.position, 1f, CameraOrbit) );
					StartCoroutine(LerpFromTo(CameraFocus.transform.position, node.transform.position+yOffset, 1f, CameraFocus) );
					//Can do a check for each unit to see if their home node has a waypoint connected to the node.
					
					//Detatch each troop from the previous node.
					//Attatch each troop to the new node.
					//Move each troop selected to the new node.
					//Toggle off highlight
					//Deselect all units, and clear the list.
					StartCoroutine(SetupLine(node, true));
					
				}
			}
		}

		//This function is a total mess and needs some polish and refactoring - but it works just well enough :D
		//This works like a hobbled together foreach loop, going backwards in selected. 
		IEnumerator SetupLine(GameObject node, bool firstloop) {

			float randomizedTiming = Random.Range(0.2f, 0.3f);
			if(firstloop) yield return new WaitUntil(()=> firstloop == true);
			else yield return new WaitForSeconds(randomizedTiming);
			

			if(selected.Count > 0){
				
				GameObject troop = selected[selected.Count - 1];
				Waypoint oldHome = new GetClosestWaypoint().search(troop.transform.position);
				Waypoint newHome = node.GetComponent<Waypoint>();
				Unit troopInfo = troop.GetComponent<UnitCore>().thisunit;
				troop.GetComponent<Outline>().enabled = false;
				troop.GetComponent<UnitCore>().isSelected = false;

				if(oldHome != newHome){

					if(troopInfo.faction == "Player"){
						oldHome.AllyRemove(troop);
						newHome.AllyAdd(troop);
						troop.GetComponent<Movement>().MoveTo(newHome, "Right", newHome.Allies.Count, true);
					}

					if(troopInfo.faction == "Enemy"){
						oldHome.EnemyRemove(troop);
						newHome.EnemyAdd(troop);
						troop.GetComponent<Movement>().MoveTo(newHome, "Left", newHome.Enemies.Count, true);
					}
				}
				if(selected.Count == 1) selected.RemoveAt(selected.Count-1);
				else{
					selected.RemoveAt(selected.Count-1);
					StartCoroutine(SetupLine(node, false));
				}
			}
		
		}
	}
}
