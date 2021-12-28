using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetMarker : MonoBehaviour
{
    [SerializeField] Transform marker;
    public void Scale(float time)
    {
        marker.localScale = Vector3.zero;
        marker.DOScale(1f, time).OnComplete(() => Destroy(gameObject));
    }
}
