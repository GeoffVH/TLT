using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GetClosestWaypoint {

	public Waypoint search(Vector3 target)
	{
		GameObject closest = null;
		float closestDist = Mathf.Infinity;
		foreach (var waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
		{
			var dist = (waypoint.transform.position - target).magnitude;
			if (dist < closestDist)
			{
				closest = waypoint;
				closestDist = dist;
			}
		}
		if (closest != null)
		{
			return closest.GetComponent<Waypoint> ();
		}
		return null;
	}
}
