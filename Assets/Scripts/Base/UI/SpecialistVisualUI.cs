using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialistVisualUI : MonoBehaviour
{
    public event System.Action<CardSlot> OnSlotClicked;

    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text cardName;
    [SerializeField] Transform specialistPosition;

    GameObject unitVisual;
    CardSlot cardSlot;

    public void Setup(CardSlot cardSlot)
    {
        this.cardSlot = cardSlot;
        cardName.text = cardSlot.card.CardName;
        level.text = cardSlot.level.ToString();
    }

    public void Show()
    {
        if (unitVisual != null)
        {
            Destroy(unitVisual);
        }
        unitVisual = Instantiate(cardSlot.card.UnitVisual, specialistPosition.position, specialistPosition.rotation);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (unitVisual != null)
        {
            Destroy(unitVisual);
        }
        gameObject.SetActive(false);
    }

    public void OnButtonClicked()
    {
        OnSlotClicked?.Invoke(cardSlot);
    }
}
