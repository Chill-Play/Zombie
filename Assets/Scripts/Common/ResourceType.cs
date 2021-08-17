using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Resource Type")]
public class ResourceType : ScriptableObject
{
    public string saveId;
    public Sprite icon;
    public Resource defaultPrefab;
}
