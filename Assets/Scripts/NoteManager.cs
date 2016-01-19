using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour {

    public List<List<GameObject>> notesList;
    public List<GameObject> As;
    public List<GameObject> Bs;
    public List<GameObject> Cs;
    public List<GameObject> Ds;
    public List<GameObject> Es;
    public List<GameObject> Fs;
    public List<GameObject> Gs;
    public int letterToGuess;
    public int timeToGuess;

    int lastNote;
    int note;

    void Start()
    {
        notesList = new List<List<GameObject>>();
        notesList.Add(As);
        notesList.Add(Bs);
        notesList.Add(Cs);
        notesList.Add(Ds);
        notesList.Add(Es);
        notesList.Add(Fs);
        notesList.Add(Gs);
    }

    public void NewNote()
    {
        GameManager.instance.StopTimer();
        foreach(List<GameObject> list in notesList)
        {
            foreach(GameObject go in notesList[notesList.IndexOf(list)])
            {
                go.SetActive(false);
            }
        }
        ChooseRandomNote();
        ShowNote();
        GameManager.instance.StartTimer(timeToGuess);
    }

    void ShowNote()
    {
        notesList[letterToGuess][note].SetActive(true);
    }

    void ChooseRandomNote()
    {
        letterToGuess = Random.Range(0, notesList.Count);
        note = Random.Range(0, notesList[letterToGuess].Count);
        while(letterToGuess == lastNote)
            letterToGuess = Random.Range(0, 7);
        lastNote = letterToGuess;
    }
}
