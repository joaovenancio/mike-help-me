using Sfs2X;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StakeholderNecessitiesSceneManager : MonoBehaviour
{
    [Header("Setup variables")]
    [SerializeField] private Image stakeholderImage;

    private SmartFox sfs;

    //----------------------------------------------------------
    // Unity calback methods
    //----------------------------------------------------------

    private void Awake()
    {
        sfs = SmartFoxConnection.Connection;
    }

    void Start()
    {
        setupInterface();
        StatusBarManager.SharedInstance.UpdateStatusBar();
        StatusBarManager.SharedInstance.StartTimer();
    }

    void Update()
    {
        sfs.ProcessEvents();
    }

    //----------------------------------------------------------
    // Private helper methods
    //----------------------------------------------------------

    private void setupInterface()
    {
        stakeholderImage.sprite = GameData.SharedInstance.stakeholderImages[Convert.ToInt32(GameData.SharedInstance.challengeData.stakeholderImage)];
        DialogueManager.SharedInstance.StartDialogue(GameData.SharedInstance.challengeData.necessitiesDialogue);
        
    }
}
