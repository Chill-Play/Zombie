using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesInfoUIPanel : MonoBehaviour
{
    [SerializeField] ResourceBar barPrefab;
    Dictionary<ResourceType, ResourceBar> barsByType = new Dictionary<ResourceType, ResourceBar>();

    private void Start()
    {

    }


    public void UpdateBar(ResourceType type, int totalCount)
    {
        ResourceBar bar = null;
        if (barsByType.ContainsKey(type))
        {
            bar = barsByType[type];
        }
        else
        {
            bar = Instantiate(barPrefab, transform);
            barsByType.Add(type, bar);
            bar.Setup(type, 0);
        }
        bar.UpdateValue(totalCount);
    }
}
