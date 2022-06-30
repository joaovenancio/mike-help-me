using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeIntroSceneManager : MonoBehaviour
{
    [SerializeField] private Dialogue mike;

    void Start()
    {
        DialogueManager.SharedInstance.StartDialogue(mike);
    }
}
