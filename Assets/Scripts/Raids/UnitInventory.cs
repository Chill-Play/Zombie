using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    [SerializeField] GameObject defaultItem;

    GameObject currentItem;

    private void Awake()
    {
        currentItem = defaultItem;
        currentItem.SetActive(true);
    }

    public void SetActiveItem(GameObject item)
    {
        currentItem.SetActive(false);
        currentItem = item;
        currentItem.SetActive(true);
    }

    public void ResetActiveItem()
    {
        SetActiveItem(defaultItem);
    }

}
