using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Replacer : MonoBehaviour
{
    [SerializeField] private GameObject _item;
    [SerializeField] private bool _rotation = true;
    [SerializeField] private bool _scale = false;
    
    [ContextMenu("Replace All Children")]
    void ReplaceChildren ()
    {
        int _childCount = gameObject.transform.childCount;
        Quaternion _newRotation;
        GameObject _newItem;

        for (int i = 0; i < _childCount; i++)
        {
            if (_rotation)
            {
                _newRotation = transform.GetChild(i).transform.rotation;
            }
            else
            {
                _newRotation = Quaternion.identity;
            }
            
            //_newItem = Instantiate(_item, transform.GetChild(i).transform.position, transform.GetChild(i).transform.rotation, gameObject.transform);

            
            _newItem = PrefabUtility.InstantiatePrefab(_item, gameObject.transform) as GameObject;
            _newItem.transform.position = transform.GetChild(i).transform.position;
            _newItem.transform.rotation = transform.GetChild(i).transform.rotation;

            if (_scale)
            {
                _newItem.transform.localScale = transform.GetChild(i).transform.localScale;
            }

            transform.GetChild(i).gameObject.SetActive(false);
            
        }
    }
    
}
