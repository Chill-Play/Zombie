using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Level Pack")]
public class LevelPack : ScriptableObject
{
    [SerializeField] List<SceneReference> levels;


    public int GetLevelsCount()
    {
        return levels.Count;
    }


    public SceneReference GetLevel(int i)
    {
        return levels[i];
    }
}
