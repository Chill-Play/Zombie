using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAgroSequence : MonoBehaviour
{
    [SerializeField] Animator animator;
    public bool IsPlaying { get; set; }

    //System.Action onComplete;


    private void Start()
    {
        GetComponent<UnitHealth>().OnDead += ZombieAgroSequence_OnDead;
    }

    private void ZombieAgroSequence_OnDead(EventMessage<Empty> obj)
    {
        StopAllCoroutines();
    }

    public void Play(System.Action onComplete = null)
    {
        StartCoroutine(PlaySequence(onComplete));
    }


    IEnumerator PlaySequence(System.Action onComplete)
    {
        animator.SetTrigger("Aggro");
        IsPlaying = true;
        yield return new WaitForSeconds(2.3f);
        IsPlaying = false;
        onComplete?.Invoke();
    }
}
