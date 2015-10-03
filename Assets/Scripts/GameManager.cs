using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public int gas;

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
        if(Application.loadedLevelName == "Music")
        {
            NM = GameObject.Find("NoteManager").GetComponent<NoteManager>();
            score = 0;
        }
            UIM = GameObject.Find("UIManager").GetComponent<UIManager>();
        if(Application.loadedLevelName == "Main Menu")
        {
            UIM.gasSlider.value = gas;
            UIM.gasFill.color = Color.Lerp(Color.red, Color.green, (float)gas / 5);
            if (gas <= 0)
                UIM.raceBtn.interactable = false;
        }
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
        if(Application.loadedLevelName == "Music")
        {
            gas++;
            if (gas > 5)
                gas = 5;
            PlayerPrefs.SetInt("Gas", gas);
        }
        else if(Application.loadedLevelName == "Race")
        {
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
