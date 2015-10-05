using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public RectTransform[] noteObjs;
    public GameObject[] noteBtns;
	public AudioSource[] notes;
	public AudioSource correctSFX;
	public Text timerText;
	public Text scoreText;
	public int noteToGuess;
	public int score;
    public int notesLeft;
	public float timer;
    [Range(0, 5)]
    public int gas;

    [HideInInspector()]
    public NoteManager NM;
    [HideInInspector()]
    public UIManager UIM;
    [HideInInspector()]
	public float timerResetNum;
    [HideInInspector()]
	public bool hasStarted;
	
	int spawnCorrectNote;
	bool isFlashing;

	public static GameManager instance;
    
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
        StartUp();
	}

    void OnLevelWasLoaded()
    {
        StartUp();
    }

	void StartUp()
	{
		hasStarted = false;
        gas = PlayerPrefs.GetInt("Gas");
        UIM = GameObject.Find("UIManager").GetComponent<UIManager>();
        if(Application.loadedLevelName == "Music")
        {
            NM = GameObject.Find("NoteManager").GetComponent<NoteManager>();
            score = 0;
        }
        else if(Application.loadedLevelName == "Main Menu")
        {
            UIM.SetUpGas(gas);
        }
    }

	public void StartGame()
	{
        hasStarted = true;
        if(Application.loadedLevelName == "Music")
        {
            NM.NewNote();
        }
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
        return false;
    }

    public void EndGame(bool hasWon)
    {
        hasStarted = false;
        if(Application.loadedLevelName == "Music")
        {
            if (hasWon)
            {
                Debug.Log("You Win!");
                UIM.Finish(hasWon);
            } else {
                Debug.Log("You Lose!");
                StartCoroutine(RestartGame());
            }

            gas++;
            if (gas > 5)
                gas = 5;
            PlayerPrefs.SetInt("Gas", gas);
        }
        else if(Application.loadedLevelName == "Race")
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.place = UIM.finishedCars.IndexOf(player.gameObject);
            UIM.Finish(player.place + 1 == 1);
            hasStarted = false;
            gas--;
            if (gas < 0)
                gas = 0;
            PlayerPrefs.SetInt("Gas", gas);
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
