using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySelectorUI : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] Transform cameraPosition;
    [SerializeField] float camZ;

    [SerializeField] Transform partySelectorButton;
    [SerializeField] Transform partySelectorEndButton;
    [SerializeField] Transform partySelectorPanel;
    [SerializeField] Transform cardsPanel;
    [SerializeField] GameObject mainCharVisualPrefab;
    [SerializeField] Transform mainCharPoint;

    [SerializeField] CardSlotUI cardSlotUIPrefab;
    [SerializeField] List<SpecialistVisualUI> specialistVisualUIs = new List<SpecialistVisualUI>();

    List<CardSlotUI> cardSlots = new List<CardSlotUI>();
    CardController cardController;
    GameObject mainCharVisual;

    private void Awake()
    {
        cardController = FindObjectOfType<CardController>();
        for (int i = 0; i < specialistVisualUIs.Count; i++)
        {
            specialistVisualUIs[i].OnSlotClicked += PartySelectorUI_OnSlotClicked;
            specialistVisualUIs[i].OnUpgradeClicked += PartySelectorUI_OnUpgradeClicked;
        }
    }

    private void PartySelectorUI_OnUpgradeClicked(CardSlot cardSlot)
    {
        cardController.UpgradeCard(cardSlot);
        UpdateSlots();
    }

    private void PartySelectorUI_OnSlotClicked(CardSlot cardSlot)
    {
        cardController.DeactivateCard(cardSlot);
        UpdateSlots();
    }

    void ShowPanel()
    {
        partySelectorButton.gameObject.SetActive(false);
        partySelectorPanel.gameObject.SetActive(true);
        partySelectorEndButton.gameObject.SetActive(true);
        cameraController.SetCameraPoint(cameraPosition, camZ);
        mainCharVisual = Instantiate(mainCharVisualPrefab, mainCharPoint.position, mainCharPoint.rotation);
        UpdateSlots();
    }

    void HidePanel()
    {
        partySelectorButton.gameObject.SetActive(true);
        partySelectorPanel.gameObject.SetActive(false);
        partySelectorEndButton.gameObject.SetActive(false);
        Destroy(mainCharVisual.gameObject);
        for (int i = 0; i < specialistVisualUIs.Count; i++)
        {
            specialistVisualUIs[i].Hide();
        }
        cameraController.ResetCameraPoint();
    }

    void UpdateSlots()
    {
        int i = 0;
        for (; i < cardController.DeckCards.cardSlots.Count; i++)
        {
            if (i < cardSlots.Count)
            {
                cardSlots[i].Setup(cardController.DeckCards.cardSlots[i]);
                cardSlots[i].Show();                
            }
            else
            {
                CardSlotUI cardSlotUI = Instantiate<CardSlotUI>(cardSlotUIPrefab, cardsPanel);
                cardSlots.Add(cardSlotUI);
                cardSlotUI.OnSlotClicked += CardSlotUI_OnSlotClicked;
                cardSlotUI.Setup(cardController.DeckCards.cardSlots[i]);
                cardSlotUI.Show();           
            }
        }

        for (; i < cardSlots.Count; i++)
        {
            cardSlots[i].Hide();
        }

        for (int j = 0; j < specialistVisualUIs.Count; j++)
        {
            if (j < cardController.ActiveCards.cardSlots.Count)
            {
                specialistVisualUIs[j].Setup(cardController.ActiveCards.cardSlots[j], cardController.CanUpgrade(cardController.ActiveCards.cardSlots[j]));
                specialistVisualUIs[j].Show();
            }
            else
            {
                specialistVisualUIs[j].Hide();
            }
        }   
    }

    private void CardSlotUI_OnSlotClicked(CardSlot card)
    {
        if (cardController.TryToActivateCard(card))
        {
            UpdateSlots(); 
        }
    }

    public void OnPartySelectorButtonClicked()
    {
        ShowPanel();
    }

    public void OnEndClicked()
    {
        HidePanel();
    }
}
