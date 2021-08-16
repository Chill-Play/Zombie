using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using DG.Tweening;
using System;

[RequireComponent(typeof(Buildable))]
public class MapCell : MonoBehaviour
{
    [SerializeField] Buildable buildable;
    [SerializeField] GameObject content;
    // public int GridIndex { get; set; }



    private void Awake()
    {
        content.SetActive(false);
    }

    private void OnEnable()
    {
        buildable.OnBuilt += Buildable_OnBuilt;
    }


    private void OnDisable()
    {
        buildable.OnBuilt -= Buildable_OnBuilt;
    }


    private void Buildable_OnBuilt(bool afterDeserialization)
    {
        content.SetActive(true);
        if (!afterDeserialization)
        {
            content.transform.localScale = Vector3.zero;
            content.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc);
        }
    }


    public virtual void Build(System.Action<MapCell> OnBuildingComplete)
    {
        //transform.localScale = Vector3.zero;
        //transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc).OnComplete(() => OnBuildingComplete(this));       
    }
}
