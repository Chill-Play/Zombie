using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Chest Info")]
public class ChestInfo : ScriptableObject
{
    [SerializeField] Sprite icon;
    [SerializeField] Sprite openedIcon;

    public Sprite Icon => icon;
    public Sprite OpenedIcon => openedIcon;
}
