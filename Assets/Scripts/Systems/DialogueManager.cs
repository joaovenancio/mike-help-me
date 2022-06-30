using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //Atributes: 
    private TMPro.TMP_Text nameText;
    private TMPro.TMP_Text dialogueText;
    private Button nextButton;
    private Queue<string> sentences = new Queue<string>();
    private Scene scene;
    private UnityAction action;
    private bool firstTime = true;
    //Singleton:
    private static DialogueManager mInstance;
    public static DialogueManager SharedInstance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("DialogueManager").AddComponent(typeof(DialogueManager)) as DialogueManager;
            }
            return mInstance;
        }
        set
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("DialogueManager").AddComponent(typeof(DialogueManager)) as DialogueManager;
            }
            mInstance = value;
        }
    }

    //Methods:
    public void StartDialogue(Dialogue dialogue)
    {
        //Setup variables reference:
        scene = SceneManager.GetActiveScene();

        //Retrieve UI elements reference:
        nameText = GameObject.Find("StakeholderNameText").GetComponent(typeof(TMPro.TMP_Text)) as TMPro.TMP_Text;
        dialogueText = GameObject.Find("DialogueText").GetComponent(typeof(TMPro.TMP_Text)) as TMPro.TMP_Text;
        if (!scene.name.Equals("StakeholderNecessitiesScene"))
        {
            action += DisplayNextSentence;
            nextButton = (GameObject.Find("NextButton").GetComponent(typeof(Button)) as Button);
            nextButton.onClick.AddListener(action);
        }

        //UI elements setup:
        nameText.text = dialogue.name;

        //Display the dialogue:
        if (scene.name.Equals("StakeholderNecessitiesScene")) // and se n estiver no ultimo turno
        {
            sentences.Enqueue(dialogue.sentences[GameData.SharedInstance.userStoriesIndex]);
        }
        else
        {
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (!firstTime)
        {
            nextButton.GetComponent<AudioSource>().Play();
        }
        firstTime = false;
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        ClearData();
        scene = SceneManager.GetActiveScene();

        switch (scene.name)
        {
            case "MikeIntroScene":
                SceneManager.LoadSceneAsync("_LoginScene");
                break;

            case "StakelhoderPresentationScene":
                StopAllCoroutines();
                SceneManager.LoadSceneAsync("AcceptOrDeclineScene");
                break;

            case "StakeholderEndgameScene":
                SceneManager.LoadSceneAsync("EndgameScene");
                break;
        }

    }

    private void ClearData()
    {
        if (sentences != null)
        {
            sentences.Clear();
        }
        sentences = new Queue<string>();

        nameText = null;
        dialogueText = null;

        firstTime = true;
    }
}
