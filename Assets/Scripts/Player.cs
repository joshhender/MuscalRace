using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class Player : MonoBehaviour {

    public float moveSpeed;
    public float turnSpeed;
    public float accelerationSpeed;
    public int laps;
    public Text lapsText;
    public Text placeText;
    public bool finished;

    public int place;
    public int currentLap;
    public int currentWaypoint;
    [HideInInspector]
    public Vector2 distanceToWaypoint;

    Rigidbody2D rb;
    WaypointCircuit waypointCircuit;
    Transform[] waypoints;
    Vector2 targetPos;
    Transform target;
    bool hasStarted = false;
    static float stoppingDistance = 2;

    public void Finish()
    {
        if (currentLap >= laps)
        {
            finished = true;
            Debug.Log("Finished!");
        }
        else{ 
            currentLap++;
            lapsText.text = string.Format("Lap {0}/{1}", currentLap.ToString(), laps.ToString());
        }
    }

    public void StartGame()
    {
        hasStarted = true;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lapsText = GameObject.Find("Laps").GetComponent<Text>();
        lapsText.text = string.Format("Lap 1/{0}", laps.ToString());
        placeText = GameObject.Find("Place").GetComponent<Text>();
        placeText.text = "Place 1/5";
        finished = false;
        waypointCircuit = GameObject.FindGameObjectWithTag("WaypointCircuit").GetComponent<WaypointCircuit>();
        waypoints = waypointCircuit.Waypoints;
        currentWaypoint = 0;
        target = waypoints[currentWaypoint];
        hasStarted = false;
    }

    void FixedUpdate()
    {
        if (finished || !hasStarted)
            return;
        if (!Input.GetMouseButton(0))
            return;
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.magnitude.MustNotBeApproximatelyEqual(transform.position.magnitude);

        Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition,
                                                 Vector3.forward);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        rb.angularVelocity = 0;

        rb.AddForce(gameObject.transform.up * moveSpeed);

        targetPos = (Vector2)target.position;
        distanceToWaypoint = targetPos - (Vector2)transform.position;
        if (distanceToWaypoint.magnitude <= stoppingDistance)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        if (currentWaypoint < waypoints.Length - 1)
            currentWaypoint++;
        else if (currentWaypoint >= waypoints.Length - 1)
            currentWaypoint = 0;
        target = waypoints[currentWaypoint];

    }

    public void UpdateText()
    {
        placeText.text = string.Format("Place {0}/5", place.ToString());
    }
}
