using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialistBar : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text text;

    public void Setup(Card card)
    {
        icon.sprite = card.Icon;
        text.text = card.CardName;
    }
}