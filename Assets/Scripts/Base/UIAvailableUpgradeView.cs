using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAvailableUpgradeView : MonoBehaviour
{
    [SerializeField] UIAvailableUpgradeInfo availableUpgradeInfoPrefab;

    UpgradeCounter[] upgradeCounters;
    List<UIAvailableUpgradeInfo> infos = new List<UIAvailableUpgradeInfo>();

    private void Start()
    {
        upgradeCounters = FindObjectsOfType<UpgradeCounter>(true);
        foreach (var upgradeCounter in upgradeCounters)
        {
            var info = Instantiate(availableUpgradeInfoPrefab, transform);
            info.Setup(upgradeCounter);
            infos.Add(info);
        }
    }
}
