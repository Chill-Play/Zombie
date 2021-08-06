using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public interface ISaveableMapData 
{
    event System.Action<ISaveableMapData> OnSave;

    string SaveId { get; set; }

    JSONNode GetSaveData();

    void Load(JSONNode loadData);
}
