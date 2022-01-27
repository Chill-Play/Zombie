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

    public void Setup(LevelService levelService)
    {
        levelService.OnLevelFinished += OnLevelCompleted;
        levelService.OnLevelFailed += OnLevelFailed;
        levelService.OnLevelStarted += OnLevelStarted;
    }

    void Start()
    {
        var zombiesLevelController = ZombiesLevelController.Instance;
        Setup(zombiesLevelController.RaidLevelService);
        Setup(zombiesLevelController.CampaignLevelService);

        if (MapController.Instance != null)
        {
            var buildables = FindObjectsOfType<Buildable>();
            foreach (var buildable in buildables)
            {
                buildable.OnBuilt += (x) => { if (!x) { OnBuildableBuilt(buildable); } };
            }
            var hq = FindObjectOfType<HQBuilding>();
            hq.OnLevelUp += OnHQLevelUp;
            var statsManager = StatsManager.Instance;
            statsManager.OnStatLevelUp += StatsManager_OnStatLevelUp;
            var cardController = FindObjectOfType<CardController>();
            cardController.OnCardUpgraded += CardController_OnCardUpgraded;
            var upgradables = FindObjectsOfType<Upgradable>();
            foreach (var upgradable in upgradables)
            {
                upgradable.OnLevelUp += () => Upgradable_OnLevelUp(upgradable);
            }             
        }
        var raiseTutorialsStage = FindObjectsOfType<RaiseTutorialStageEvent>();
        foreach (var raiseTutorialStage in raiseTutorialsStage)
        {
            raiseTutorialStage.OnRaiseTutorialStage += RaiseTutorialStage_OnRaiseTutorialStage;
        }
        var advertisementManager = AdvertisementManager.Instance;
        advertisementManager.OnReportAnalytics += AdvertisementManager_OnReportAnalytics;
    }

    private void RaiseTutorialStage_OnRaiseTutorialStage(string text)
    {
        var args = new Dictionary<string, object>();
        args.Add("step_name", text);     
        SendEvent("raise_tutorial_stage", args);
    }

    private void Upgradable_OnLevelUp(Upgradable upgradable)
    {
        var args = new Dictionary<string, object>();
        args.Add("upgradable", upgradable.gameObject.name);
        args.Add("level", upgradable.Level + 1);
        SendEvent("building_upgrade", args);
    }

    private void AdvertisementManager_OnReportAnalytics(AdvertisementManager.AnalyticsReport report)
    {
        var args = new Dictionary<string, object>();
        args.Add("ad_type", report.adType);
        args.Add("placement", report.placement);
        args.Add("result", report.result);
        args.Add("connection", report.connection);
        SendEvent(report.eventType, args);
    }

    void OnBuildableBuilt(Buildable buildable)
    {
        var args = new Dictionary<string, object>();
        args.Add("building", buildable.gameObject.name);
        SendEvent("building_built", args);
    }


    public void OnLevelCompleted(LevelInfo info)
    {
        var args = new Dictionary<string, object>();
        RaidAddArgs(info,args);
        SendEvent("raid_completed", args);
    }

    private void RaidAddArgs(LevelInfo info, Dictionary<string, object> args)
    {
        args.Add("level", info.levelNumber);
        args.Add("level_name", info.levelName);
        args.Add("game_mode", info.gameMode);
        args.Add("time", (int)(Time.realtimeSinceStartup - raidStart));    
        args.Add("connection", Application.internetReachability != NetworkReachability.NotReachable ? 1 : 0);
    }

    public void OnLevelFailed(LevelInfo info)
    {
        var args = new Dictionary<string, object>();
        RaidAddArgs(info, args);
        SendEvent("raid_completed", args);
    }


    public void OnLevelStarted(LevelInfo info)
    {
        var args = new Dictionary<string, object>();
        RaidAddArgs(info, args);
        SendEvent("raid_started", args);
    }


    public void OnHQLevelUp(int level)
    {
        var args = new Dictionary<string, object>();
        args.Add("level", level + 1);
        SendEvent("hq_level_up", args);
    }

    private void CardController_OnCardUpgraded(Card card, StatsType statType, int level)
    {
        var args = new Dictionary<string, object>();
        args.Add("character", card.CardName);
        args.Add("stat", statType.name);
        args.Add("level", level + 1);
        SendEvent("stat_level_up", args);
    }

    private void StatsManager_OnStatLevelUp((StatsType, int) obj)
    {
        var args = new Dictionary<string, object>();
        args.Add("character", "main_survivor");
        args.Add("stat", obj.Item1.saveId);
        args.Add("level", obj.Item2);
        SendEvent("stat_level_up", args);
    }


    public void OnScreenSwitched(string id)
    {
        if (currentScreenId != null)
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

    void SendEvent(string e, Dictionary<string, object> args)
    {
        Debug.Log("Send Unity Event : " + e + " : " + Environment.NewLine + string.Join(Environment.NewLine, args));
        AnalyticsEvent.Custom(e, args);
    }

    public void Unsubscribe(LevelService levelService)
    {
        levelService.OnLevelFinished -= OnLevelCompleted;
        levelService.OnLevelFailed -= OnLevelFailed;
        levelService.OnLevelStarted -= OnLevelStarted;
    }

    private void OnDisable()
    {
        var advertisementManager = AdvertisementManager.Instance;
        if (advertisementManager != null)
        {
            advertisementManager.OnReportAnalytics -= AdvertisementManager_OnReportAnalytics;
        }

        var zombiesLevelController = ZombiesLevelController.Instance;
        if (zombiesLevelController != null)
        {
            Unsubscribe(zombiesLevelController.RaidLevelService);
            Unsubscribe(zombiesLevelController.CampaignLevelService);
        }
    }
}

