using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapCellWithBuildings : MapCell
{
   [SerializeField] List<GameObject> buildings = new List<GameObject>();

    public override void Build(Action<MapCell> OnBuildingComplete)
    {
        transform.localScale = Vector3.zero;
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].transform.localScale = Vector3.zero;
        }
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc).OnComplete(() => BuildBuildings(OnBuildingComplete));
    }


    void BuildBuildings(Action<MapCell> OnBuildingComplete)
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < buildings.Count; i++)
        {
            sequence.Join(buildings[i].transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc));
            sequence.Join(buildings[i].transform.DOPunchPosition(Vector3.up * 0.8f, 0.5f,1));
        }
        sequence.AppendCallback(() => OnBuildingComplete(this));
    }
}
