using System;
using UnityEngine;

public class DebugButtons : MonoBehaviour
{
    [SerializeField] private bool showButtons;
    [SerializeField] private GameObject[] debugUI;

    private HQBuilding hq;

    private void Start()
    {
        if (showButtons)
        {
            hq = FindObjectOfType<HQBuilding>();
            foreach (var ui in debugUI)
            {
                ui.SetActive(true);
            }
        }
    }

    public void AddNoiseLevel()
    {
        if (NoiseController.Instance)
            NoiseController.Instance.AddNoiseLevel(99999f);
    }

    public void KillZombies()
    {
        if (ZombieWaveSpawner.Instance)
        {
            ZombieWaveSpawner.Instance.StopSpawning();
            ZombieWaveSpawner.Instance.ExecudeAll();
        }
    }

    public void AddPointLevel()
    {
        hq.AddPoint(1);
    }

    public void LevelUp()
    {
        hq.LevelUp();
    }

    public void AddResources()
    {
        foreach(var slot in ResourcesController.Instance.ResourcesCount.Slots)
        {
            slot.count += 100;
        }
        ResourcesController.Instance.UpdateResources();
    }

    public void Suicide()
    {
        Squad.Instance.Units[0].GetComponent<UnitHealth>().TakeDamage(1000f, Vector3.forward);
    }
}