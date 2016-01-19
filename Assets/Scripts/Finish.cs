using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {

    public BoxCollider2D _collider;
    public bool playerCol;

    float width;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        width = _collider.size.x;
    }

    void Update()
    {
        if (!playerCol)
            return;
        if (player.position.x < transform.position.x - width / 2)
            _collider.isTrigger = true;
        else if (player.position.x > transform.position.x + width / 2)
            _collider.isTrigger = false;
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<Player>().Finish();
        }
        else if (col.tag == "Car")
        {
            col.GetComponent<AICar>().Finish();
        }
    }
}
