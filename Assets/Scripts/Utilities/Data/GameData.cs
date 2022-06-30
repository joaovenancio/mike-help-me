using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    //----------------------------------------------------------
    // Editor public properties
    //----------------------------------------------------------
    [Header("Setup variables:")]
    public Sprite[] stakeholderImages;
    public Sprite[] stakeholderBarImages;
    public int userStoriesIndex = 0;
    public bool isTextMeshProInstructionCanvasEnabled = false;
    public List<User> playersList = new List<User>();
    public bool firstTimeOnWaitingScene = false;
    public int maxUserStoryPointsAtThisPointInGame = 0;
    public int lastMaxUserStoryPoints = 0;
    public Challenge challengeData = new Challenge("default", "default", "default.png", "default");
    public string playerID = "default";

    [Header("Configuration:")]
    public int minCapacityToStartGame = 3;
    public bool playerNotResponded = false;
    public List<User> leaderboard = new List<User>();

    //----------------------------------------------------------
    // Private properties
    //----------------------------------------------------------

    private SmartFox sfs;
    private string _turn = "RP1"; // HU is proposing phase, AV is avaliation phase, ENDEP is ended (not) enought players, END is end (the game finished without any problem)
    private bool sentResponseHaveAllUserStories = false;

    //----------------------------------------------------------
    // Getters & Setters
    //----------------------------------------------------------

    public string Turn
    {
        get { return _turn; }
        set
        {
            _turn = value;

            userStoriesIndex = Convert.ToInt32(Turn.Substring(2)) - 1; //Minus 1 because we want to make the value an index and not the proper turn

        }
    }

    //----------------------------------------------------------
    // Singleton
    //----------------------------------------------------------

    private static GameData mInstance;
    public static GameData SharedInstance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("GameData").AddComponent(typeof(GameData)) as GameData;
            }
            return mInstance;
        }
        set
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("GameData").AddComponent(typeof(GameData)) as GameData;
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

        Application.runInBackground = true;//***
    }

    private void Start()
    {
        sfs = SmartFoxConnection.Connection;

    }

    private void Update()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;

        if ((activeSceneName.Equals("GameplayScene") || activeSceneName.Equals("WaitingScene")) && !sentResponseHaveAllUserStories)
        {
            bool haveAllUserStories = true;
            foreach (User user in sfs.LastJoinedRoom.UserList)
            {
                if (!user.ContainsVariable("H" + (userStoriesIndex + 1).ToString()))
                {
                    haveAllUserStories = false;
                    break;
                }
            }

            if (haveAllUserStories)
            {
                sentResponseHaveAllUserStories = true;
                sfs.Send(new ExtensionRequest("haveAllStories", new SFSObject(), sfs.LastJoinedRoom));
                Debug.Log("I have all user stories!");
            }
        }
    }

    //----------------------------------------------------------
    // Private helper methods
    //----------------------------------------------------------

    private void Reset()
    {
        // Remove SFS2X listeners
        sfs.RemoveAllEventListeners();
    }

    //----------------------------------------------------------
    // SmartFoxServer event listeners
    //----------------------------------------------------------

    private void OnRoomVariablesUpdate(BaseEvent evt)
    {

        if (((Room)evt.Params["room"]).Id == sfs.LastJoinedRoom.Id)
        {
            foreach (string changedVar in (List<string>)evt.Params["changedVars"])
            {
                switch (changedVar)
                {
                    case "turn":
                        if (sfs.LastJoinedRoom.GetVariable("turn").GetStringValue().Equals("ENDEP"))
                        {
                            FindTopPlayers(false);
                            Debug.Log("ENDEP");
                            SceneManager.LoadScene("StakeholderEndgameScene");
                            break;
                        }
                        else if (sfs.LastJoinedRoom.GetVariable("turn").GetStringValue().Equals("END"))
                        {
                            Debug.Log("Nice!");
                        }

                        switch (sfs.LastJoinedRoom.GetVariable("turn").GetStringValue().Substring(0, 2))
                        {
                            case "RP":
                                Debug.Log("RP");
                                if (sfs.LastJoinedRoom.GetVariable("turn").GetStringValue().Equals("RP1"))
                                {
                                    //PlayerBoardManager.sharedInstance.DisableEventListeners();
                                }

                                Turn = sfs.LastJoinedRoom.GetVariable("turn").GetStringValue();
                                SceneManager.LoadScene("StakeholderNecessitiesScene");
                                break;

                            case "PP":
                                Debug.Log("PP");
                                Turn = sfs.LastJoinedRoom.GetVariable("turn").GetStringValue();
                                SceneManager.LoadScene("GameplayScene");
                                break;

                            case "EP":
                                Debug.Log("EP");
                                Debug.Log(sfs.MySelf.Id);
                                Turn = sfs.LastJoinedRoom.GetVariable("turn").GetStringValue();
                                SceneManager.LoadScene("GameplayEvaluationScene");
                                break;

                            case "SP":
                                Debug.Log("SP");
                                Turn = sfs.LastJoinedRoom.GetVariable("turn").GetStringValue();
                                maxUserStoryPointsAtThisPointInGame = ((userStoriesIndex + 1) * ((sfs.LastJoinedRoom.PlayerList.Count - 1) * (sfs.LastJoinedRoom.PlayerList.Count - 2)));
                                SceneManager.LoadScene("ScoreScene");
                                sentResponseHaveAllUserStories = false;
                                break;
                        }
                        break;

                    case "timer":
                        Debug.Log("Timer");
                        break;
                }
            }
        }
    }

    //----------------------------------------------------------
    // Methods
    //----------------------------------------------------------

    public void ActivateRoomVariablesUpdateListener(bool activate)
    {
        Debug.Log("activateRoomVariablesUpdateListener");

        if (activate)
        {
            sfs.AddEventListener(SFSEvent.ROOM_VARIABLES_UPDATE, OnRoomVariablesUpdate);
        }
        else
        {
            sfs.RemoveEventListener(SFSEvent.ROOM_VARIABLES_UPDATE, OnRoomVariablesUpdate);
        }
    }

    public string CalculateNextPhase()
    {
        string nextTrun = "";

        switch (Turn.Substring(0, 2))
        {
            case "RP":
                nextTrun = "PP" + (userStoriesIndex + 1).ToString();
                break;

            case "PP":
                nextTrun = "EP" + (userStoriesIndex + 1).ToString();
                break;

            case "EP":
                nextTrun = "RP" + (userStoriesIndex + 2);
                break;
        }

        return nextTrun;
    }

    public string CalculateNextTurn()
    {
        string nextTrun = "";

        nextTrun = "HU" + (userStoriesIndex + 2);

        return nextTrun;
    }

    public void FindTopPlayers(bool ofTheTurn)
    {
        string variableID = "";

        if (ofTheTurn)
        {
            variableID = "E" + (userStoriesIndex + 1).ToString();
        }
        else
        {
            variableID = "score";
        }

        List<User> orderedPlayersList = sfs.LastJoinedRoom.UserList.OrderByDescending(user => user.GetVariable(variableID).GetIntValue()).ToList();

        GameData.SharedInstance.leaderboard = orderedPlayersList;

        Debug.Log("-----------");
        foreach (User u in orderedPlayersList)
        {
            Debug.Log(u.GetVariable("score").GetIntValue());
        }


        Debug.Log("-----------");
        foreach (User u in orderedPlayersList)
        {
            Debug.Log(u.Name);
        }
        Debug.Log("-----------");
    }

    public void StopPlaying() //*** quem sabe colocar no Session + lembrar de tirar todos os triggers events
    {

        sfs.Send(new LeaveRoomRequest(sfs.LastJoinedRoom));
        sfs.Send(new UnsubscribeRoomGroupRequest(GameData.SharedInstance.challengeData.stakeholderName.ToString())); // sfs.addEventListener(SFSEvent.ROOM_GROUP_UNSUBSCRIBE, onGroupUnsubscribed);sfs.addEventListener(SFSEvent.ROOM_GROUP_UNSUBSCRIBE_ERROR, onGroupUnsubscribeError);
        sfs.RemoveAllEventListeners();
        ResetVariables();
        Destroy(GameObject.Find("StatusBarManager"));
        Destroy(GameObject.Find("InstructionsManager"));
        SceneManager.LoadSceneAsync("ChallengesScene");
        Destroy(this.gameObject);
    }

    private void ResetVariables()
    {
        userStoriesIndex = 0;
        isTextMeshProInstructionCanvasEnabled = false;
        playersList = new List<User>();
        firstTimeOnWaitingScene = false;
        maxUserStoryPointsAtThisPointInGame = 0;
        lastMaxUserStoryPoints = 0;
        challengeData = new Challenge("default", "default", "default.png", "default");
        playerID = "default";
        minCapacityToStartGame = 3;
        playerNotResponded = false;
        leaderboard = new List<User>();
        _turn = "RP1";
        Turn = "RP1";
        sentResponseHaveAllUserStories = false;
    }
}
