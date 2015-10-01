using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Note : MonoBehaviour {

	[Range(1, 7)]
	public int pitch;
	[Range(1, 8)]
	public int octave;
	public float speed;
	public float time;

	RectTransform m_Transform;

	void Start()
	{
		m_Transform = GetComponent<RectTransform> ();
		transform.SetParent (GameObject.Find ("DropArea").transform, true);
		if (octave == 8 && pitch > 3)
			pitch = 3;
		if (octave < 1 && pitch < 1) {
			octave = 1;
			pitch = 1;
		}
		Destroy (this.gameObject, time);
	}

	void FixedUpdate()
	{
		m_Transform.anchoredPosition = new Vector2 (m_Transform.anchoredPosition.x,
		                                           m_Transform.anchoredPosition.y - speed);
	}

	public void Clicked()
	{
		//GameManager.instance.CheckAnswer (this);
	}
}
