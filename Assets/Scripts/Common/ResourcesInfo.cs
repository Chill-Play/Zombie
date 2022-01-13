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
public class ResourcesInfo 
{
    [SerializeField] List<ResourceSlot> slots = new List<ResourceSlot>();
    Dictionary<ResourceType, ResourceSlot> idsByTypes = new Dictionary<ResourceType, ResourceSlot>();

    bool initialized = false;

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
        if (!initialized)
        {
            Initialize();
        }
        return idsByTypes[type].count;
    }


    public void AddSlot(ResourceType type, int count)
    {
        if (!initialized)
        {
            Initialize();
        }
        ResourceSlot slot = new ResourceSlot(type, count);
        idsByTypes.Add(type, slot);
        slots.Add(slot);
    }


    public void AddSlot(ResourceSlot slot)
    {
        if (!initialized)
        {
            Initialize();
        }
        idsByTypes.Add(slot.type, slot);
        slots.Add(slot);
    }


    public void Add(ResourcesInfo resourcesInfo)
    {
        if (!initialized)
        {
            Initialize();
        }
        foreach (var slot in resourcesInfo.slots)
        {          
            if (idsByTypes.TryGetValue(slot.type, out var thisSlot))
            {
                thisSlot.count += slot.count;
            }
        }
    }


    public void Add(ResourceType type, int count)
    {
        if (!initialized)
        {
            Initialize();
        }
        if (idsByTypes.TryGetValue(type, out var slot))
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
        if (!initialized)
        {
            Initialize();
        }
        int i = 0; 
        foreach (var slot in resourcesInfo.slots)
        {            
            if (idsByTypes.TryGetValue(slot.type, out var thisSlot))
            {
                i++;
                int spendAmount = Mathf.Min(spendCount, slot.count, cost.idsByTypes[slot.type].count - thisSlot.count);
                thisSlot.count += spendAmount;
                slot.count -= spendAmount;
                onSpendCallback?.Invoke(slot.type, spendAmount, i);
            }
        }
    }



    public void Subtract(ResourcesInfo resourcesInfo)
    {
        if (!initialized)
        {
            Initialize();
        }
        foreach (var slot in resourcesInfo.slots)
        {
            if (idsByTypes.TryGetValue(slot.type, out var thisSlot))
            {
                thisSlot.count -= slot.count;
            }
        }
    }


    public void Subtract(ResourceType type, int count)
    {
        if (!initialized)
        {
            Initialize();
        }
        if (idsByTypes.TryGetValue(type, out var slot))
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
        if (!initialized)
        {
            Initialize();
        }
        int resourcesFilled = 0;
        foreach (var slot in resourcesInfo.slots)
        {
            if (idsByTypes.TryGetValue(slot.type, out var thisSlot))
            {
                if (thisSlot.count <= slot.count)
                {
                    resourcesFilled++;
                }
            }
        }
        return resourcesFilled == slots.Count;
    }


    public void Initialize()
    {
        initialized = true;
        idsByTypes.Clear();
        foreach (var slot in slots)
        {
            idsByTypes.Add(slot.type, slot);
        }
    }
}
