using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIInitializer : MonoBehaviour
{
    [SerializeField] SubjectId baseScreenId;
    // Start is called before the first frame update
    void Start()
    {
        UIController.Instance.ShowScreen(baseScreenId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
