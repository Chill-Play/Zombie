using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Level Collection")]
public class LevelCollection : ScriptableObject, ILevelCollection
{
    [SerializeField] string id;
    [SerializeField] List<LevelPack> packs;

    public int PacksCount => packs.Count;

    public string Id => id;

    public ILevelPack GetPack(int i)
    {
        return packs[i % packs.Count];
    }
}

