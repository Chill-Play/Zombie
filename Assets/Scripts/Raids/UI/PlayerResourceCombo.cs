using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceCombo : MonoBehaviour
{
    [SerializeField] float comboTime = 0.2f;
    [SerializeField] float showingTime = 0.8f;
    [SerializeField] float upOffset = 2f;
    [SerializeField]

    int resourceCombo = 0;
    float nextCheckCombo;

    UINumbers uiNumbers;
    UINumber uiNumber;

    private void Start()
    {
        uiNumbers = FindObjectOfType<UINumbers>();
    }

    private void OnEnable()
    {
        GetComponent<PlayerBackpack>().OnPickupResource += PlayerResourceCombo_OnPickupResource;
    }
    
    private void PlayerResourceCombo_OnPickupResource(ResourceType type, int lastCount, int addCount)
    {
        resourceCombo += addCount;
        nextCheckCombo = Time.time + comboTime;
        if (uiNumber == null)
        {
            uiNumber = uiNumbers.GetNumber(transform.position + Vector3.up * upOffset, "+" + resourceCombo.ToString(), Vector2.zero, 0f, 0f, false);
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

        if (resourceCombo > 0 && nextCheckCombo < Time.time)
        {
            uiNumbers.ScaleToZeroAndDestroy(uiNumber, 0.4f);
            uiNumber = null;
            resourceCombo = 0;
        }   
    }

    private void OnDisable()
    {
        GetComponent<PlayerBackpack>().OnPickupResource -= PlayerResourceCombo_OnPickupResource;
    }

}
