using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySceneController : MonoBehaviour
{
    [Header("Setup variables")]
    [SerializeField] private GameObject[] userStoryParts;
    [SerializeField] private Transform userStoryFatherTransform;
    [SerializeField] private float amountToMove;
    [SerializeField] [Range(0f,1f)] private float translationVelocity;

    private TMPro.TMP_InputField userStoryInputField;
    private int partIndex = 0;
    private SmartFox sfs;
    private string userStory = "";
    private Vector2 newPosition;
    private bool firstTime = true;

    //----------------------------------------------------------
    // Unity calback methods
    //----------------------------------------------------------
    void Start()
    {
        newPosition = new Vector2(userStoryFatherTransform.position.x, userStoryFatherTransform.position.y);

        sfs = SmartFoxConnection.Connection;

        StatusBarManager.SharedInstance.UpdateStatusBar();
        StatusBarManager.SharedInstance.StartTimer();
    }

    void Update()
    {
        sfs.ProcessEvents();

        MoveUserStoryUIParts();
    }

    //----------------------------------------------------------
    // Private helper methods
    //----------------------------------------------------------

    public void SendUserStory()
    {
        userStoryInputField = userStoryParts[partIndex].GetComponentInChildren<TMPro.TMP_InputField>();

        if (userStoryInputField.text.ToString().Length == 0) //Check if the user is playing
        {
            GameData.SharedInstance.playerNotResponded = true;
        }
        else
        {
            string userStoryPrefix = "";

            switch (partIndex)
            {
                case 0:
                    userStoryPrefix = "Como um(a) ";
                break;

                case 1:
                    userStoryPrefix = ", eu gostaria de ";
                break;

                case 2:
                    userStoryPrefix = " para que eu possa ";
                break;
            }

            userStory += userStoryPrefix + userStoryInputField.text;

            if (partIndex == 2)
            {
                userStory += ".";

                Debug.Log(userStory);

                SendStorySFS();

                ResetVariables();

                SceneManager.LoadScene("WaitingScene");

                return;
            }

            newPosition.Set(newPosition.x += amountToMove, newPosition.y);

            partIndex++;
        }

    }

    public void SendIncompleteUSerStory()
    {
        Debug.Log(firstTime);
        if (firstTime)
        {
            firstTime = false;

            if (userStory.Length > 0)
            {
                userStory += ".";

                Debug.Log(userStory);

                SendStorySFS();

                ResetVariables();

                SceneManager.LoadScene("WaitingScene");

                return;
            }
            else
            {
                userStory = "Não enviei.";

                Debug.Log(userStory);

                SendStorySFS();

                ResetVariables();

                SceneManager.LoadScene("WaitingScene");

                return;

            }
        }
        
    }

    private void MoveUserStoryUIParts()
    {
        userStoryFatherTransform.position = Vector3.Lerp(userStoryFatherTransform.position, newPosition, translationVelocity);
    }

    private void ResetVariables()
    {
        partIndex = 0;
    }

    private void SendStorySFS()
    {
        SFSObject obj = new SFSObject();

        obj.PutUtfString("story", userStory);

        sfs.Send(new ExtensionRequest("receiveStory", obj, sfs.LastJoinedRoom));
    }
}
