using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceStore 
{
   void OnPickupResource(ResourceType type, int lastCount, int addCount);

}
