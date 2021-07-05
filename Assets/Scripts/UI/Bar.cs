using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] Image fill;
    
    public void SetValue(float value)
    {
        fill.fillAmount = value;
    }
}
