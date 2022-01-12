using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockerTag : MonoBehaviour
{
    [SerializeField] TMP_Text levelLabel;
    [SerializeField] private SpriteRenderer locked;
    [SerializeField] private Sprite unlockedSprite;

    public void SetLevel(int level)
    {
        levelLabel.text = "LVL " + level;
    }

    public void Unlock()
    {
        locked.sprite = unlockedSprite;
    }

}
