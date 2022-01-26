using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitHealthBars : SingletonMono<UIUnitHealthBars>
{
    [System.Serializable]
    struct UIUnitHealthBarSettings
    {
        public SubjectId uiHealthBarId;
        public UIUnitHealthBar healthBarPrefab;
    }


    [SerializeField] List<UIUnitHealthBarSettings> healthBarSettings = new List<UIUnitHealthBarSettings>();

    Dictionary<UnitHealthBar, UIUnitHealthBar> healthBars = new Dictionary<UnitHealthBar, UIUnitHealthBar>();


    public void CreateHealthBar(UnitHealthBar unit)
    {
        for (int i = 0; i < healthBarSettings.Count; i++)
        {
            if (healthBarSettings[i].uiHealthBarId == unit.UIHealthBarId)
            {
                UIUnitHealthBar instance = Instantiate(healthBarSettings[i].healthBarPrefab, transform);
                instance.Setup(unit, 3.5f);
                healthBars.Add(unit, instance);
                break;
            }
        }
       
    }


    public void RemoveHealthBar(UnitHealthBar unit)
    {
        if (healthBars.ContainsKey(unit))
        {
            healthBars[unit].Remove();
            healthBars.Remove(unit);
        }
    }
}
