using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseObject : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] protected string id;
    public string Id {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }


    public virtual void BaseAfterDeserialize()
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Object id is null : " + gameObject.name);
        }
    }

//#if UNITY_EDITOR
    public void OnAfterDeserialize()
    {
        if(string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }

    public void OnBeforeSerialize()
    {

    }
//#endif
}
