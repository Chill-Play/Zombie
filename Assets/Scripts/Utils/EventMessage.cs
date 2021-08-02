using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EventMessage<T>
{
    public T data;
    public object sender;


    public EventMessage(T data, object sender)
    {
        this.data = data;
        this.sender = sender;
    }
}
