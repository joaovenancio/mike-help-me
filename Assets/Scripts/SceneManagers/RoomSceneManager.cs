using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSceneManager : MonoBehaviour
{
    //----------------------------------------------------------
    // Editor public properties
    //----------------------------------------------------------

    [Header("Setup variables")]
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject contentContainer;
    [SerializeField] private Button startGameButton;

    //----------------------------------------------------------
    // Private properties
    //----------------------------------------------------------

    private SmartFox sfs;

    //----------------------------------------------------------
    // Singleton
    //----------------------------------------------------------

    private static RoomSceneManager mInstance;
    public static RoomSceneManager sharedInstance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("PlayerBoardManager").AddComponent(typeof(RoomSceneManager)) as RoomSceneManager;
            }
            return mInstance;
        }
        set
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("PlayerBoardManager").AddComponent(typeof(RoomSceneManager)) as RoomSceneManager;
            }
            mInstance = value;
        }
    }

    //----------------------------------------------------------
    // Unity calback methods
    //----------------------------------------------------------

    private void Awake()
    {
        mInstance = this;

        sfs = SmartFoxConnection.Connection;

        sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);

        
    }

    private void Start()
    {
        foreach (User user in sfs.LastJoinedRoom.PlayerList)
        {
            CreateRow(user);
        }

        //Debug.Log(sfs.RoomList.Count);
        Debug.Log(sfs.LastJoinedRoom.UserCount);
        Debug.Log(sfs.LastJoinedRoom.MaxUsers);
        Debug.Log(sfs.LastJoinedRoom.GetVariable("turn"));

        CheckIfUserCanStartGame();

        GameData.SharedInstance.ActivateRoomVariablesUpdateListener(true);
    }

    private void Update()
    {
        if (sfs != null)
            sfs.ProcessEvents();
    }

    //----------------------------------------------------------
    // Private helper methods
    //----------------------------------------------------------

    private void reset()
    {
        // Remove SFS2X listeners
        sfs.RemoveEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        sfs.RemoveEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
    }

    //----------------------------------------------------------
    // SmartFoxServer event listeners
    //----------------------------------------------------------

    void OnUserEnterRoom(BaseEvent evt)
    {
        Room room = (Room)evt.Params["room"];
        User user = (User)evt.Params["user"];

        CreateRow(user);

        Debug.Log("User: " + user.Name + " has just joined Room: " + room.Name);                        // .Net / Unity
        System.Diagnostics.Debug.WriteLine("User: " + user.Name + " has just joined Room: " + room.Name);       // UWP

        CheckIfUserCanStartGame();
    }

    private void OnUserExitRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        Room room = (Room)evt.Params["room"];

        if (user.Id != sfs.MySelf.Id)
        {
            if (contentContainer.transform.childCount >= 1)
            {
                for (int i = 0; i < contentContainer.transform.childCount; i++)
                {
                    GameObject childOBject = contentContainer.transform.GetChild(i).gameObject;

                    if (childOBject.GetComponent<PlayerRowConfiguration>().user.Id == user.Id)
                    {
                        Destroy(childOBject);
                        CheckIfUserCanStartGame();
                        Debug.Log("User: " + user.Name + " left the Room: " + room.Name);                        // .Net / Unity
                        Debug.Log(sfs.LastJoinedRoom.UserCount);

                        return;
                    }

                }
            }
        }

    }

    public void OnExtensionResponsePlayerBoard(BaseEvent evt) //***
    {
        string cmd = (string)evt.Params["cmd"];
        SFSObject dataObject = (SFSObject)evt.Params["params"];

        switch (cmd)
        {
            case "start":

                break;
        }

    }

    //----------------------------------------------------------
    // Methods
    //----------------------------------------------------------

    public void DisableEventListeners()
    {
        reset();
    }

    public void CreateRow(User user)
    {
        if (rowPrefab == null)
        {
            rowPrefab = Resources.Load("PlayersRowButton") as GameObject;
        }

        rowPrefab.transform.localScale.Set(1, 1, 1);

        GameObject newRow = Instantiate(rowPrefab) as GameObject;
        newRow.GetComponent<PlayerRowConfiguration>().Initialise(user);
        newRow.transform.SetParent(contentContainer.transform);
    }

    public void CheckIfUserCanStartGame()
    {

        TMPro.TMP_Text textComponent = startGameButton.GetComponentInChildren<TMPro.TMP_Text>();

        if (sfs.LastJoinedRoom.UserCount >= GameData.SharedInstance.minCapacityToStartGame)
        {
            textComponent.text = "Começar Partida";
            startGameButton.interactable = true;
        }
        else if (sfs.LastJoinedRoom.UserCount < GameData.SharedInstance.minCapacityToStartGame)
        {
            textComponent.text = "Aguardando Jogadores";
            startGameButton.interactable = false;
        }
    }

    public void StartGame()
    {
        if ((sfs.LastJoinedRoom.UserCount >= GameData.SharedInstance.minCapacityToStartGame) && (sfs.LastJoinedRoom.UserCount <= sfs.LastJoinedRoom.MaxUsers))
        {
            sfs.Send(new ExtensionRequest("start", new SFSObject(), sfs.LastJoinedRoom));
        }
    }
}
