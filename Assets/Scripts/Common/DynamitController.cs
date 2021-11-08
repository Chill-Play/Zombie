using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamitController : MonoBehaviour
{
    [SerializeField] GameObject button;

    private int dynamitCount = 0;

    void Start() 
    {
        button.SetActive(false);
    }

    public void ShowButton(int count)
    {
        dynamitCount = count;
        button.SetActive(true);
    }

    public void SetDynamit()
    {
        dynamitCount--;
        if(dynamitCount <= 0)
        {
            button.SetActive(false);
        }
    }
}
