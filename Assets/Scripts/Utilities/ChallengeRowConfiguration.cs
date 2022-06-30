using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChallengeRowConfiguration : MonoBehaviour
{
    //Attributes:
    [Header("Variables setup")]
    [SerializeField] private TMPro.TMP_Text title;
    [SerializeField] private Image stakeholderImage;
    private Challenge challenge;

    [Header("Configuration")]
    [SerializeField] private string nextSceneToLoad;

    //Methods:
    public void initialise(Challenge challenge)
    {
        this.title.text = challenge.title.ToString();
        stakeholderImage.sprite = GameData.SharedInstance.stakeholderBarImages[Convert.ToInt32(challenge.stakeholderImage)];
        this.challenge = challenge;
    }

    public void createNewRoomWhenButtonIsPressed()
    {
        //Update session:
        updateSessionWithSelectedChallengeData(challenge);
        Button button = GetComponentInParent(typeof(Button)) as Button;
        button.interactable = false;
        //Show stakeholder intro scene:
        SceneManager.LoadSceneAsync(nextSceneToLoad);
    }

    private void updateSessionWithSelectedChallengeData(Challenge challenge)
    {
        GameData.SharedInstance.challengeData = challenge;
    }
}
