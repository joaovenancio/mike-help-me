using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [Header("Setup variables")]
    [SerializeField] private Animator animatorController;


    private void Awake()
    {
        if (animatorController == null)
        {
            Debug.LogWarning("AnmimationTrigger has a NULL AnimatorController.");
        }
    }

    public void OnAnimationTrigger ()
    {
        animatorController.SetTrigger("jump");
    }
}
