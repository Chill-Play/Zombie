using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Level Pack")]
public class LevelPack : ScriptableObject , ILevelPack
{
    [SerializeField] List<SceneReference> levels;

    public int LevelsCount => levels.Count;

    public int GetLevelsCount()
    {
        return levels.Count;
    }


    public SceneReference GetLevel(int i)
    {
        return levels[i % levels.Count];
    }

    public string GetLevelName(int level)
    {
        return levels[level].ToString();
    }

    public string GetLevelType(int level)
    {
        return  "normal"; 
    }
}
