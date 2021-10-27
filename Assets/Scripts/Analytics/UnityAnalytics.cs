using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UnityAnalytics : SingletonMono<UnityAnalytics>
{
    static float screenOpenTime = 0f;
    static string currentScreenId;

    float raidStart;

    private void Awake()
    {
        raidStart = Time.realtimeSinceStartup;
    }

    //    void Start()
    //    {
    //        if (Level.Instance != null)
    //        {
    //            Level.Instance.OnLevelEnded += LevelController_OnLevelCompleted;
    //            Level.Instance.OnLevelFailed += LevelController_OnLevelFailed;
    //            Level.Instance.OnLevelStarted += LevelController_OnLevelStarted;
    //        }
    //        if(MapController.Instance != null)
    //        {
    //            var buildables = FindObjectsOfType<Buildable>();
    //            foreach(var buildable in buildables)
    //            {
    //                buildable.OnBuilt += (x) => { if (!x) { OnBuildableBuilt(buildable); } };
    //            }
    //            var hq = FindObjectOfType<HQBuilding>();
    //            hq.OnLevelUp += Hq_OnLevelUp;
    //            var statsManager = StatsManager.Instance;
    //            statsManager.OnStatLevelUp += StatsManager_OnStatLevelUp;
    //        }
    //    }

    //    private void StatsManager_OnStatLevelUp((StatsType, int) obj)
    //    {
    //        var args = new Dictionary<string, object>();
    //        args.Add("stat", obj.Item1.saveId);
    //        args.Add("level", obj.Item2);
    //        SendEvent("stat_level_up", args);
    //    }


    //    void OnBuildableBuilt(Buildable buildable)
    //    {
    //        var args = new Dictionary<string, object>();
    //        args.Add("building", buildable.gameObject.name);
    //        SendEvent("building_built", args);
    //    }


    public void OnLevelCompleted(LevelInfo info, int tries)
    {
        var args = new Dictionary<string, object>();
        RaidAddArgs(info, tries, args);
        SendEvent("raid_completed", args);
    }

    private void RaidAddArgs(LevelInfo info, int tries, Dictionary<string, object> args)
    {
        args.Add("level", info.levelNumber);
        args.Add("time", (int)(Time.realtimeSinceStartup - raidStart));
        args.Add("tries", tries);
        args.Add("connection", Application.internetReachability != NetworkReachability.NotReachable ? 1 : 0);
    }

    public void OnLevelFailed(LevelInfo info, int tries)
    {
        var args = new Dictionary<string, object>();
        RaidAddArgs(info, tries, args);
        SendEvent("raid_completed", args);
    }


    public void OnLevelStarted(LevelInfo info)
    {
        var args = new Dictionary<string, object>();
        RaidAddArgs(info, 0, args);
        SendEvent("raid_started", args);
    }


    public void OnHQLevelUp(int lvl)
    {
        var args = new Dictionary<string, object>();
        args.Add("level", lvl + 1);
        SendEvent("hq_level_up", args);
    }


    public void OnStatLevelUp(string stat, int lvl)
    {
        var args = new Dictionary<string, object>();
        args.Add("level", lvl + 1);
        args.Add("stat", stat);
        SendEvent("stat_level_up", args);
    }


    public void OnScreenSwitched(string id)
    {
        if(currentScreenId != null)
        {
            id = id.ToSnakeCase();
            var args = new Dictionary<string, object>();
            args.Add("time", (int)(Time.realtimeSinceStartup - screenOpenTime));
            args.Add("screen", id);
            SendEvent("screen_switch", args);
        }
        screenOpenTime = Time.realtimeSinceStartup;
        currentScreenId = id;
    }


    public void AdsEvent(string eventType, string adType, string placement, string result, int connection)
    {
        var args = new Dictionary<string, object>();
        args.Add("ad_type", adType);
        args.Add("placement", placement);
        args.Add("result", result);
        args.Add("connection", connection);
        SendEvent(eventType, args);
    }


    void SendEvent(string e, Dictionary<string, object> args)
    {
        Debug.Log("Send Unity Event : " + e + " : " + string.Join(Environment.NewLine, args));
        AnalyticsEvent.Custom(e, args);
    }
}
