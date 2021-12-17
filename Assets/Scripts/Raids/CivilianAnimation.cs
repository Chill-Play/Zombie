using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAnimation : MonoBehaviour
{
    const string IDLE_ANIMATION_KEY = "Idle";
    const string WALK_ANIMATION_KEY = "Walk";
    const string RUN_ANIMATION_KEY = "Run";
    const string DANCE_ANIMATION_KEY = "Dance";
    const string DANCEID_ANIMATION_KEY = "DanceId";

    [SerializeField] Animator animator;

    public void GoIdle()
    {
        animator.SetBool(WALK_ANIMATION_KEY, false);
        animator.SetBool(RUN_ANIMATION_KEY, false);
        animator.SetBool(DANCE_ANIMATION_KEY, false);
        animator.SetBool(IDLE_ANIMATION_KEY, true);
    }

    public void GoWalk()
    {
        animator.SetBool(WALK_ANIMATION_KEY, true);
        animator.SetBool(RUN_ANIMATION_KEY, false);
        animator.SetBool(DANCE_ANIMATION_KEY, false);
        animator.SetBool(IDLE_ANIMATION_KEY, false);
    }

    public void GoRun()
    {
        animator.SetBool(WALK_ANIMATION_KEY, false);
        animator.SetBool(RUN_ANIMATION_KEY, true);
        animator.SetBool(DANCE_ANIMATION_KEY, false);
        animator.SetBool(IDLE_ANIMATION_KEY, false);
    }

    public void GoDance()
    {
        animator.SetInteger(DANCEID_ANIMATION_KEY, Random.Range(0, 5));
        //animator.SetBool(WALK_ANIMATION_KEY, false);
        //animator.SetBool(RUN_ANIMATION_KEY, false);
        animator.SetBool(DANCE_ANIMATION_KEY, true);
        //animator.SetBool(IDLE_ANIMATION_KEY, false);
    }
}
