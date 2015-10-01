using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class AICar : MonoBehaviour {

    public float moveSpeed;
    public float stoppingDistance;
    public float rotateSpeed;
    public Transform target;

    public int place;
    public int currentLap;
    public int currentWaypoint;
    [HideInInspector]
    public Vector2 distanceToWaypoint;

    WaypointCircuit waypointCircuit;
    Transform[] waypoints;
    Rigidbody2D rb;
    Vector2 targetPos;
    bool finished;
    bool hasStarted = false;
    public int laps;

    void Start()
    {
        waypointCircuit = GameObject.FindGameObjectWithTag("WaypointCircuit").GetComponent<WaypointCircuit>();
        waypoints = waypointCircuit.Waypoints;
        rb = GetComponent<Rigidbody2D>();
        currentWaypoint = 0;
        target = waypoints[currentWaypoint];
        finished = false;
        laps = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().laps;
        hasStarted = false;
    }

    public void Finish()
    {
        if (currentLap >= laps)
        {
            finished = true;
        }
        else
        {
            currentLap++;
        }
    }

    public void StartGame()
    {
        hasStarted = true;
    }

    void Update()
    {
        if (!hasStarted)
            return;
        if (finished)
        {
            transform.rotation = Quaternion.identity;
            //rb.velocity = Vector2.zero;
            return;
        }
        targetPos = (Vector2) target.position;
        distanceToWaypoint = targetPos - (Vector2)transform.position;
        if(distanceToWaypoint.magnitude <= stoppingDistance)
        {
            NextWaypoint();
            return;
        }

        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.AddForce(gameObject.transform.right * moveSpeed);
    }

    void NextWaypoint()
    {
        try
        {
            if (currentWaypoint < waypoints.Length)
                currentWaypoint++;
            else if (currentWaypoint >= waypoints.Length - 1)
                currentWaypoint =0;
            target = waypoints[currentWaypoint];
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log("Caught Exception");
        }

    }
}
