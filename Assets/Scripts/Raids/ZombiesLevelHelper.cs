using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZombiesLevelHelper : MonoBehaviour
{
    IEnumerable<IZombiesLevelPhases> zombiesLevelPhases;

    private void Awake()
    {
        Level level = FindObjectOfType<Level>();
        level.OnLevelStarted += Level_OnLevelStarted;
        level.OnLevelEnded += Level_OnLevelEnded;
        level.OnLevelFailed += Level_OnLevelFailed;

        Raid raid = FindObjectOfType<Raid>();
        raid.OnHordeDefeated += Raid_OnHordeDefeated;

        zombiesLevelPhases = FindObjectsOfType<MonoBehaviour>().OfType<IZombiesLevelPhases>();
    }

    private void Raid_OnHordeDefeated()
    {
        foreach (IZombiesLevelPhases elt in zombiesLevelPhases)
        {
            elt.OnHordeDefeated();
        }
    }

    private void Level_OnLevelFailed()
    {
        foreach (IZombiesLevelPhases elt in zombiesLevelPhases)
        {
            elt.OnLevelFailed();
        }
    }

    private void Level_OnLevelEnded()
    {
        foreach (IZombiesLevelPhases elt in zombiesLevelPhases)
        {
            elt.OnLevelEnded();
        }
    }

    private void Level_OnLevelStarted()
    {
        foreach (IZombiesLevelPhases elt in zombiesLevelPhases)
        {
            elt.OnLevelStarted();
        }
    }
}
