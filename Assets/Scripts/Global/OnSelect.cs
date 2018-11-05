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
		private UnitCore selectedUnit;
		[HideInInspector] public bool isCombatTriggered;
		[SerializeField] private LayerMask Clickable;
		[SerializeField] private LayerMask Moveable;

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
					selectedUnit = target.GetComponent<UnitCore>();
					if(!selectedUnit.isSelected){
						target.GetComponent<Outline>().enabled = true;
						selected.Add(target);
						selectedUnit.isSelected = true;
					}
				}

				//Deselect all units out there if the click is not a unit or a waypoint.
				else
				{
					foreach(GameObject item in selected){
						item.GetComponent<Outline>().enabled = false;
						item.GetComponent<UnitCore>().isSelected = false;
					}
					selected.Clear();
				}
			}

			//If the right click is on a waypoint, move all units currently selected to that waypoint.
			if (Input.GetMouseButtonDown(1)){
				if( Physics.Raycast( ray, out hit, Mathf.Infinity, Moveable ) )
				{
					GameObject node = hit.transform.gameObject;
					Waypoint DestinationWaypoint = node.GetComponent<Waypoint>();
					MoveCameraToWaypoint(node);
					MoveAllUnitsTo(DestinationWaypoint);
					StartCoroutine(BufferThenSendSelectedList());
				}
			}
		}

		//A small fraction of a second is needed before units are moving.
		//If we started SendSelectedList() first, that small fraction would be a false positive.
		IEnumerator BufferThenSendSelectedList(){

			const float waitTime = 0.3f;
			float counter = 0f;
			while (counter < waitTime)
			{
				counter += Time.deltaTime;
				yield return null; //Don't freeze Unity
			}
			StartCoroutine(SendSelectedList());
		}

		//This coroutine waits until all units in the selected list have arrived.
		//It then sends off a copy of the selected array, before cleaning house. 
		IEnumerator SendSelectedList(){
			yield return new WaitUntil(()=> selected.TrueForAll(CheckUnitIsNotMoving));
			foreach(GameObject item in selected){
				item.GetComponent<Outline>().enabled = false;
				item.GetComponent<UnitCore>().isSelected = false;
			}
			//TODO: Send Selected array to combat manager HERE 
			selected.Clear();
		}

		private static bool CheckUnitIsNotMoving(GameObject s)
		{
			return !s.GetComponent<Movement>().isMoving;
		}

		private void MoveAllUnitsTo(Waypoint DestinationWaypoint){
			foreach(GameObject item in selected){
				MoveUnitTo(item, DestinationWaypoint);
			}
		}

		private void MoveUnitTo(GameObject troop, Waypoint newHome){
			Waypoint oldHome = new GetClosestWaypoint().search(troop.transform.position);
			Unit Core = troop.GetComponent<UnitCore>().thisunit;
			if(Core.faction == "Player"){
				oldHome.AllyRemove(troop);
				newHome.AllyAdd(troop);
				troop.GetComponent<Movement>().MoveTo(newHome, "Right", newHome.Allies.Count);
			}

			if(Core.faction == "Enemy"){
				oldHome.EnemyRemove(troop);
				newHome.EnemyAdd(troop);
				troop.GetComponent<Movement>().MoveTo(newHome, "Left", newHome.Enemies.Count);
			}	
		}

		private void MoveCameraToWaypoint(GameObject node){
			Vector3 yOffset = new Vector3(0.0f, 1.0f, 0.0f);
			StartCoroutine(LerpFromTo(CameraOrbit.transform.position, node.transform.position, 1f, CameraOrbit) );
			StartCoroutine(LerpFromTo(CameraFocus.transform.position, node.transform.position+yOffset, 1f, CameraFocus) );
		}
	}
}
