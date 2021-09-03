using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class UpgradeZone : MonoBehaviour
{
    public event System.Action OnEndUpgrading;

    [SerializeField] string label;
    [SerializeField] SubjectId screenId;
    [SerializeField] List<StatsType> stats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        var screen = (UpgradesScreen)FindObjectOfType<UIController>().ShowScreen(screenId);
        var statsList = new List<(StatsType, StatInfo)>();
        foreach (var type in stats)
        {
            var info = FindObjectOfType<StatsManager>().GetStatInfo(type);
            statsList.Add((type, info));
        }
        screen.Show(label, statsList, FindObjectOfType<ResourcesController>().ResourcesCount, () => { FindObjectOfType<UIController>().HideActiveScreen(); OnEndUpgrading?.Invoke(); });
    }
}
