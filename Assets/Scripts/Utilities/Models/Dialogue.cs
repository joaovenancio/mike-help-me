using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //Atributes:
    [TextArea(1, 1)]
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;

    //Constructor:
    public Dialogue(string name, string[] sentences)
    {
        this.name = name;
        this.sentences = sentences;
    }

}
