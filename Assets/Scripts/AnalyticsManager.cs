using System;
using System.Collections.Generic;
using UnityEngine;


public class AnalyticsManager : SingletonMono<AnalyticsManager>
{
    float levelStartTime;

    void OnEnable()
    {
        if (Level.Instance != null)
        {
            Level.Instance.OnLevelEnded += LevelController_OnLevelCompleted;
            Level.Instance.OnLevelFailed += LevelController_OnLevelFailed;
            Level.Instance.OnLevelStarted += LevelController_OnLevelStarted;
        }
    }



    void OnDisable()
    {
        if (Level.Instance != null)
        {
            Level.Instance.OnLevelEnded -= LevelController_OnLevelCompleted;
            Level.Instance.OnLevelFailed -= LevelController_OnLevelFailed;
            Level.Instance.OnLevelStarted -= LevelController_OnLevelStarted;
        }
    }


    void LevelController_OnLevelCompleted()
    {
        var info = Level.Instance.GetLevelInfo();
        SendLevelCompleted(info);
    }


    void LevelController_OnLevelFailed()
    {
        var info = Level.Instance.GetLevelInfo();
        SendLevelFailed(info);
    }


    void LevelController_OnLevelStarted()
    {
        var info = Level.Instance.GetLevelInfo();
        levelStartTime = Time.time;
        SendLevelStarted(info);
    }


    public void SendLevelStarted(LevelInfo info)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        AddLevelInfoToParams(info, p);
        ReportEvent("level_start", p);
        AppMetrica.Instance.SendEventsBuffer();
    }



    void AddLevelInfoToParams(LevelInfo info, Dictionary<string, object> p)
    {
        p.Add("level_number", info.levelId);
        p.Add("level_name", info.levelName);
        p.Add("level_count", info.levelNumber);
        p.Add("level_diff", "medium");
        p.Add("level_loop", info.loop);
        p.Add("level_random", 0);
        p.Add("level_type", "normal");
        p.Add("game_mode", "classic");
    }


    void AddLevelInfoFinishParams(LevelInfo info, Dictionary<string, object> p)
    {
        p.Add("time", (int)(Time.time - levelStartTime));
        p.Add("progress", (int)(info.progress) * 100);
    }



    public void SendLevelFailed(LevelInfo info)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        AddLevelInfoToParams(info, p);
        p.Add("result", "lose");
        AddLevelInfoFinishParams(info, p);
        ReportEvent("level_finish", p);
        AppMetrica.Instance.SendEventsBuffer();
    }


    public void SendLevelCompleted(LevelInfo info)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        AddLevelInfoToParams(info, p);
        p.Add("result", "win");
        AddLevelInfoFinishParams(info, p);
        ReportEvent("level_finish", p);
        AppMetrica.Instance.SendEventsBuffer();
    }


    public void ReportEvent(string id, Dictionary<string, object> parameters)
    {
        Debug.Log("Analytics event : " + id + " : " + string.Join(Environment.NewLine, parameters));
        AppMetrica.Instance.ReportEvent(id, parameters);
    }
}
