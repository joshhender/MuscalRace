using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject[] noteBtns;
    public GameObject[] strikeObjs;
    public GameObject finishPanel;
    public Text timerText;
    public Text strikeText;
    public Text finishText;

    [HideInInspector()]
    public int strikeNum;

    public void IncorrectAnswer()
    {
        noteBtns[GameManager.instance.NM.noteToGuess].GetComponent<ButtonUtil>().correct.SetActive(true);
        AddStrike();
    }

    void AddStrike()
    {
        strikeNum++;
        strikeObjs[strikeNum - 1].SetActive(true);
        if(strikeNum == 3)
        {
            GameManager.instance.EndGame(false);
            finishPanel.SetActive(true);
            finishText.text = "You Lose";
            strikeText.text = "Strikes: " + strikeNum.ToString();
            return;
        }
        StartCoroutine(GameManager.instance.NewNote());
    }
}