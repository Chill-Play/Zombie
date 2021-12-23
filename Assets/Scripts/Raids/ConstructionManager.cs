using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    [SerializeField] Sprite constructionIcon;
    [SerializeField] Sprite repairIcon;
    [SerializeField] PickupableResource rewardStarPrefab;

    public Sprite ConstructionIcon => constructionIcon;
    public Sprite RepairIcon => repairIcon;
    public PickupableResource RewardStarPrefab => rewardStarPrefab;

}
