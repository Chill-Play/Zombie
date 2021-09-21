using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UnityAnalytics : MonoBehaviour
{
    void Start()
    {
        if (Level.Instance != null)
        {
            Level.Instance.OnLevelEnded += LevelController_OnLevelCompleted;
            Level.Instance.OnLevelFailed += LevelController_OnLevelFailed;
            Level.Instance.OnLevelStarted += LevelController_OnLevelStarted;
        }
        if(MapController.Instance != null)
        {
            var buildables = FindObjectsOfType<Buildable>();
            foreach(var buildable in buildables)
            {
                buildable.OnBuilt += (x) => { if (!x) { OnBuildableBuilt(buildable); } };
            }
            var hq = FindObjectOfType<HQBuilding>();
            hq.OnLevelUp += Hq_OnLevelUp;
            var statsManager = StatsManager.Instance;
            statsManager.OnStatLevelUp += StatsManager_OnStatLevelUp;
        }
    }

    private void StatsManager_OnStatLevelUp((StatsType, int) obj)
    {
        var args = new Dictionary<string, object>();
        args.Add("stat", obj.Item1.saveId);
        args.Add("level", obj.Item2);
        SendEvent("stat_level_up", args);
    }

    private void Hq_OnLevelUp()
    {
        var hq = FindObjectOfType<HQBuilding>();
        var args = new Dictionary<string, object>();
        args.Add("level", hq.Level + 1);
        SendEvent("hq_level_up", args);
    }

    void OnBuildableBuilt(Buildable buildable)
    {
        var args = new Dictionary<string, object>();
        args.Add("building", buildable.gameObject.name);
        SendEvent("building_built", args);
    }


    void LevelController_OnLevelCompleted()
    {
        var info = Level.Instance.GetLevelInfo();
        var args = new Dictionary<string, object>();
        args.Add("raid_number", info.levelNumber);
        SendEvent("raid_completed", args);
    }


    void LevelController_OnLevelFailed()
    {
        var info = Level.Instance.GetLevelInfo();
        var args = new Dictionary<string, object>();
        args.Add("raid_number", info.levelNumber);
        SendEvent("raid_failed", args);
    }


    void LevelController_OnLevelStarted()
    {
        var info = Level.Instance.GetLevelInfo();
        var args = new Dictionary<string, object>();
        args.Add("raid_number", info.levelNumber);
        SendEvent("raid_started", args);
    }


    void SendEvent(string e, Dictionary<string, object> args)
    {
#if UNITY_EDITOR
        Debug.Log("Send Unity Event : " + e + " : " + string.Join(Environment.NewLine, args));
#endif
        AnalyticsEvent.Custom(e, args);
    }
}
