using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
    public GameObject[] noteBtns;
	public AudioSource[] notes;
    public GameObject[] playerCars;
	public AudioSource correctSFX;
	public int noteToGuess, notesLeft, currentCar;
	public float timer;
    [Range(0, 100)]
    public int gas;
    public int gasToRace = 20;

    [HideInInspector()]
    public NoteManager NM;
    [HideInInspector()]
    public UIManager UIM;
    [HideInInspector()]
	public float timerResetNum;
    [HideInInspector()]
	public bool hasStarted;
	
    Transform spawnPoint;
    int notesRestartNum = 5;

	public static GameManager instance;
    
	void Awake()
	{
		if (instance == null)
			instance = this;
		if (instance != this)
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
        gas = PlayerPrefs.GetInt("Gas", 0);
        UIM = GameObject.Find("UIManager").GetComponent<UIManager>();
        switch (SceneManager.GetActiveScene().name)
        {
            case "BothClefs":
            case "TrebleClef":
            case "BassClef":
                notesRestartNum = notesLeft;
                NM = GameObject.Find("NoteManager").GetComponent<NoteManager>();
                noteBtns = UIM.noteBtns;
                break;
            case "MainMenu":
                UIM.SetUpGas(gas);
                currentCar = -1;
                notesLeft = notesRestartNum;
                break;
            case "Race":
                spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
                try
                {
                    Instantiate(playerCars[currentCar], 
                        spawnPoint.position, playerCars[currentCar].transform.rotation);
                }
                catch(System.IndexOutOfRangeException)
                {
                }
                break;
        }
    }

	public void StartGame()
	{
        hasStarted = true;
        string scene = SceneManager.GetActiveScene().name;
        if(scene == "BothClefs" || scene == "TrebleClef" || scene == "BassClef")
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

    public void SetCar(int car)
    {
        currentCar = car;
    }

    public bool CheckAnswer(int noteNum)
    {

        if (noteNum == NM.letterToGuess)
        {
            correctSFX.Play();
            notesLeft--;
            StopTimer();
            StartCoroutine(NewNote(notesLeft <= 0));
            return true;
        }
        if(noteNum != -1)
            StopTimer();
        UIM.IncorrectAnswer();
        return false;
    }
    
    public IEnumerator NewNote(bool end)
    {
        yield return new WaitForSeconds(1.5f);
        if (end)
        {
            EndGame(true);
            yield break;
        }
        foreach(GameObject go in noteBtns)
        {
            go.GetComponent<ButtonUtil>().ClearSigns();
        }
        NM.NewNote();
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

	IEnumerator RestartGame ()
	{
		yield return new WaitForSeconds (3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void LoseGas(int amount)
    {
        gas -= amount;
        if (gas > 100)
            gas = 100;
        else if (gas < 0)
            gas = 0;
        PlayerPrefs.SetInt("Gas", gas);
    }

    public void EndGame(bool hasWon)
    {
        hasStarted = false;
        switch (SceneManager.GetActiveScene().name)
        {
            case "BothClefs":
                if (hasWon)
                {
                    UIM.Finish(hasWon);
                }
                else {
                    StartCoroutine(RestartGame());
                }
                notesLeft = notesRestartNum;
                LoseGas(-20);
                break;
            case "TrebleClef":
                if (hasWon)
                {
                    UIM.Finish(hasWon);
                }
                else {
                    StartCoroutine(RestartGame());
                }
                notesLeft = notesRestartNum;
                LoseGas(-15);
                break;
            case "BassClef":
                if (hasWon)
                {
                    UIM.Finish(hasWon);
                } else {
                    StartCoroutine(RestartGame());
                }
                notesLeft = notesRestartNum;
                LoseGas(-15);
                break;
            case "Race":
                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                player.place = UIM.finishedCars.IndexOf(player.gameObject);
                UIM.Finish(player.place + 1 == 1);
                hasStarted = false;
                LoseGas(20);
                break;
        }
    }
}
