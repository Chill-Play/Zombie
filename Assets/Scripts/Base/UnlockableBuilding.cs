using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableBuilding : MonoBehaviour
{
    [SerializeField] int unlockLevel;


    public void SetLevel(int level)
    {
        if(level >= unlockLevel)
        {

        }
    }
}
