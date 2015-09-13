using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class FollowWaypoints : MonoBehaviour {

	WaypointCircuit circuit;
	WaypointProgressTracker tracker;
	NavMeshAgent nav;
	public float stoppingDistance = 2;
	public float minWaitTime = 1;
	public float maxWaitTime = 5;
	public Transform[] waypoints;
	public Transform target;
	public bool hit = false;
	int index;
	int previousIndex;
	float waitTime;

	enum WaypointState
	{
		MovingToNextWaypoint,
		WaitingForNextMove
	}

	WaypointState waypointState;

	void Start()
	{
		nav = GetComponent<NavMeshAgent> ();

		if (target == null)
		{
			target = new GameObject(name + " Waypoint Target").transform;
		}
	}

	Vector3 GetCurrentDestination ()
	{
		return nav.destination;
	}

	void UpdateMovingToNextWaypointPoint()
	{
		Vector3 currentDestination = GetCurrentDestination();

		if (nav.remainingDistance < stoppingDistance)
		{
			waitTime = Random.Range(minWaitTime, maxWaitTime);
			waypointState = WaypointState.WaitingForNextMove;
		}
	}
	
	void UpdateWaitingForNextMove ()
	{
		waitTime -= Time.deltaTime;
		if (waitTime < 0.0f)
		{
			ChooseWaypoint();
			waypointState = WaypointState.MovingToNextWaypoint;
		}
	}
	
	void Update()
	{
		if (hit) {
			Destroy (nav);
			Destroy (this);
		}
		if (waypointState == WaypointState.WaitingForNextMove)
			UpdateWaitingForNextMove ();
		else if (waypointState == WaypointState.MovingToNextWaypoint)
			UpdateMovingToNextWaypointPoint ();
	}


	void ChooseWaypoint()
	{
			index = Random.Range (0, waypoints.Length);
			target.position = waypoints [index].position;
			target.rotation = waypoints [index].rotation;
			nav.SetDestination (target.position);
	}
}
