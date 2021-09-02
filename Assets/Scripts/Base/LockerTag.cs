using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockerTag : MonoBehaviour
{
    [SerializeField] TMP_Text levelLabel;
    

    public void SetLevel(int level)
    {
        levelLabel.text = "LVL " + level;
    }
}
