using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<Player>().Finish();
            Debug.Log("Passed Finish Line!");
        }
        else if (col.tag == "Car")
        {
            col.GetComponent<AICar>().Finish();
        }
    }
}
