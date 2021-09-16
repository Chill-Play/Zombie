using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnPointPack : MonoBehaviour
{
    [SerializeField] SubjectId sectionId;   

    [SerializeField]List<Transform> spawnPoints = new List<Transform>();


    public List<Transform> SpawnPoints => spawnPoints;
    public SubjectId SectionId => sectionId;
}
