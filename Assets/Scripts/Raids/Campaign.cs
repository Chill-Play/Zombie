using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campaign : MonoBehaviour
{
    [SerializeField] List<Unit> units = new List<Unit>();
    [SerializeField] Sprite constructionIcon;
    [SerializeField] Sprite repairIcon;

    public Sprite ConstructionIcon => constructionIcon;
    public Sprite RepairIcon => repairIcon;

    void Start()
    {
        for (int i = 0; i < units.Count; i++)
        {
            Squad squad = FindObjectOfType<Squad>();
            Unit instance = Instantiate(units[i], squad.Units[0].transform.position, squad.transform.rotation);
            squad.AddUnit(instance);
        }
    }

   
}
