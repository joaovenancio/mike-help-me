using Sfs2X;
using UnityEngine;

public class WaitingSceneManager : MonoBehaviour
{
    [Header("Variables setup")]
    [SerializeField] private GameObject screen1;
    [SerializeField] private GameObject screen2;
    [SerializeField] private TMPro.TMP_Text tipTitleScreen1;
    [SerializeField] private TMPro.TMP_Text tipText1Screen1;
    [SerializeField] private TMPro.TMP_Text tipTitleScreen2;
    [SerializeField] private TMPro.TMP_Text tipText1Screen2;
    [SerializeField] private TMPro.TMP_Text tipText2Screen2;
    [SerializeField] [TextArea(3, 10)] private string[] tipsTitle =
        {"",
        "",
        "",
        "",
        "" };
    [SerializeField] [TextArea(3, 10)] private string[] tips =
        {"",
        "",
        "",
        "",
        "" };
    [TextArea(3, 10)] [SerializeField] private string[] tips2 = 
        {"",
        "",
        "",
        "",
        ""};

    private SmartFox sfs;

    private void Awake()
    {

        switch (GameData.SharedInstance.userStoriesIndex)
        {
            case 0:
                screen2.SetActive(false);

                tipTitleScreen1.text = tipsTitle[GameData.SharedInstance.userStoriesIndex];
                tipText1Screen1.text = tips[GameData.SharedInstance.userStoriesIndex];

                break;

            case 1:
                screen1.SetActive(false);
                screen2.SetActive(true);

                tipTitleScreen2.text = tipsTitle[GameData.SharedInstance.userStoriesIndex];
                tipText1Screen2.text = tips[GameData.SharedInstance.userStoriesIndex];
                tipText2Screen2.text = tips2[GameData.SharedInstance.userStoriesIndex];

                break;

            default:
                screen1.SetActive(true);
                screen2.SetActive(false);

                if ((GameData.SharedInstance.userStoriesIndex + 1) > tips.Length)
                {
                    tipText1Screen1.fontSize = 56;
                    tipText1Screen1.text = "Esperando pelos outros jogadores.";
                }
                else
                {
                    tipText1Screen1.fontSize = 42;
                    tipTitleScreen1.text = tipsTitle[GameData.SharedInstance.userStoriesIndex];
                    tipText1Screen1.text = tips[GameData.SharedInstance.userStoriesIndex];
                }

                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sfs = SmartFoxConnection.Connection;

        Debug.Log("Waiting Scene");

        StatusBarManager.SharedInstance.UpdateStatusBar();
        StatusBarManager.SharedInstance.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (sfs != null)
            sfs.ProcessEvents();
    }
}
