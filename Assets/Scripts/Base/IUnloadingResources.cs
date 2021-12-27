using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnloadingResources
{  
    ResourceType ResourcesType { get; }
    int CurrentCount { get; }

    void Unload(int count);
 
}
