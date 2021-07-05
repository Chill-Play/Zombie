using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim /Level Collection")]
public class LevelCollection : ScriptableObject
{
    [SerializeField] List<LevelPack> packs;

    public int GetPacksCount()
    {
        return packs.Count;
    }


    public LevelPack GetPack(int i)
    {
        return packs[i % packs.Count];
    }
}
