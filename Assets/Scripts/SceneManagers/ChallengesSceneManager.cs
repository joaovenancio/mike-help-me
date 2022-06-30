using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengesSceneManager : MonoBehaviour
{
    //Atributes:
    public List<Challenge> challengesList = new List<Challenge>();
    public GameObject rowPrefab;
    public GameObject scrollContainer;

    private void Start()
    {
        InitialiseUI();
    }

    void InitialiseUI()
    {
        foreach (Challenge challenge in challengesList)
        {
            CreateRow(challenge);
        }
    }

    public void CreateRow(Challenge challenge)
    {
        Debug.Log(challenge.title);
        GameObject newRow = Instantiate(rowPrefab) as GameObject;
        newRow.GetComponent<ChallengeRowConfiguration>().initialise(challenge);
        newRow.transform.SetParent(scrollContainer.transform);
    }
}
