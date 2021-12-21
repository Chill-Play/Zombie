using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    [SerializeField] Sprite constructionIcon;
    [SerializeField] Sprite repairIcon;

    public Sprite ConstructionIcon => constructionIcon;
    public Sprite RepairIcon => repairIcon;

}
