using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] Vector3 _direction;
    [SerializeField] float _min = 0;
    [SerializeField] float _max = 360;
    
    [ContextMenu("Set random rotation")]
    void RandomRotate ()
    {
        int _childCount = gameObject.transform.childCount;
        Vector3 _rotation;

        for (int i = 0; i < _childCount; i++)
        {
            _rotation = _direction * Random.Range(_min, _max);
            transform.GetChild(i).localEulerAngles = _rotation;
        }
    }
}
