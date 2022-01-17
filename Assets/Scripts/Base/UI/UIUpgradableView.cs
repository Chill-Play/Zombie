using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgradableView : MonoBehaviour
{
    [SerializeField] UpgradableInfoUI upgradableInfoUI;

    Transform target;
    Upgradable[] upgradables;

    private void Awake()
    {
        upgradables = FindObjectsOfType<Upgradable>(true);
        foreach (var upgradable in upgradables)
        {
            upgradable.OnUpgradeRangeEnter += () => Show(upgradable);
            upgradable.OnUpgradeRangeExit += () => Hide(upgradable);
        }
    }

    public void Show(Upgradable upgradable)
    {
        target = upgradable.UIPoint;
        upgradableInfoUI.Show(upgradable);         
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
            screenPos.z = 0f;
            upgradableInfoUI.transform.position = screenPos;
        }
    }


    public void Hide(Upgradable upgradable)
    {
        target = null;
        upgradableInfoUI.Hide();
    }
}
