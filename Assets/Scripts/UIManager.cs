using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject[] noteBtns;
    public GameObject[] strikeObjs;
    public GameObject finishPanel;
    public Text timerText;
    public Text strikeText;
    public Text finishText;
    public Text placeText;
    public Slider gasSlider;
    public Image gasFill;
    public Button raceBtn;

    [HideInInspector()]
    public int strikeNum;
    //[HideInInspector()]
    public List<GameObject> finishedCars = new List<GameObject>(5);

    public void IncorrectAnswer()
    {
        noteBtns[GameManager.instance.NM.letterToGuess]
            .GetComponent<ButtonUtil>().correct.SetActive(true);
        StartCoroutine("AddStrike");
    }

    IEnumerator AddStrike()
    {
        strikeNum++;
        strikeObjs[strikeNum - 1].SetActive(true);
        GameManager.instance.LoseGas(10);
        if(strikeNum == 3)
        {
            yield return new WaitForSeconds(3);
            GameManager.instance.EndGame(false);
            finishPanel.SetActive(true);
            finishText.text = "You Lose";
            strikeText.text = "Strikes: " + strikeNum.ToString();
            yield break;
        }
        StartCoroutine(GameManager.instance.NewNote(false));
    }

    public void SetUpGas(int gas)
    {
        gasSlider.value = gas;
        gasFill.color = Color.Lerp(Color.red, Color.green, (float)gas / 5);
        if (gas < GameManager.instance.gasToRace)
            raceBtn.interactable = false;
    }

    public void Finish(bool hasWon)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "BothClefs":
            case "TrebleClef":
            case "BassClef":
                finishText.text = "Finished!";
                strikeText.text = "Strikes: " + strikeNum.ToString();
                finishPanel.SetActive(true);
                break;
            case "Race":
                if (hasWon)
                {
                    finishText.text = "You Win!";
                    placeText.text = "Place: 1/5";
                }
                else
                {
                    finishText.text = "Finished!";
                    placeText.text = string.Format("Place: {0}/5",
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().place + 1);
                }
                finishPanel.SetActive(true);
                break;
        }
    }
}