using System;
using System.Collections.Generic;
using UnityEngine;


public class AnalyticsManager : SingletonMono<AnalyticsManager>
{
    float levelStartTime;

    //void OnEnable()
    //{
    //    if (Level.Instance != null)
    //    {
    //        Level.Instance.OnLevelEnded += LevelController_OnLevelCompleted;
    //        Level.Instance.OnLevelFailed += LevelController_OnLevelFailed;
    //        Level.Instance.OnLevelStarted += LevelController_OnLevelStarted;
    //    }
    //}



    //void OnDisable()
    //{
    //    if (Level.Instance != null)
    //    {
    //        Level.Instance.OnLevelEnded -= LevelController_OnLevelCompleted;
    //        Level.Instance.OnLevelFailed -= LevelController_OnLevelFailed;
    //        Level.Instance.OnLevelStarted -= LevelController_OnLevelStarted;
    //    }
    //}

    public void SendTutorialEvent(string name)
    {
        ReportEvent(name, null);
        AppMetrica.Instance.SendEventsBuffer();
    }


    public void OnLevelCompleted(LevelInfo info)
    {
        SendLevelCompleted(info);
    }


    public void OnLevelFailed(LevelInfo info)
    {
        SendLevelFailed(info);
    }


    public void OnLevelStarted(LevelInfo info)
    {
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
        p.Add("level_number", info.levelId + 1);
        p.Add("level_name", info.levelName);
        p.Add("level_count", info.levelNumber + 1);
        p.Add("level_diff", "medium");
        p.Add("level_loop", info.loop);
        p.Add("level_random", 0);
        p.Add("level_type", "normal");
        p.Add("game_mode", "classic");
    }


    void AddLevelInfoFinishParams(LevelInfo info, Dictionary<string, object> p)
    {
        p.Add("time", (int)(Time.time - levelStartTime));
    }



    public void SendLevelFailed(LevelInfo info)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        AddLevelInfoToParams(info, p);
        p.Add("result", "lose");
        p.Add("progress", 0);
        AddLevelInfoFinishParams(info, p);
        ReportEvent("level_finish", p);
        AppMetrica.Instance.SendEventsBuffer();
    }


    public void SendLevelCompleted(LevelInfo info)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        AddLevelInfoToParams(info, p);
        p.Add("result", "win");
        p.Add("progress", 100);
        AddLevelInfoFinishParams(info, p);
        ReportEvent("level_finish", p);
        AppMetrica.Instance.SendEventsBuffer();
    }


    public void ReportEvent(string id, Dictionary<string, object> parameters)
    {
        if (parameters == null)
        {
            AppMetrica.Instance.ReportEvent(id);
#if UNITY_EDITOR
            Debug.Log("Analytics event : " + id);
#endif
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Analytics event : " + id + " : " + string.Join(Environment.NewLine, parameters));
#endif
            AppMetrica.Instance.ReportEvent(id, parameters);
        }
    }
}
