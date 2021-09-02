using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildablesView : MonoBehaviour
{
    [SerializeField] BuildableInfoUI buildableInfoPrefab;
    Dictionary<Buildable, BuildableInfoUI> infoInstancePerBuildable = new Dictionary<Buildable, BuildableInfoUI>();

    void Start()
    {
        Buildable[] buildables = FindObjectsOfType<Buildable>(true);
        foreach(var buildable in buildables)
        {
            if (buildable.enabled)
            {
                SpawnInfoPrefab(buildable);
            }
            else
            {
                buildable.OnEnabled += () => SpawnInfoPrefab(buildable);
            }
        }
    }

    private void SpawnInfoPrefab(Buildable buildable)
    {
        var info = Instantiate(buildableInfoPrefab, transform);
        info.Initialize(buildable);
        info.OnBuildingBuilt += Info_OnBuildingBuilt;
        infoInstancePerBuildable.Add(buildable, info);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach(var pair in infoInstancePerBuildable)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pair.Key.transform.position);
            screenPos.z = 0f;
            pair.Value.transform.position = screenPos;
        }
    }


    private void Info_OnBuildingBuilt(EventMessage<Buildable> obj)
    {
        var info = obj.sender as BuildableInfoUI;
        info.transform.DOScale(0f, 0.3f).SetEase(Ease.InSine);
        infoInstancePerBuildable.Remove(obj.data);
    }
}
