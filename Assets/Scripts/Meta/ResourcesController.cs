using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ResourcesController : SingletonMono<ResourcesController>
{
    public event System.Action OnResourcesUpdated;
    [SerializeField] List<ResourceType> resourceTypes;


    public List<ResourceType> Resources => resourceTypes;

    void Start()
    {
        for (int i = 0; i < resourceTypes.Count; i++)
        {           
           resourceTypes[i].UpdateCount();
           resourceTypes[i].Count = 999; ////////////////////////////
        }
    }


    public void UpdateResources()
    {
        OnResourcesUpdated?.Invoke();
        Save();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < resourceTypes.Count; i++)
            {
                resourceTypes[i].Count += 100;
            }
            UpdateResources();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < resourceTypes.Count; i++)
            {
               Debug.Log(resourceTypes[i].name + " : " + resourceTypes[i].Count);
            }
        }
    }



    public void Save()
    {
        for(int i = 0; i < resourceTypes.Count; i++)
        {
            resourceTypes[i].Save();
        }
    }
}
