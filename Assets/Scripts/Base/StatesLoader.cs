using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatesLoader : SingletonMono<StatesLoader>
{
    public void LoadState(SceneReference sceneReference)
    {
        SceneManager.LoadScene(sceneReference);
    }
}
