using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GlobalMapPlayer : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    public void Move(Vector3 target, System.Action onReachingCallback = null)
    {
        float time = Vector3.Distance(transform.position, target)/ speed;
        transform.DOMove(target, time).OnComplete(() => onReachingCallback());
    }



}
