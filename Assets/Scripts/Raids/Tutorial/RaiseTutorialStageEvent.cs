using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseTutorialStageEvent : MonoBehaviour
{
    [SerializeField] string stage;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ConditionTrigger>().OnTrigger += RaiseTutorialStageEvent_OnTrigger;   
    }

    private void RaiseTutorialStageEvent_OnTrigger()
    {
        AnalyticsManager.Instance.SendTutorialEvent(stage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
