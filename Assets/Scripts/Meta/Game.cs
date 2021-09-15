using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : SingletonMono<Game>
{

    private void Awake()
    {
        for (int i = 0; i <= 40; i += 10)
        {
            Debug.Log(i);
        }
    }

    //public void ToGlobalMap()
    //{
    //    LevelController.Instance.ToGlobalMap();
    //}
    public void RunRaid()
    {
       LevelController.Instance.NextRaid();
    }
}
