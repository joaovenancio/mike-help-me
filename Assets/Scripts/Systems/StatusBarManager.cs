using Sfs2X;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatusBarManager : MonoBehaviour
{
    [Header("Setup variables")]
    [SerializeField] private int readPhaseTimerStartAt;
    [SerializeField] private int proposePhaseTimerStartAt;
    [SerializeField] private int gameplayEvaluationScene;
    [SerializeField] private int scorePhaseTimeStartAt;

    private TMPro.TMP_Text currentTurnText;
    private TMPro.TMP_Text timerText;
    private Image stakeholderStatusBarImage;
    private Image stakeholderMoodBarImage;
    private float timerCounter;
    private bool enable = false;
    private SmartFox sfs;

    //----------------------------------------------------------
    // Singleton
    //----------------------------------------------------------

    private static StatusBarManager mInstance;
    public static StatusBarManager SharedInstance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("StatusBarManager").AddComponent(typeof(StatusBarManager)) as StatusBarManager;
            }
            return mInstance;
        }
        set
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("StatusBarManager").AddComponent(typeof(StatusBarManager)) as StatusBarManager;
            }
            mInstance = value;
        }
    }

    //----------------------------------------------------------
    // Unity calback methods
    //----------------------------------------------------------

    private void Awake()
    {

        if (mInstance == null)
        {
            mInstance = this;

            DontDestroyOnLoad(this);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        sfs = SmartFoxConnection.Connection;
    }

    // Update is called once per frame
    void Update()
    {
        if ((SceneManager.GetActiveScene().name.Equals("StakeholderNecessitiesScene") ||
            SceneManager.GetActiveScene().name.Equals("GameplayScene") ||
            SceneManager.GetActiveScene().name.Equals("WaitingScene") ||
            SceneManager.GetActiveScene().name.Equals("ScoreScene") ||
            SceneManager.GetActiveScene().name.Equals("GameplayEvaluationScene")) &&
            (enable))
        {
            if (timerCounter <= 0)
            {
                timerCounter = 0;
                enable = false;

                if (SceneManager.GetActiveScene().name.Equals("GameplayScene"))
                {
                    GameObject.FindObjectOfType<GameplaySceneController>().SendIncompleteUSerStory();
                }
            }
            else
            {
                timerCounter -= Time.deltaTime;
                float seconds = Mathf.FloorToInt(timerCounter % 60);
                float minutes = timerCounter / 60;
                timerText.text = string.Format("{0:0}:{1:00}", (int) minutes, seconds);
            }
        }
    }

    public void UpdateStatusBar()
    {
        UpdateTimerVariables();

        RetrieveInterfaceReferences();

        timerText.text = timerCounter.ToString();
        currentTurnText.text = "Turno: " + ((GameData.SharedInstance.userStoriesIndex + 1).ToString()) + "/" + GameData.SharedInstance.challengeData.necessitiesDialogue.sentences.Length;
        stakeholderStatusBarImage.sprite = GameData.SharedInstance.stakeholderBarImages[Convert.ToInt32(GameData.SharedInstance.challengeData.stakeholderImage)];

        if (!GameData.SharedInstance.userStoriesIndex.Equals(0))
        {
            Debug.Log(sfs.MySelf.GetVariable("score").GetIntValue());
            Debug.Log(GameData.SharedInstance.maxUserStoryPointsAtThisPointInGame);

            stakeholderMoodBarImage.fillAmount = Convert.ToSingle(sfs.MySelf.GetVariable("score").GetIntValue()) / (float)(GameData.SharedInstance.maxUserStoryPointsAtThisPointInGame);
        }
        else
        {
            stakeholderMoodBarImage.fillAmount = 1;
        }
    }

    private void RetrieveInterfaceReferences()
    {
        currentTurnText = GameObject.Find("PhaseText").GetComponent<TMPro.TMP_Text>();
        timerText = GameObject.Find("Time").GetComponent<TMPro.TMP_Text>();
        stakeholderStatusBarImage = GameObject.Find("StakholderBarImage").GetComponent<Image>();
        stakeholderMoodBarImage = GameObject.Find("GreenMoodImage").GetComponent<Image>();
    }

    private void UpdateTimerVariables()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "StakeholderNecessitiesScene":
                timerCounter = readPhaseTimerStartAt;
                break;

            case "GameplayScene":
                timerCounter = proposePhaseTimerStartAt;
                break;

            case "GameplayEvaluationScene":
                timerCounter = gameplayEvaluationScene;
                break;

            case "ScoreScene":
                timerCounter = scorePhaseTimeStartAt;
                break;
        }
    }

    public void StartTimer()
    {
        enable = true;
    }
}
