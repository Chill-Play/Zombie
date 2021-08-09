using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPackProgressBar : MonoBehaviour
{
    [SerializeField] Image knobPrefab;

    public void Setup(int currentLevel, int totalLevels)
    {
        for(int i = 0; i < totalLevels; i++)
        {
            Image knob = Instantiate(knobPrefab, transform);
            if (i >= currentLevel)
            {
                knob.color = Color.black;
            }
        }
    }
}
