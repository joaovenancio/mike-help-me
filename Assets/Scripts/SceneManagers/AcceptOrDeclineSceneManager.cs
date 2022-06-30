using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AcceptOrDeclineSceneManager : MonoBehaviour
{
    //----------------------------------------------------------
    // Editor public properties
    //----------------------------------------------------------

    [Header("Setup variables")]
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private TMPro.TMP_Text errorText;

    [Header("Configuration")]
    [SerializeField] private int maxUsersOnRoom;
    [SerializeField] private string roomExtensionID = "mikeHelpMe";
    [SerializeField] private string roomClassName = "br.ufsc.inf.leb.MikeHelpMeRoomExtension";
    [SerializeField] private string nextSceneToLoad;

    //----------------------------------------------------------
    // Private properties
    //----------------------------------------------------------

    private SmartFox sfs;
    private bool clickedOneTime = false;

    //----------------------------------------------------------
    // Unity calback methods
    //----------------------------------------------------------

    private void Awake()
    {
        sfs = SmartFoxConnection.Connection;

        sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
        sfs.AddEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreationError);

        sfs.AddEventListener(SFSEvent.ROOM_GROUP_SUBSCRIBE, onGroupSubscribed);
        sfs.AddEventListener(SFSEvent.ROOM_GROUP_SUBSCRIBE_ERROR, onGroupSubscribeError);

    }

    private void Start()
    {
        errorText.enabled = false;
    }

    void Update()
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
        sfs.RemoveAllEventListeners();

        clickedOneTime = false;

        // Enable interface
        enableUIButtons(true);
    }

    private void enableUIButtons(bool enable)
    {

        this.acceptButton.interactable = enable;
        this.declineButton.interactable = enable;

    }

    //----------------------------------------------------------
    // SmartFoxServer event listeners
    //----------------------------------------------------------

    private void OnRoomJoin(BaseEvent evt)
    {
        // Remove SFS2X listeners and re-enable interface before moving to the main game scene
        reset();

        // Go to main game scene
        SceneManager.LoadScene(nextSceneToLoad);
    }

    private void OnRoomJoinError(BaseEvent evt)
    {
        // Show error message
        clickedOneTime = false;
        enableUIButtons(true);
        errorText.text = "Falha ao entrar em uma sala: " + (string)evt.Params["errorMessage"];
    }

    void OnRoomCreationError(BaseEvent evt)
    {
        // Show error message
        clickedOneTime = false;
        enableUIButtons(true);
        errorText.text = ("Room creation failed: " + (string)evt.Params["errorMessage"]);
    }

    //----------------------------------------------------------
    // Methods
    //----------------------------------------------------------

    public void onGroupSubscribed(BaseEvent evt)
    {
        Debug.Log("Group subscribed. The following rooms are now accessible: " + (List<Room>)evt.Params["newRooms"]);
        foreach (Room r in (List<Room>)evt.Params["newRooms"])
        {
            Debug.Log(r.Name);
        }
        foreach (String s in sfs.RoomManager.GetRoomGroups())
        {
            Debug.Log(s);
        }

        List<Room> roomsList = sfs.RoomManager.GetRoomListFromGroup(GameData.SharedInstance.challengeData.stakeholderName);
        Debug.Log(roomsList.Count);
        foreach (String s in sfs.RoomManager.GetRoomGroups())
        {
            Debug.Log(s);
        }

        if (roomsList != null)
        {
            Debug.Log("Entrei");
            Debug.Log(roomsList.Count);
            foreach (Room room in roomsList)
            {
                Debug.Log("Entrei foreach");
                if ((room.GetVariable("turn").GetStringValue().Equals("LOBBY")) && (room.UserCount < room.MaxUsers))
                {
                    Debug.Log(room.Name);
                    sfs.Send(new JoinRoomRequest(room.Id));
                    return;
                }
            }

            Debug.Log("Não tem salas para entrar");

            RoomSettings roomSettings = new RoomSettings(sfs.MySelf.Name);

            roomSettings.MaxUsers = Convert.ToInt16(maxUsersOnRoom);
            roomSettings.MaxSpectators = 0;
            roomSettings.IsGame = true;
            roomSettings.GroupId = GameData.SharedInstance.challengeData.stakeholderName;
            roomSettings.Extension = new RoomExtension(roomExtensionID, roomClassName);
            roomSettings.Events = new RoomEvents
            {
                AllowUserCountChange = true,
                AllowUserEnter = true,
                AllowUserExit = true,
                AllowUserVariablesUpdate = true
            };

            sfs.Send(new CreateRoomRequest(roomSettings, true));
        }

    }

    public void onGroupSubscribeError(BaseEvent evt)
    {
        clickedOneTime = false;
        enableUIButtons(true);
        Debug.Log("Group subscription failed: " + (string)evt.Params["errorMessage"]);
    }


    public void joinRoomWhenButtonIsPressed() //Verificar se existe um lugar melhor para colocar esse metodo. Queria colocar em um gameManager
    {

        enableUIButtons(false);

        if (!clickedOneTime)
        {
            clickedOneTime = true;
            Debug.Log(clickedOneTime);
            Debug.Log(GameData.SharedInstance.challengeData.stakeholderName);
            //Search for available rooms to join:
            sfs.Send(new SubscribeRoomGroupRequest(GameData.SharedInstance.challengeData.stakeholderName.ToString()));
        }

    }

    public void goBackWhenButtonIsPressed()
    {
        //Reset shared data:
        GameData.SharedInstance.challengeData = new Challenge("default", "default", "default.png", "default");
        //Reset network settings:
        GameData.SharedInstance.StopPlaying();
    }
}
