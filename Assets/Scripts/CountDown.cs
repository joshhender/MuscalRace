using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountDown : MonoBehaviour
{
    public GameObject howTo;

    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        text.text = "3";
        yield return new WaitForSeconds(1);
        text.text = "2";
        yield return new WaitForSeconds(1);
        text.text = "1";
        yield return new WaitForSeconds(1);
        text.text = "Go!";
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            go.SendMessage("StartGame", SendMessageOptions.DontRequireReceiver);
        }
        howTo.SetActive(false);
        yield return new WaitForSeconds(1);
        text.text = "";
    }
}
