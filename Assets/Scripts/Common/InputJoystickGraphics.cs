using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJoystickGraphics : MonoBehaviour
{
    [SerializeField] 
    InputJoystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        joystick = GetComponent<InputJoystick>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
}
