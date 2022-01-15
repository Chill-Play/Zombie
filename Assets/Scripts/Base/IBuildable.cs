using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildable
{
    bool CanBuild { get; }

    void SpendResources(ResourcesInfo info, int count);
}
