using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//http://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/
public class Waypoint : MonoBehaviour
{
	public List<Waypoint> neighbors;
	public List<GameObject> Enemies= new List<GameObject>();
	public List<GameObject> Allies = new List<GameObject>();

	public Waypoint previous
	{
		get;
		set;
	}

	public float distance
	{
		get;
		set;
	}

	public void AllyAdd(GameObject ally){
		Allies.Add(ally);
	}

	public void AllyRemove(GameObject ally){
		Allies.Remove(ally);
	}

	public List<GameObject> AllyGetAll(){
		return Allies;
	}

	public void EnemyAdd(GameObject ally){
		Enemies.Add(ally);
	}

	public void EnemyRemove(GameObject ally){
		Enemies.Remove(ally);
	}

	public List<GameObject> EnemyGetAll(){
		return Enemies;
	}

	void OnDrawGizmos()
	{
		if (neighbors == null)
			return;
		Gizmos.color = new Color (0f, 0f, 0f);
	
		foreach(var neighbor in neighbors)
		{
			if (neighbor != null)
				Gizmos.DrawLine (transform.position, neighbor.transform.position);
		}
	}
}
