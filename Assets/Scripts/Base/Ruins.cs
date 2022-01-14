using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ruins : MonoBehaviour
{
    [SerializeField] GameObject buildingRuins;


    public void Show(bool show)
    {
        var obstacles = GetComponentsInChildren<UnityEngine.AI.NavMeshObstacle>();
        foreach(var obstacle in obstacles)
        {
            obstacle.enabled = show;
        }
        buildingRuins.gameObject.SetActive(show);
    }

    public void ShowWhithAnimation(System.Action callback = null)
    {        
        Show(true);
        float targetScale = transform.localScale.x;
        transform.localScale = Vector3.one * 0.2f;
        transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutElastic, 1.1f, 0.4f).OnComplete(()=> callback?.Invoke());
    }
}
