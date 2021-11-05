using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Level Collection")]
public class LevelCollection : ScriptableObject, ILevelCollection
{
    [SerializeField] List<LevelPack> packs;

    public int PacksCount => packs.Count;


    public ILevelPack GetPack(int i)
    {
        return packs[i % packs.Count];
    }
}

