using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Bounce : MonoBehaviour
{
    void Start()
    {
        transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 1, 1f).SetLoops(-1);
    }
}
