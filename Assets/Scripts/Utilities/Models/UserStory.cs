using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStory
{
    //Atributos:
    public string content;
    public int score;

    //Construtor:
    public UserStory()
    {
        this.content = "";
        this.score = 0;
    }

    public UserStory(string content, int score)
    {
        this.content = content;
        this.score = score;
    }

    public UserStory(IDictionary<string, object> dict)
    {
        this.content = dict["content"].ToString();
        this.score = Convert.ToInt32(dict["score"]);
    }
}
