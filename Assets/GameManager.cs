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

	public Tuple<int, int> currentNote = new Tuple<int, int>(0, 0);
	public Tuple<int, int> noteToGuess = new Tuple<int, int>(0, 0);
	public Text currentNoteText;
	public Text noteToGuessText;
	public Button goUpBtn;
	public Button goDownBtn;
	public GameObject checkAnswerBtn;
	public GameObject startBtn;
	public GameObject correctText;
	public GameObject incorrectText;

	string[] notes = new string[]{"A", "B", "C", "D", "E", "F", "G"};


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
		startBtn.SetActive (true);
		checkAnswerBtn.SetActive (false);
		correctText.SetActive (false);
		incorrectText.SetActive (false);
	}

	public void StartGame()
	{
		noteToGuess = RandomNote ();
		currentNote = RandomNote ();

		while (currentNote == noteToGuess) {
			currentNote = RandomNote ();
		}

		startBtn.SetActive (false);
		checkAnswerBtn.SetActive (true);
		UpdateText ();
	}

	public void ChangeCurrentNote(bool goUp)
	{
		if (goUp) {
			currentNote.a++;
			if (currentNote.a > 7) {
				currentNote.a = 1;
				currentNote.b++;
				if (currentNote.b > 8) {
					currentNote.b = 8;
					currentNote.a = 3;
				}
			}
			if (currentNote.a >= 3 && currentNote.b >= 8) {
				currentNote.a = 3;
				currentNote.b = 8;
				goUpBtn.interactable = false;
			}
			if (currentNote.a > 1 && currentNote.b == 1)
				goDownBtn.interactable = true;
		} else {
			currentNote.a--;
			if(currentNote.a < 1)
			{
				currentNote.a = 7;
				currentNote.b--;
				if(currentNote.b < 1)
				{
					currentNote.b = 1;
					currentNote.a = 1;
				}
			}
			if(currentNote.a <= 1 && currentNote.b <= 1)
			{
				currentNote.a = 1;
				currentNote.b = 1;
				goDownBtn.interactable = false;
			}
			if(currentNote.a < 3 && currentNote.b == 8)
				goUpBtn.interactable = true;
		}

		currentNoteText.text = notes [currentNote.a - 1] + currentNote.b.ToString ();
	}

	public void CheckAnswer()
	{
		if (currentNote.IsEqualTo (noteToGuess)) {
			correctText.SetActive (true);
			correctText.GetComponent<Text>().text = "Correct!";
			StartCoroutine (RestartGame ());
		} else {
			incorrectText.SetActive(true);
			incorrectText.GetComponent<Text>().text = (string.Format ("Incorrect!\nYour Guess: {0}\n" +
			                                                          "ActualNote: {1}",
			                                                          notes [currentNote.a - 1] + currentNote.b.ToString (),
			                                                          notes [noteToGuess.a - 1] + noteToGuess.b.ToString ()));
			StartCoroutine (RestartGame());
		}
	}

	IEnumerator RestartGame ()
	{
		yield return new WaitForSeconds (3f);
		Application.LoadLevel (Application.loadedLevel);
	}

	void UpdateText()
	{
		noteToGuessText.text = notes [noteToGuess.a - 1] + noteToGuess.b.ToString ();
		currentNoteText.text = notes [currentNote.a - 1] + currentNote.b.ToString ();
	}

	Tuple<int, int> RandomNote()
	{
		int a = Random.Range (1, 8);
		int b = Random.Range (1, 9);

		if (b == 8 && a > 3)
			a = 3;
		Tuple<int, int> ab = new Tuple<int, int> (a, b);
		return ab;
	}
}
