using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    ScrewFactory factory;
    // Start is called before the first frame update
    void Start()
    {
        factory = FindObjectOfType<ScrewFactory>();
    }

    void SwitchFactoryLod()
    {
        factory.StartSwichLod();
    }
}
