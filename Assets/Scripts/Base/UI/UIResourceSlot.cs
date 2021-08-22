using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Color defaultColor;
    [SerializeField] Color lackingColor;
    [SerializeField] TMP_Text count;


    public void SetSlot(ResourceSlot slot)
    {
        count.text = slot.count.ToString();
        icon.sprite = slot.type.icon;
    }


    public void SetSlot(ResourceSlot slot, ResourceSlot availableSlot)
    {
        count.text = slot.count.ToString();
        icon.sprite = slot.type.icon;
        count.color = (slot.count > availableSlot.count) ? lackingColor : defaultColor;
    }
}
