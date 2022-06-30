using System;
using UnityEngine;
using UnityEngine.UI;

public class StakeholderPresentationSceneManager : MonoBehaviour
{
    [Header("Setup variables")]
    [SerializeField] private Image stakeholderImage;

    private void Awake()
    {
        //Debug.Log(GameData.sharedInstance.challengeData.stakeholderImage);
        stakeholderImage.sprite = GameData.SharedInstance.stakeholderImages[Convert.ToInt32(GameData.SharedInstance.challengeData.stakeholderImage)];
    }

    void Start()
    {
        //Launch Stakeholder Dialogue:
        DialogueManager.SharedInstance.StartDialogue(GameData.SharedInstance.challengeData.introDialogue);
    }

    //private void LaunchDialogue()
    //{
    //    Dialogue dialogue = (GameObject.Find("Mike").GetComponent(typeof(DialogueTrigger)) as DialogueTrigger).dialogue;
    //    DialogueManager.sharedInstance.StartDialogue(dialogue);

    //}
}
