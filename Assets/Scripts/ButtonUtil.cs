using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonUtil : MonoBehaviour {

    public GameObject correct;
    public GameObject incorrect;

    public GameObject[] selectCircles;

	public void StartGame()
	{
		GameManager.instance.StartGame ();
	}

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        Time.fixedDeltaTime = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void CheckAnswer(int noteNum)
    {
        bool isCorrect = GameManager.instance.CheckAnswer(noteNum);
        if(isCorrect)
        {
            correct.SetActive(true);
            GameManager.instance.correctSFX.Play();
        }
        else
        {
            incorrect.SetActive(true);
        }
    }
    
    public void ClearSigns()
    {
        correct.SetActive(false);
        incorrect.SetActive(false);
    }

    public void SetCar(int car)
    {
        foreach(GameObject select in selectCircles)
        {
            select.SetActive(false);
        }
        selectCircles[car].SetActive(true);
        GameManager.instance.SetCar(car);
    }

    public void LoadRace()
    {
        if(GameManager.instance.currentCar != -1)
        {
            LoadLevel("Race");
        }
    }
}
