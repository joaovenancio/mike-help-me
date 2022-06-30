using Sfs2X;
using Sfs2X.Entities;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSceneManager : MonoBehaviour
{
    [Header("Setup variables")]
    public TMPro.TMP_Text textPlayerTop1;
    public Image starsPlayer1;
    [Space]
    public TMPro.TMP_Text textPlayerTop2;
    public Image starsPlayer2;
    [Space]
    public TMPro.TMP_Text textPlayerTop3;
    public Image starsPlayer3;

    private SmartFox sfs;
    private User top1 = null;
    private User top2 = null;
    private User top3 = null;
    private int userStoriesIndex;

    private void Awake()
    {
        sfs = SmartFoxConnection.Connection;
    }

    // Start is called before the first frame update
    void Start()
    {
        StatusBarManager.SharedInstance.UpdateStatusBar();
        StatusBarManager.SharedInstance.StartTimer();

        userStoriesIndex = GameData.SharedInstance.userStoriesIndex;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (sfs != null)
            sfs.ProcessEvents();
    }

    private void UpdateUI()
    {
        float maxPointsOnRound = (float)((sfs.LastJoinedRoom.UserCount - 1) * (sfs.LastJoinedRoom.UserCount - 2));
        string round = "E" + (userStoriesIndex + 1);

        setupTop3();

        textPlayerTop1.text = top1.Name;
        starsPlayer1.fillAmount = (float)(top1.GetVariable(round).GetIntValue()) / maxPointsOnRound;
        textPlayerTop2.text = top2.Name;
        starsPlayer2.fillAmount = (float)(top2.GetVariable(round).GetIntValue()) / maxPointsOnRound;
        textPlayerTop3.text = top3.Name;
        starsPlayer3.fillAmount = (float)(top3.GetVariable(round).GetIntValue()) / maxPointsOnRound;
    }

    private void setupTop3()
    {
        GameData.SharedInstance.FindTopPlayers(true);

        Debug.Log("LEADERBOARD: " + GameData.SharedInstance.leaderboard.Count);
        //Debug.Log(GameData.sharedInstance.leaderboard[2]);

        top1 = GameData.SharedInstance.leaderboard[0];
        top2 = GameData.SharedInstance.leaderboard[1];
        top3 = GameData.SharedInstance.leaderboard[2];
    }

}
