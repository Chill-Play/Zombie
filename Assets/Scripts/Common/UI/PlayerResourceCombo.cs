using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceCombo : MonoBehaviour
{
    [SerializeField] float comboTime = 0.2f;
    [SerializeField] float showingTime = 0.8f;
    [SerializeField] float upOffset = 2f;
  

    int resourceCombo = 0;
    float nextCheckCombo;

    UINumbers uiNumbers;
    UINumber uiNumber;

    IComboCounter[] comboCounters;

    private void Awake()
    {
        uiNumbers = FindObjectOfType<UINumbers>();
        comboCounters = GetComponents<IComboCounter>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < comboCounters.Length; i++)
        {
            comboCounters[i].OnAddingPoints += OnAddingPoints;
        }       
    }

    private void OnAddingPoints(Sprite icon, int addCount)
    {
        resourceCombo += addCount;
        nextCheckCombo = Time.time + comboTime;
        if (uiNumber == null)
        {
            uiNumber = uiNumbers.GetNumber(transform.position + Vector3.up * upOffset, "+" + resourceCombo.ToString(), Vector2.zero, 0f, 0f, false);
            if (icon != null)
            {
                uiNumbers.AttachImage(uiNumber, icon);
            }
        }
        uiNumber.text.text = "+" + resourceCombo.ToString();
        uiNumbers.PunchScaleNumber(uiNumber, 0.2f, 0.4f);
    }

    void Update()
    {
        if (uiNumber != null)
        {
            uiNumbers.FollowPosition(uiNumber, transform.position + Vector3.up * upOffset);
        }

        if (uiNumber != null && resourceCombo > 0 && nextCheckCombo < Time.time)
        {
            uiNumbers.ScaleToZeroAndDestroy(uiNumber, 0.4f);
            uiNumber = null;
            resourceCombo = 0;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < comboCounters.Length; i++)
        {
            comboCounters[i].OnAddingPoints -= OnAddingPoints;
        }

        if (uiNumber != null)
        {
            uiNumbers.ScaleToZeroAndDestroy(uiNumber, 0.2f);
            uiNumber = null;
            resourceCombo = 0;
        }
    }
}
