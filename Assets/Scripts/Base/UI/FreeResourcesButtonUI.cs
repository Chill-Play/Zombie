using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeResourcesButtonUI : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text countText;

    public void Show(ResourceType resourceType, int count, System.Action<ResourceType, int> callback)
    {
        gameObject.SetActive(true);
        button.onClick.AddListener(() => {button.onClick.RemoveAllListeners(); callback?.Invoke(resourceType, count);});
        icon.sprite = resourceType.icon;
        countText.text = "+" + count.ToString();
    }

    public void Hide()
    {
        button.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
  
}
