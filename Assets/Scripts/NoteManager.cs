using UnityEngine;

public class NoteManager : MonoBehaviour {

    public class Tuple<A, B>
    {
        public A a;
        public B b;

        public Tuple(A a, B b)
        {
            this.a = a;
            this.b = b;
        }

        public bool IsEqualTo(Tuple<A, B> other)
        {
            if (this.a.Equals(other.a) && this.b.Equals(other.b))
                return true;
            else
                return false;
        }
    }

    public GameObject[] notes;
    public int noteToGuess;

    int lastNote;

    void Start()
    {
        
    }

    public void NewNote()
    {
        GameManager.instance.StopTimer();
        foreach(GameObject go in notes)
        {
            go.SetActive(false);
        }
        ChooseRandomNote();
        ShowNote();
        GameManager.instance.StartTimer(5);
    }

    void ShowNote()
    {
        notes[noteToGuess].SetActive(true);
    }

    void ChooseRandomNote()
    {
        noteToGuess = Random.Range(0, 7);
        while(noteToGuess == lastNote)
            noteToGuess = Random.Range(0, 7);
        lastNote = noteToGuess;
    }
}
