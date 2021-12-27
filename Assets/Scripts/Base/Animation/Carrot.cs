using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float deltaGrowTime = 2f;

    public bool isGrows = false;

    void Start() 
    {
        
    }

    public void Grow()
    {
        StartCoroutine(GrowAnimation());
    }

    public void Pull()
    {
        animator.SetTrigger("Pull");
        isGrows = false;
        StartCoroutine(GrowAnimation());
    }

    IEnumerator GrowAnimation()
    {
        yield return new WaitForSeconds(deltaGrowTime);
        
        for (int i = 0; i < 3; i++)
        {
            string growState = "Grow" + i;
            animator.SetTrigger(growState);
            yield return new WaitForSeconds(deltaGrowTime);
        }

        isGrows = true;
    }
}
