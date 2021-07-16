using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BaseLoader : MonoBehaviour
{
    [SerializeField] List<SceneReference> statesScenes = new List<SceneReference>();

    private void Awake()
    {
        if (PlayerPrefs.HasKey("active_scene"))
        {
            string scenePath = PlayerPrefs.GetString("active_scene");
            for (int i = 0; i < statesScenes.Count; i++)
            {
                if (statesScenes[i].ScenePath == scenePath)
                {
                    SceneManager.LoadScene(statesScenes[i]);
                }
            }
        }
        else
        {
            SceneManager.LoadScene(statesScenes[0]);
        }
    }
}
