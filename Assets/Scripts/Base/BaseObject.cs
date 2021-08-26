using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseObject : MonoBehaviour, ISerializationCallbackReceiver
{
    public event System.Action OnRequireSave;
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


    protected void RequireSave()
    {
        OnRequireSave?.Invoke();
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
