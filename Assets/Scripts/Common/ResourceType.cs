using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Resource Type")]
public class ResourceType : ScriptableObject
{
    public string saveId;
    public Sprite icon;
    public Resource defaultPrefab;

    public int Count { get; set; }


    public void OnEnable()
    {
        UpdateCount();
    }

    public void UpdateCount()
    {
        Count = PlayerPrefs.GetInt(saveId, 0);
    }

    public void Save()
    {
        PlayerPrefs.SetInt(saveId, Count);
    }
}
