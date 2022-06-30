using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayEvaluationSceneManager : MonoBehaviour
{
    [Header("Variables setup")]
    [SerializeField] private TMPro.TMP_Text TextUserStoryBox1;
    [SerializeField] private TMPro.TMP_Text TextUserStoryBox2;

    private int numPlayers = 0;
    private int iteration = 0;
    private SmartFox sfs;


    public void SortPlayers()
    {
        List<User> aux = new List<User>(sfs.LastJoinedRoom.PlayerList);

        GameData.SharedInstance.playersList.Clear();

        foreach (User player in aux)
        {
            if (player.Id.Equals(sfs.MySelf.Id))
            {
                GameData.SharedInstance.playersList.Add(player);
                break;
            }
        }
        //Add the other players in the list:
        foreach (User player in aux)
        {
            if (!player.Id.Equals(sfs.MySelf.Id))
            {
                GameData.SharedInstance.playersList.Add(player);
            }
        }

        aux.Clear();

    }

    private void Awake()
    {
        sfs = SmartFoxConnection.Connection;
    }

    // Start is called before the first frame update
    void Start()
    {
        SortPlayers();
        SetupContent();

        numPlayers = GameData.SharedInstance.playersList.Count;
        Debug.Log(numPlayers);

        iteration = 0;

        StatusBarManager.SharedInstance.UpdateStatusBar();
        StatusBarManager.SharedInstance.StartTimer();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetupContent()
    {
        TextUserStoryBox1.text = GameData.SharedInstance.playersList[1].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
        Debug.Log(GameData.SharedInstance.playersList[1].Id);
        TextUserStoryBox2.text = GameData.SharedInstance.playersList[2].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
        Debug.Log(GameData.SharedInstance.playersList[2].Id);
    }

    public void NextStory(int choice)
    {
        switch (numPlayers)
        {
            case 3:
                //Load waiting/gameplay scene:
                //Mudar o turno?

                switch (choice)
                {
                    case 1:
                        GivePoints(GameData.SharedInstance.playersList[1], GameData.SharedInstance.playersList[2]);
                        break;

                    case 2:
                        GivePoints(GameData.SharedInstance.playersList[2], GameData.SharedInstance.playersList[1]);
                        break;
                }

                SceneManager.LoadSceneAsync("WaitingScene");

                break;

            case 4:
                switch (iteration)
                {
                    case 1:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[1], GameData.SharedInstance.playersList[2]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[2], GameData.SharedInstance.playersList[1]);
                                break;
                        }

                        //Update de text with the next text:
                        TextUserStoryBox1.text = GameData.SharedInstance.playersList[1].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
                        TextUserStoryBox2.text = GameData.SharedInstance.playersList[3].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();

                        break;

                    case 2:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[1], GameData.SharedInstance.playersList[3]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[3], GameData.SharedInstance.playersList[1]);
                                break;
                        }

                        //Update de text with the next text:
                        TextUserStoryBox1.text = GameData.SharedInstance.playersList[2].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
                        TextUserStoryBox2.text = GameData.SharedInstance.playersList[3].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();

                        break;

                    case 3:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[2], GameData.SharedInstance.playersList[3]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[3], GameData.SharedInstance.playersList[2]);
                                break;
                        }

                        SceneManager.LoadSceneAsync("WaitingScene");

                        break;
                }

                iteration++; //Update the iteration

                break;

            case 5:
                switch (iteration)
                {
                    case 1:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[1], GameData.SharedInstance.playersList[2]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[2], GameData.SharedInstance.playersList[1]);
                                break;
                        }

                        //Update de text with the next text:
                        TextUserStoryBox1.text = GameData.SharedInstance.playersList[1].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
                        TextUserStoryBox2.text = GameData.SharedInstance.playersList[3].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();

                        break;

                    case 2:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[1], GameData.SharedInstance.playersList[3]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[3], GameData.SharedInstance.playersList[1]);
                                break;
                        }

                        //Update de text with the next text:
                        TextUserStoryBox1.text = GameData.SharedInstance.playersList[1].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
                        TextUserStoryBox2.text = GameData.SharedInstance.playersList[4].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();

                        break;

                    case 3:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[1], GameData.SharedInstance.playersList[4]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[4], GameData.SharedInstance.playersList[1]);
                                break;
                        }

                        //Update de text with the next text:
                        TextUserStoryBox1.text = GameData.SharedInstance.playersList[2].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
                        TextUserStoryBox2.text = GameData.SharedInstance.playersList[3].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();

                        break;

                    case 4:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[2], GameData.SharedInstance.playersList[3]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[3], GameData.SharedInstance.playersList[2]);
                                break;
                        }

                        //Update de text with the next text:
                        TextUserStoryBox1.text = GameData.SharedInstance.playersList[2].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
                        TextUserStoryBox2.text = GameData.SharedInstance.playersList[4].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();

                        break;

                    case 5:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[2], GameData.SharedInstance.playersList[4]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[4], GameData.SharedInstance.playersList[2]);
                                break;
                        }

                        //Update de text with the next text:
                        TextUserStoryBox1.text = GameData.SharedInstance.playersList[3].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();
                        TextUserStoryBox2.text = GameData.SharedInstance.playersList[4].GetVariable("H" + (GameData.SharedInstance.userStoriesIndex + 1).ToString()).GetStringValue();

                        break;

                    case 6:

                        switch (choice)
                        {
                            case 1:
                                GivePoints(GameData.SharedInstance.playersList[3], GameData.SharedInstance.playersList[4]);
                                break;

                            case 2:
                                GivePoints(GameData.SharedInstance.playersList[4], GameData.SharedInstance.playersList[3]);
                                break;
                        }

                        SceneManager.LoadSceneAsync("WaitingScene");

                        break;
                }

                iteration++; //Update the iteration

                break;
        }
    }

    private void GivePoints(User playerVoted, User playerNotVoted)
    {
        SFSObject requestObject = new SFSObject();
        requestObject.PutInt("playerVoted", playerVoted.Id);
        Debug.Log("playerVoted: " + playerVoted.Id);

        requestObject.PutInt("playerNotVoted", playerNotVoted.Id);
        Debug.Log("playerNotVoted: " + playerNotVoted.Id);
        //Adicionar outros atributos como o player que enviou e o conjunto que ele avaliou

        sfs.Send(new ExtensionRequest("takePoints", requestObject, sfs.LastJoinedRoom));
    }
}
