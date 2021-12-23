using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetMarker : MonoBehaviour
{
    public void Scale(float time)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, time).OnComplete(() => Destroy(gameObject));
    }
}
