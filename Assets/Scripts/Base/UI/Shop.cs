using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Shop : BaseObject
{
    [BaseSerialize] protected int adsCount;
    [SerializeField] public Transform[] buttonPos;
    [SerializeField] private int sellPercent;
    [SerializeField] private int ADSvalue;
    [SerializeField] private int minCount;
    private List<ResourceType> resources = new List<ResourceType>();
    private UIShopScreenView uiShopScreenView;
    public event System.Action onAdsShowed;
    [SerializeField] ColliderEventListener showButtonsCollider;
    
    private void Awake()
    {
        resources = FindObjectOfType<ResourcesController>().OpenedResources;
        uiShopScreenView = FindObjectOfType<UIShopScreenView>();
        adsCount++;
        RequireSave();
        showButtonsCollider.OnTriggerEnterEvent += Shop_OnTriggerEnterEvent;
        showButtonsCollider.OnTriggerExitEvent += Shop_OnTriggerExitEvent;
        UpdateButtons();
    }

    void Shop_OnTriggerEnterEvent(Collider obj)
    {
        if (obj.TryGetComponent<PlayerBuilding>(out var playerBuilding))
            uiShopScreenView.ShowButtons();
    }

    void Shop_OnTriggerExitEvent(Collider obj)
    {
        if (obj.TryGetComponent<PlayerBuilding>(out var playerBuilding))
            uiShopScreenView.HideButtons();
    }

    public void UpdateButtons()
    {
        uiShopScreenView.UpdateSellButtons(minCount, sellPercent);
        uiShopScreenView.UpdateGetResourceButtons(ADSvalue, adsCount);
    }
}