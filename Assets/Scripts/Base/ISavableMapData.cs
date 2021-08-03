using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public interface ISavableMapData 
{
    string SaveId { get; set; }

    JSONNode GetSaveData();

    void Load(JSONNode loadData);
}
