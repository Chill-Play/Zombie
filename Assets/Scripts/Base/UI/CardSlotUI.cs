using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSlotUI : MonoBehaviour
{
    public event System.Action<CardSlot> OnSlotClicked;

    [SerializeField] Image icon;
    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text cardName;

    CardSlot cardSlot;  

    public void Setup(CardSlot cardSlot)
    {
        icon.sprite = cardSlot.card.Icon;
        cardName.text = cardSlot.card.CardName;
        level.text = cardSlot.level.ToString();
        this.cardSlot = cardSlot;
    }

    public void Show()
    {        
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnButtonClicked()
    {
        OnSlotClicked?.Invoke(cardSlot);
    }
}
