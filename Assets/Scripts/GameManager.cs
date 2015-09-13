using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public class Tuple<A,B> {
		public A a;
		public B b;
		
		public Tuple(A a, B b) {
			this.a = a;
			this.b = b;
		}

		public bool IsEqualTo(Tuple<A, B> other)
		{
			if (this.a.Equals (other.a) && this.b.Equals (other.b))
				return true;
			else
				return false;
		}
	}

	public GameObject[] spawners;
	public RectTransform[] noteObjs;
	public int noteToGuess;
	public AudioSource correctSFX;
	public Text timerText;
	public Text scoreText;
	public int score;
	public float timer;
	public float timerResetNum;
	public AudioSource[] notes;
	
	int spawnCorrectNote;
	int previousIndex = 1;
	bool hasStarted;
	bool isFlashing;

	public static GameManager instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);

		StartUp ();
	}

	void StartUp()
	{
		hasStarted = false;
		spawners = GameObject.FindGameObjectsWithTag ("Spawner");
		score = 0;
	}

	public void StartGame()
	{
		StartCoroutine (RandomNote ());
		spawnCorrectNote = Random.Range (3, 7);
		InvokeRepeating ("SpawnNotes", 1, 0.75f);
		StartTimer (timerResetNum);
	}

	public void StartTimer(float time)
	{
		timer = time;
		timerResetNum = timer;
		InvokeRepeating ("DecreaseTimeRemaining", 1.0f, 1.0f);
		hasStarted = true;
	}

	void DecreaseTimeRemaining()
	{
		timer--;
		UpdateText ();
	}

	void UpdateText()
	{
		string minutes = Mathf.Floor (timer / 60).ToString ("00");
		string seconds = Mathf.Floor (timer % 60).ToString ("00");
		
		if(timerText)
			timerText.text = minutes + ":" + seconds;
	}

	public void StopTimer()
	{
		CancelInvoke ("DecreaseTimeRemaining");
		CancelInvoke ("Flash");
		hasStarted = false;
		timer = timerResetNum;
	}

	public void CheckAnswer(Note note)
	{
		if (!hasStarted)
			return;

		if (note.pitch == noteToGuess) {
			correctSFX.Play ();
			Debug.Log ("Correct!");
			score += 15;

			CancelInvoke ("SpawnNotes");
			StartCoroutine (RandomNote ());
			InvokeRepeating ("SpawnNotes", 1, 0.75f);
		} else {
			Debug.Log ("Incorrect!");
			score -= 5;

		}
	}
	
	IEnumerator RestartGame ()
	{
		yield return new WaitForSeconds (3f);
		Application.LoadLevel (Application.loadedLevel);
	}

	IEnumerator RandomNote()
	{
		noteToGuess = Random.Range (1, 8);
		yield return new WaitForSeconds (1);
		notes [noteToGuess - 1].Play ();
		//		int b = Random.Range (1, 9);
		
		//		if (b == 8 && a > 3)
//			a = 3;
//		Tuple<int, int> ab = new Tuple<int, int> (a, b);
//		return ab;
//		return a;
	}
}
