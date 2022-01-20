using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnloadingResources
{  
    ResourceType ResourcesType { get; }
    int CurrentCount { get; }
    
    int MaxCount { get; }

    void Unload(int count);
 
}
