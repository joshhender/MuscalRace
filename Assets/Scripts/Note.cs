using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Note {

    [Range(1, 7)]
    public int pitch;
    [Range(1, 8)]
    public int octave;

    public Note(int pitch, int octave)
    {
        this.pitch = pitch;
        this.octave = octave;
    } 
}
