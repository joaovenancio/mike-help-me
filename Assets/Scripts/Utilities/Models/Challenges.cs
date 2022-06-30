using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Challenge
{
    //Atributes:
    [TextArea(1, 1)]
    public string stakeholderID; 
    [TextArea(1, 1)]
    public string stakeholderName; //The stakeholderName is the same as the Room group
    [TextArea(1, 1)]
    public string stakeholderImage;
    [TextArea(1, 1)]
    public string title;
    public Dialogue introDialogue;
    public Dialogue necessitiesDialogue;

    //Constructor:
    public Challenge(string stakeholderID, string stakeholderName, string stakeholderImage, string title)
    {
        this.stakeholderID = stakeholderID;
        this.stakeholderName = stakeholderName;
        this.stakeholderImage = stakeholderImage;
        this.title = title;
        introDialogue = new Dialogue("default", new string[] { "default","default"});
        necessitiesDialogue = new Dialogue("default", new string[] { "default", "default" });
    }

    public Challenge(string stakeholderID, string stakeholderName, string stakeholderImage, string title, Dialogue dialogue, Dialogue necessitiesDialogue)
    {
        this.stakeholderID = stakeholderID;
        this.stakeholderName = stakeholderName;
        this.stakeholderImage = stakeholderImage;
        this.title = title;
        this.introDialogue = dialogue;
        this.necessitiesDialogue = necessitiesDialogue;
    }

    public Challenge(IDictionary<string, object> dict)
    {
        stakeholderID = dict["stakeholderID"].ToString();
        stakeholderName = dict["stakeholder"].ToString();
        stakeholderImage = dict["stakeholderImage"].ToString();
        title = dict["title"].ToString();
        introDialogue = new Dialogue("default", new string[] { "default", "default" });
        necessitiesDialogue = new Dialogue("default", new string[] { "default", "default" });
    }
}
