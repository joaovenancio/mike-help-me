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
            sentences = new string[] {"Acredito que isso seja tudo...", "Já vi o que cada um conseguiu propor e agora é minha hora de escolher qual de vocês vai virar o meu engenheiro de software", "E foi você " + sfs.MySelf.Name + "!", "Parabéns jovem! Você manda muito bem nesse trabalho de escrever histórias de usuário", "Tenho certeza de que daqui para frente você vai encontrar muitas riquezas.", "Já posso ver que vai ser muito bom trabalhar com você!" };
        }
        else if ((playerScore < (maxPoints * 0.7)) &&
          (playerScore >= (maxPoints * 0.5)))
        {
            sentences = new string[] {"Acredito que isso seja tudo...", "Já vi o que cada um conseguiu propor e agora é minha hora de escolher qual de vocês vai virar o meu engenheiro de software", "E vou dizer, não foi dessa vez " + sfs.MySelf.Name + ".", "Mesmo não conseguindo essa vaga, existem diversas outras que você pode tentar.", "E digo ainda: você foi muito bem, mas ainda precisa calibrar um pouco mais esses textos.", "De qualquer forma, foi muito bom ter entrado em contato contigo!" };
        }
        else
        {
            sentences = new string[] {"Acredito que isso seja tudo...", "Já vi o que cada um conseguiu propor e agora é minha hora de escolher qual de vocês vai virar o meu engenheiro de software", "E não foi dessa vez " + sfs.MySelf.Name + ".", "Escrever histórias de usuário não é fácil mesmo...", "Mas não desanime!", "Não conseguir essa vaga simplesmente significa que faltou aprender um pouco mais sobre como as histórias funcionam.", "Tenho certeza que com mais um pouco de estudo você vai ficar imbatível!", "Quero te ver por aí hein!" };
        }

        Dialogue dialogue = new Dialogue(GameData.SharedInstance.challengeData.stakeholderName, sentences);
        DialogueManager.SharedInstance.StartDialogue(dialogue);
    }
}
