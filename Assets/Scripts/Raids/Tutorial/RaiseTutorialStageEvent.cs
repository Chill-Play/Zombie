using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseTutorialStageEvent : MonoBehaviour
{
    public event System.Action<string> OnRaiseTutorialStage;

    [SerializeField] string stage;
   
    void Start()
    {
        GetComponent<ConditionTrigger>().OnTrigger += RaiseTutorialStageEvent_OnTrigger;   
    }

    private void RaiseTutorialStageEvent_OnTrigger()
    {
        OnRaiseTutorialStage?.Invoke(stage);
        AnalyticsService.Instance.SendTutorialEvent(stage);
    }
}
