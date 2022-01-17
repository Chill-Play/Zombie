using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAvailableUpgradeInfo : MonoBehaviour
{
    [SerializeField] TMP_Text countText;

    UpgradeCounter upgradeCounter;
    bool active = false;

    public void Setup(UpgradeCounter upgradeCounter)
    {
        this.upgradeCounter = upgradeCounter;
        upgradeCounter.OnRequireUpdate += UpgradeCounter_OnRequireUpdate;
        gameObject.SetActive(false);
        UpdateInfo();
    }

    void UpdateInfo()
    {        
        if (upgradeCounter.gameObject.activeSelf && upgradeCounter.AvailableUpgrades() > 0)
        {
            countText.text = upgradeCounter.AvailableUpgrades().ToString();
            if (!active)
            {
                Show();
            }
            active = true;

        }
        else
        {
            if (active)
            {
                Hide();
            }
            active = false;
        }
    }

    private void LateUpdate()
    {
        if (active)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(upgradeCounter.transform.position);
            screenPos.z = 0f;
            transform.position = screenPos;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpgradeCounter_OnRequireUpdate()
    {
        UpdateInfo();
    }
}
