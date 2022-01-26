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

    public bool FreeUpgradeAvailable { get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerBuilding playerBuilding))
        {
            //var screen = (UpgradesScreen)FindObjectOfType<UIController>().ShowScreen(screenId);
            var screen = (IShowScreen)UIController.Instance.ShowScreen(screenId);
            var statsList = new List<(StatsType, StatInfo)>();
            foreach (var type in stats)
            {
                var info = StatsManager.Instance.GetStatInfo(type);
                statsList.Add((type, info));
            }
            screen.Show(this, label, statsList, ResourcesController.Instance.ResourcesCount, () => { UIController.Instance.ReturnToDefaultScreen(); OnEndUpgrading?.Invoke(); });
        }
    }
}
