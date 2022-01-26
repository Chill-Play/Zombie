using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveController : SingletonMono<ReviveController>
{
    public event System.Action OnRevive;

    [SerializeField] Bomb bombPrefab;
    [SerializeField] Unit mainSurvivor;
    [SerializeField] Unit pickupSurvivor;


    public Bomb BombPrefab => bombPrefab;
    public Unit MainSurvivor => mainSurvivor;
    public Unit PickupSurvivor => pickupSurvivor;

    public bool ReviveOption { get; set; } = true; 

    public bool ReviveClicked()
    {
       /* var available = AdvertisementManager.Instance.RewardedAvailable;
        AdvertisementManager.Instance.ShowRewardedVideo((result) =>
        {
            if (result) Revive();
        }, "raid_revive");
        return available;*/
        Revive();
        return true;
    }

    public void Revive()
    {
        ReviveOption = false;  
        OnRevive?.Invoke();
    }

}
