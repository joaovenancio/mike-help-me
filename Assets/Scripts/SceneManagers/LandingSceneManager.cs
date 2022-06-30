using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandingSceneManager : MonoBehaviour
{
    [Header("Setup variables")]
    [SerializeField] private Button startButton;

    [Header("Custom variables")]
    [SerializeField] private string nextSceneToLoad;

    public void OnClickStartButton ()
    {
        startButton.enabled = false;
        SceneManager.LoadScene(nextSceneToLoad);
    }

}
