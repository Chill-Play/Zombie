using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Buildable), typeof(BaseBuilding))]
public class BuildingProcess : MonoBehaviour
{
    [SerializeField] GameObject buildingSpotPrefab; //Move to singleton or something

    GameObject buildingSpotInstance;
    Buildable buildable;
    BaseBuilding baseBuilding;
    // Start is called before the first frame update
    void Awake()
    {
        baseBuilding = GetComponent<BaseBuilding>();
        buildable = GetComponent<Buildable>();
        buildingSpotInstance = Instantiate(buildingSpotPrefab, transform.position, transform.rotation);
        SetBuildingActive(false);
        buildable.OnUpdate += Buildable_OnUpdate;
        buildable.OnBuilt += Buildable_OnBuilt;
    }

    private void SetBuildingActive(bool active)
    {
        buildingSpotInstance.SetActive(!active);
        baseBuilding.enabled = active;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
        if (TryGetComponent<MeshRenderer>(out var renderer))
        {
            renderer.enabled = active;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    private void Buildable_OnUpdate()
    {

    }


    private void Buildable_OnBuilt(bool obj)
    {
        SetBuildingActive(true);
        float targetScale = transform.localScale.x;
        transform.localScale = Vector3.one * 0.2f;
        transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutElastic, 1.1f, 0.4f);
    }
}
