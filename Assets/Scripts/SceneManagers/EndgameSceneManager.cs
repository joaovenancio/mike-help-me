using Sfs2X;
using Sfs2X.Entities;
using UnityEngine;
using UnityEngine.UI;

public class EndgameSceneManager : MonoBehaviour
{
    //Attributes:
    //public Text testText;
    [Header("Variables Setup")]
    public TMPro.TMP_Text textPlayerTop1;
    public Image starsPlayer1;
    public TMPro.TMP_Text textScore1;
    public TMPro.TMP_Text isDrawText1;
    [Space]
    public TMPro.TMP_Text textPlayerTop2;
    public Image starsPlayer2;
    public TMPro.TMP_Text textScore2;
    public TMPro.TMP_Text isDrawText2;
    [Space]
    public TMPro.TMP_Text textPlayerTop3;
    public Image starsPlayer3;
    public TMPro.TMP_Text textScore3;
    public TMPro.TMP_Text isDrawText3;

    private int userStoriesIndex;
    private User top1 = null;
    private User top2 = null;
    private User top3 = null;
    private SmartFox sfs;

    //Methods:
    private void Awake()
    {
        this.isDrawText1.enabled = false;
        this.isDrawText2.enabled = false;
        this.isDrawText3.enabled = false;

        sfs = SmartFoxConnection.Connection;

        userStoriesIndex = GameData.SharedInstance.userStoriesIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupTop3();
        UpdateUI();
        CheckForDraws();
    }

    private void SetupTop3()
    {
        top1 = GameData.SharedInstance.leaderboard[0];
        top2 = GameData.SharedInstance.leaderboard[1];
        top3 = GameData.SharedInstance.leaderboard[2];
    }

    private void UpdateUI()
    {
        int maxPoints = sfs.LastJoinedRoom.GetVariable("maxPoints").GetIntValue();

        textPlayerTop1.text = top1.Name;
        starsPlayer1.fillAmount = (float)(top1.GetVariable("score").GetIntValue()) / (float)(maxPoints);
        textPlayerTop2.text = top2.Name;
        starsPlayer2.fillAmount = (float)(top2.GetVariable("score").GetIntValue()) / (float)(maxPoints);
        textPlayerTop3.text = top3.Name;
        starsPlayer3.fillAmount = (float)(top3.GetVariable("score").GetIntValue()) / (float)(maxPoints);

        textScore1.text = top1.GetVariable("score").GetIntValue().ToString() + "/" + maxPoints.ToString();
        textScore2.text = top2.GetVariable("score").GetIntValue().ToString() + "/" + maxPoints.ToString();
        textScore3.text = top3.GetVariable("score").GetIntValue().ToString() + "/" + maxPoints.ToString();
    }

    //Top 1,2,3 must have been determinated, otherwise it will fail
    public void CheckForDraws()
    {
        if ((top1.GetVariable("score").GetIntValue() == top2.GetVariable("score").GetIntValue()) &&
            (top1.GetVariable("score").GetIntValue() == top3.GetVariable("score").GetIntValue()))
        {
            isDrawText1.enabled = true;
            isDrawText2.enabled = true;
            isDrawText3.enabled = true;
        }
        else if (top1.GetVariable("score").GetIntValue() == top2.GetVariable("score").GetIntValue())
        {
            isDrawText1.enabled = true;
            isDrawText2.enabled = true;
        }
        else if (top2.GetVariable("score").GetIntValue() == top3.GetVariable("score").GetIntValue())
        {
            isDrawText2.enabled = true;
            isDrawText3.enabled = true;
        }
    }

    public void PlayAgain()
    {
        GameData.SharedInstance.StopPlaying();
    }
}
