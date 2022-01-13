using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceSlot
{
    public ResourceType type;
    public int count;
    [HideInInspector] public bool opened;

    public ResourceSlot(ResourceType type, int count, bool opened = false)
    {
        this.type = type;
        this.count = count;
        this.opened = opened;
    }
}

[System.Serializable]
public class ResourcesInfo : ISerializationCallbackReceiver
{
    [SerializeField] List<ResourceSlot> slots = new List<ResourceSlot>();
    Dictionary<string, ResourceSlot> idsByTypes = new Dictionary<string, ResourceSlot>();
    public List<ResourceSlot> Slots => slots;
    


    public void Clear()
    {
        slots.Clear();
        idsByTypes.Clear();
    }


    public void ApplyTypes(ResourcesInfo resourcesInfo)
    {
        foreach(var slot in resourcesInfo.slots)
        {
            AddSlot(slot.type, 0);
        }
    }


    public int Count(ResourceType type)
    {
        return idsByTypes[type.saveId].count;
    }


    public void AddSlot(ResourceType type, int count)
    {
        ResourceSlot slot = new ResourceSlot(type, count);
        idsByTypes.Add(type.saveId, slot);
        slots.Add(slot);
    }


    public void AddSlot(ResourceSlot slot)
    {
        idsByTypes.Add(slot.type.saveId, slot);
        slots.Add(slot);
    }


    public void Add(ResourcesInfo resourcesInfo)
    {
        foreach(var slot in resourcesInfo.slots)
        {
            if (idsByTypes.TryGetValue(slot.type.saveId, out var thisSlot))
            {
                thisSlot.count += slot.count;
            }
        }
    }


    public void Add(ResourceType type, int count)
    {
        if(idsByTypes.TryGetValue(type.saveId, out var slot))
        {
            slot.count += count;
        }
        else
        {
            AddSlot(type, count);
        }
    }


    public void Spend(ResourcesInfo resourcesInfo, ResourcesInfo cost, int spendCount, System.Action<ResourceType, int, int> onSpendCallback = null)
    {
        int i = 0; 
        foreach (var slot in resourcesInfo.slots)
        {            
            if (idsByTypes.TryGetValue(slot.type.saveId, out var thisSlot))
            {
                i++;
                int spendAmount = Mathf.Min(spendCount, slot.count, cost.idsByTypes[slot.type.saveId].count - thisSlot.count);
                thisSlot.count += spendAmount;
                slot.count -= spendAmount;
                onSpendCallback?.Invoke(slot.type, spendAmount, i);
            }
        }
    }



    public void Subtract(ResourcesInfo resourcesInfo)
    {
        foreach (var slot in resourcesInfo.slots)
        {
            if (idsByTypes.TryGetValue(slot.type.saveId, out var thisSlot))
            {
                thisSlot.count -= slot.count;
            }
        }
    }


    public void Subtract(ResourceType type, int count)
    {
        if(idsByTypes.TryGetValue(type.saveId, out var slot))
        {
            slot.count -= count;
            if(slot.count < 0)
            {
                slot.count = 0;
            }
        }
        else
        {
            Debug.LogError("No such resource in collection : " + type.name);
        }
    }


    public bool IsFilled(ResourcesInfo resourcesInfo)
    {
        int resourcesFilled = 0;
        foreach (var slot in resourcesInfo.slots)
        {
            if (idsByTypes.TryGetValue(slot.type.saveId, out var thisSlot))
            {
                if (thisSlot.count <= slot.count)
                {
                    resourcesFilled++;
                }
            }
        }
        return resourcesFilled == slots.Count;
    }

    public void OnBeforeSerialize()
    {
        //
    }

    public void OnAfterDeserialize()
    {
        idsByTypes.Clear();
        foreach (var slot in slots)
        {           
            idsByTypes.Add(slot.type.saveId, slot);
        }
    }
}
