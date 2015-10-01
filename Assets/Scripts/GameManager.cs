using UnityEngine;
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
    public GameObject[] noteBtns;
	public int noteToGuess;
	public AudioSource correctSFX;
	public Text timerText;
	public Text scoreText;
	public int score;
	public float timer;
	public AudioSource[] notes;
    public int notesLeft;

    [HideInInspector()]
    public NoteManager NM;
    [HideInInspector()]
    public UIManager UIM;
    [HideInInspector()]
	public float timerResetNum;
	
	int spawnCorrectNote;
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
        NM = GameObject.Find("NoteManager").GetComponent<NoteManager>();
        UIM = GameObject.Find("UIManager").GetComponent<UIManager>();
        score = 0;
    }

	public void StartGame()
	{
        hasStarted = true;
        NM.NewNote();
	}

	public void StartTimer(float time)
	{
		timer = time;
		timerResetNum = timer;
        UpdateText();
		InvokeRepeating ("DecreaseTimeRemaining", 1.0f, 1.0f);
		hasStarted = true;
	}

	void DecreaseTimeRemaining()
	{
        if(timer <= 0)
        {
            StopTimer();
            CheckAnswer(-1);
            //timerText.text = "00";
            return;
        }
		timer--;
		UpdateText ();
	}

	void UpdateText()
	{
        //string minutes = Mathf.Floor (timer / 60).ToString ("00");
        //string seconds = Mathf.Floor (timer % 60).ToString ("00");

        string seconds = timer.ToString("00");
		
		if(UIM.timerText)
			UIM.timerText.text = seconds;
	}

	public void StopTimer()
	{
        if (!hasStarted)
            return;
		CancelInvoke ("DecreaseTimeRemaining");
		CancelInvoke ("Flash");
		hasStarted = false;
		timer = timerResetNum;
	}

    public bool CheckAnswer(int noteNum)
    {

        if (noteNum == NM.noteToGuess)
        {
            notesLeft--;
            if(notesLeft <= 0)
            {
                StopTimer();
                EndGame(true);
                return true;
            }
            StopTimer();
            StartCoroutine(NewNote());
            return true;
        }
        if(noteNum != -1)
            StopTimer();
        UIM.IncorrectAnswer();
        //StartCoroutine(NewNote());
        return false;
    }

    public void EndGame(bool hasWon)
    {
        if (hasWon)
        {
            Debug.Log("You Win!");
            UIM.finishText.text = "Finished!";
            UIM.strikeText.text = "Strikes: " + UIM.strikeNum.ToString();
            UIM.finishPanel.SetActive(true);
        }
        else
        {
            Debug.Log("You Lose!");
            StartCoroutine(RestartGame());
        }
    }

    public IEnumerator NewNote()
    {
        yield return new WaitForSeconds(1.5f);
        foreach(GameObject go in noteBtns)
        {
            go.GetComponent<ButtonUtil>().ClearSigns();
        }
        NM.NewNote();
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
