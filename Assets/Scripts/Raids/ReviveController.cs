using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveController : MonoBehaviour
{
    public event System.Action OnRevive;

    public bool ReviveOption { get; set; } = true; 

    public bool ReviveClicked()
    {
        var available = AdvertisementManager.Instance.RewardedAvailable;
        AdvertisementManager.Instance.ShowRewardedVideo((result) =>
        {
            if (result) Revive();
        }, "raid_revive");
        return available;

    }

    public void Revive()
    {
        ReviveOption = false;  
        OnRevive?.Invoke();
    }

}
