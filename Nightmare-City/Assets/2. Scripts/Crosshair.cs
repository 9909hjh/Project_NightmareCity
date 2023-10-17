using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float gunAccuracy;


    [SerializeField]
    private GameObject go_CrosshairHUD;

    
    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("Walking"))
            animator.SetTrigger("Walk_Fire");
        else if (animator.GetBool("Running"))
            animator.SetTrigger("Running_Fire");
        else
        {
            animator.SetTrigger("idel_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
            gunAccuracy = 0.05f;
        else if (animator.GetBool("Running"))
            gunAccuracy = 0.08f;
        else
            gunAccuracy = 0.015f;

        return gunAccuracy;
    }
}
