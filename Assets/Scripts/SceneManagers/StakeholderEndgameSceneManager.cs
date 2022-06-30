using Sfs2X;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StakeholderEndgameSceneManager : MonoBehaviour
{
    //[TextArea(3, 10)]
    //public string[] sentences07;
    //[TextArea(3, 10)]
    //public string[] sentences0507;
    //[TextArea(3, 10)]
    //public string[] sentencesElse;
    [Header("Variables Setup")]
    public Image stakeholderImage;

    private SmartFox sfs;

    private void Awake()
    {
        sfs = SmartFoxConnection.Connection;
        //End game:
        //GameData.sharedInstance.isPlaying = false;
        //GameData.sharedInstance.FinishPlaying();//***
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        stakeholderImage.sprite = GameData.SharedInstance.stakeholderImages[Convert.ToInt32(GameData.SharedInstance.challengeData.stakeholderImage)];
        LaunchEndGameDialogue();
    }

    private void LaunchEndGameDialogue()
    {
        int playerScore = sfs.MySelf.GetVariable("score").GetIntValue();
        int maxPoints = sfs.LastJoinedRoom.GetVariable("maxPoints").GetIntValue();
        string[] sentences;

        if (playerScore >= (maxPoints * 0.7))
        {
            sentences = new string[] {"Acredito que isso seja tudo...", "J� vi o que cada um conseguiu propor e agora � minha hora de escolher qual de voc�s vai virar o meu engenheiro de software", "E foi voc� " + sfs.MySelf.Name + "!", "Parab�ns jovem! Voc� manda muito bem nesse trabalho de escrever hist�rias de usu�rio", "Tenho certeza de que daqui para frente voc� vai encontrar muitas riquezas.", "J� posso ver que vai ser muito bom trabalhar com voc�!" };
        }
        else if ((playerScore < (maxPoints * 0.7)) &&
          (playerScore >= (maxPoints * 0.5)))
        {
            sentences = new string[] {"Acredito que isso seja tudo...", "J� vi o que cada um conseguiu propor e agora � minha hora de escolher qual de voc�s vai virar o meu engenheiro de software", "E vou dizer, n�o foi dessa vez " + sfs.MySelf.Name + ".", "Mesmo n�o conseguindo essa vaga, existem diversas outras que voc� pode tentar.", "E digo ainda: voc� foi muito bem, mas ainda precisa calibrar um pouco mais esses textos.", "De qualquer forma, foi muito bom ter entrado em contato contigo!" };
        }
        else
        {
            sentences = new string[] {"Acredito que isso seja tudo...", "J� vi o que cada um conseguiu propor e agora � minha hora de escolher qual de voc�s vai virar o meu engenheiro de software", "E n�o foi dessa vez " + sfs.MySelf.Name + ".", "Escrever hist�rias de usu�rio n�o � f�cil mesmo...", "Mas n�o desanime!", "N�o conseguir essa vaga simplesmente significa que faltou aprender um pouco mais sobre como as hist�rias funcionam.", "Tenho certeza que com mais um pouco de estudo voc� vai ficar imbat�vel!", "Quero te ver por a� hein!" };
        }

        Dialogue dialogue = new Dialogue(GameData.SharedInstance.challengeData.stakeholderName, sentences);
        DialogueManager.SharedInstance.StartDialogue(dialogue);
    }
}
