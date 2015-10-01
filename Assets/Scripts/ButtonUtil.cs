using UnityEngine;
using System.Collections;

public class ButtonUtil : MonoBehaviour {

    public GameObject correct;
    public GameObject incorrect;

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
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoadLevel(string level)
    {
        Application.LoadLevel(level);
    }

    public void CheckAnswer(int noteNum)
    {
        bool isCorrect = GameManager.instance.CheckAnswer(noteNum);
        if(isCorrect)
        {
            correct.SetActive(true);
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
}
