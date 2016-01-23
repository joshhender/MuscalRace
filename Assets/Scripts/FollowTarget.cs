using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float dampTime = 0.15f;
    public bool smooth;

    Vector3 velocity = Vector3.zero;

    float normSpeed;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target)
        {
            transform.position = target.position + offset;
        }
    }

    void FixedUpdate()
    {
        if (smooth)
        {
            if (transform.position != target.position + offset)
            {
                Vector3 destination = Vector3.SmoothDamp(transform.position,
                                                          target.position + offset,
                                                          ref velocity,
                                                          dampTime);
                transform.position = new Vector3(destination.x,
                                                 destination.y,
                                                 transform.position.z);
            }
        }
    }
}
