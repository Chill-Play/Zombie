using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceCombo : MonoBehaviour, IResourceStore
{
    [SerializeField] float comboTime = 0.2f;
    [SerializeField] float showingTime = 0.8f;
    [SerializeField] float upOffset = 2f;


    int resourceCombo = 0;
    float nextCheckCombo;

    UINumbers uiNumbers;
    UINumber uiNumber;

    PlayerBackpack playerBackpack;

    private void Start()
    {
        uiNumbers = FindObjectOfType<UINumbers>();        
    }

    private void OnEnable()
    {
        playerBackpack = GetComponent<PlayerBackpack>();
        if (playerBackpack != null)
        {
            playerBackpack.OnPickupResource += PlayerResourceCombo_OnPickupResource;
        }
    }
    
    private void PlayerResourceCombo_OnPickupResource(ResourceType type, int lastCount, int addCount)
    {
        resourceCombo += addCount;
        nextCheckCombo = Time.time + comboTime;
        if (uiNumber == null)
        {
            uiNumber = uiNumbers.GetNumber(transform.position + Vector3.up * upOffset, "+" + resourceCombo.ToString(), Vector2.zero, 0f, 0f, false);
            uiNumbers.AttachImage(uiNumber, type.icon);
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
        if (playerBackpack != null)
        {
            playerBackpack.OnPickupResource -= PlayerResourceCombo_OnPickupResource;
        }
        if (uiNumber != null)
        {
            uiNumbers.ScaleToZeroAndDestroy(uiNumber, 0.2f);
            uiNumber = null;
            resourceCombo = 0;
        }
    }

    public void OnPickupResource(ResourceType type, int lastCount, int addCount)
    {
        PlayerResourceCombo_OnPickupResource(type, lastCount, addCount);
    }
}
