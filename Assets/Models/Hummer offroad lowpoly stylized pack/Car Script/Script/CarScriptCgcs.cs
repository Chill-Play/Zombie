using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScriptCgcs : MonoBehaviour {
    public float maxTorque = 500f;
    public float maxSteerAngle = 45f;
    public Transform t_CenterOfMass;
    public Transform[] wheelMesh = new Transform[4];
    public WheelCollider[] wheelCollider = new WheelCollider[4];
    private Rigidbody r_Rigidbody;

    public AudioSource highAccel;

    public Rigidbody rb;
    public float gearLength = 3;

    public float currentSpeed {get {return rb.velocity.magnitude * gearLength;}}
     public float lowPitch =1f;
     public float highPitch = 6f;
     public int numGears = 5;
     float rpm;
     int currentGear = 1;
     float currentGearPerc;
     public float maxSpeed =200;


	// Use this for initialization
	public void Start() 
        {
        r_Rigidbody = GetComponent<Rigidbody>();
        r_Rigidbody.centerOfMass = t_CenterOfMass.localPosition;
    }

       public void CalculateEngineSound() {

        float gearPercentage = (1 / (float)numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1),
            Mathf.Abs(currentSpeed / maxSpeed));

        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5.0f);

        var gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);

        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / (float)numGears) * (currentGear + 1);
        float downGearMax = (1 / (float)numGears) * currentGear;

        if (currentGear > 0 && speedPercentage < downGearMax) {

            currentGear--;
        }

        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1))) {

            currentGear++;
        }

        float pitch = Mathf.Lerp(lowPitch, highPitch, rpm);
        highAccel.pitch = Mathf.Min(highPitch, pitch) * 0.25f;

    }


	
	// Update is called once per frame
	public void Update()
    {
        UpdateMeshPosition();
        CalculateEngineSound();
        
	}

    public void FixedUpdate()
    {
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        float torque = Input.GetAxis("Vertical") * maxTorque;

        wheelCollider[0].steerAngle = steer;
        wheelCollider[1].steerAngle = steer;

        for(int i=0;i<4;i++)
        {
            wheelCollider[i].motorTorque = torque;
        }
    }
    public void UpdateMeshPosition()
    {

     
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 pos;

            wheelCollider[i].GetWorldPose(out pos, out quat);

            wheelMesh[i].position = pos;
            wheelMesh[i].rotation = quat;
        }
    }
}
