using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : ResourceFactory
{
    [SerializeField] List<GameObject> batteryBoxLod = new List<GameObject>();  
    [SerializeField] float batteryFillSpeed = 0.5f;
    [SerializeField] Animator generator;
    [SerializeField] Animator plant;

    int currentBatteryLod = -1;

    protected override void Setup()
    {
        base.Setup();
        plant.SetFloat("FillSpeed", batteryFillSpeed);     
    }

    protected override void StartWork()
    {
        base.StartWork(); 
        plant.enabled = true;
        plant.SetBool("Work", true);
        generator.SetBool("Work", true);
    }

    protected override void StopWork()
    {
        base.StopWork();
        generator.SetBool("Work", false);
        plant.SetBool("Work", false);
    }

    [ContextMenu("Restart")]
    public void RestartWork()
    {
        batteryBoxLod[currentBatteryLod].SetActive(false);
        currentBatteryLod = -1;
        StartWork();
    }

    protected override void AddResource(int count = 1)
    {
        base.AddResource(count);
        UpdateSwichLod();
    }

    public override void Unload(int count)
    {
        base.Unload(count);
        UpdateSwichLod();
    }

    public void SwitchBatteryBoxLod()
    {

    }

    public void UpdateSwichLod()
    {
        int screwLod = Mathf.FloorToInt((float)currentResourcesCount / (float)resourcesLimit * (float)(batteryBoxLod.Count - 1));
        if (currentBatteryLod != -1)
        {
            batteryBoxLod[currentBatteryLod].SetActive(false);
        }
        if (currentResourcesCount > 0)
        {
            batteryBoxLod[screwLod].SetActive(true);
            currentBatteryLod = screwLod;
        }
    }
}
