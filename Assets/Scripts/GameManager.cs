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

	void Update()
	{
		if (timer == 0 && hasStarted) {
			StopTimer ();
			EndGame ();
		}
	}

	void EndGame ()
	{
		CancelInvoke ("SpawnNotes");
		ClearScreen ();
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
		if (timer <= 10 && !isFlashing)
			InvokeRepeating ("Flash", 0.0f, 0.5f);
		if(timerText)
			timerText.text = minutes + ":" + seconds;
	}

	void Flash()
	{
		if(!isFlashing)
			isFlashing = true;
		if (timerText.color == Color.black)
			timerText.color = Color.red;
		else if (timerText.color == Color.red)
			timerText.color = Color.black;
	}

	public void StopTimer()
	{
		CancelInvoke ("DecreaseTimeRemaining");
		CancelInvoke ("Flash");
		hasStarted = false;
		timer = timerResetNum;
	}

	void SpawnNotes()
	{
		if (spawnCorrectNote <= 0) {
			Spawn (noteToGuess - 1);
			spawnCorrectNote = Random.Range (3, 7);
		} else {
			Spawn(Random.Range (0, noteObjs.Length));
			spawnCorrectNote--;
		}
	}
	
	void Spawn(int index1)
	{
		int posIndex = Random.Range (0, spawners.Length);
		while (posIndex == previousIndex) {
			posIndex = Random.Range (0, spawners.Length);
		}
		previousIndex = posIndex;
		
		Instantiate (noteObjs [index1].gameObject, spawners[posIndex].transform.position,
		             spawners[posIndex].transform.rotation);

	}

	public void CheckAnswer(Note note)
	{
		if (!hasStarted)
			return;

		if (note.pitch == noteToGuess) {
			correctSFX.Play ();
			Debug.Log ("Correct!");
			score += 15;
			UpdateScore ();
			CancelInvoke ("SpawnNotes");
			StartCoroutine (RandomNote ());
			InvokeRepeating ("SpawnNotes", 1, 0.75f);
		} else {
			Debug.Log ("Incorrect!");
			score -= 5;
			UpdateScore();
		}
	}

	void UpdateScore ()
	{
		if (score < 0)
			score = 0;
		scoreText.text = "Score: " + score.ToString ();
	}
	
	IEnumerator RestartGame ()
	{
		yield return new WaitForSeconds (3f);
		Application.LoadLevel (Application.loadedLevel);
	}

	void ClearScreen()
	{
		GameObject[] notesOnScreen = GameObject.FindGameObjectsWithTag ("Note");
		foreach (GameObject go in notesOnScreen)
			Destroy (go);
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
