using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] SubjectId screenId;
    [SerializeField] SubjectId baseScreenId;
    [SerializeField] List<StatsType> stats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var screen = (UpgradesScreen)FindObjectOfType<UIController>().ShowScreen(screenId);
            var statsList = new List<(StatsType, StatInfo)>();
            foreach (var type in stats)
            {
                var info = FindObjectOfType<StatsManager>().GetStatInfo(type);
                statsList.Add((type, info));
            }
            screen.Show("TEST", statsList, FindObjectOfType<ResourcesController>().ResourcesCount, () => FindObjectOfType<UIController>().ShowScreen(baseScreenId));
        }   
    }
}
