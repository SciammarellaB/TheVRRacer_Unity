using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMainScript : MonoBehaviour
{
    public WheelCollider[] wheelsPhysics;
    public Transform[] wheelsMesh;

    public Rigidbody carRb;

    public Text speedText;

    public AudioSource engineSound;

    public GameObject steeringWheel;

    public float maxSteerAngle,actualSteerAngle;

    public float maxEngineTorque,actualEngineTorque;

    public float maxSpeed,gearMaxSpeed,gearMinSpeed,gear,currentSpeed;

    public float GForceValue,lastFrameSpeed;


    // Start is called before the first frame update
    void Start()
    {
        maxSteerAngle = 30;
        maxEngineTorque = 1000;
        maxSpeed = 100;
    }

    // Update is called once per frame
    void Update()
    {
        WheelsUpdate();
        GForce();
        Gears();
        speedText.text = Mathf.Round(currentSpeed).ToString() + "\n Km/h";
    }

    void FixedUpdate()
    {
        Steer();
        Engine();
        DownForce();
    }

    void Steer()
    {
        actualSteerAngle = Input.GetAxis("Horizontal") * maxSteerAngle;
        steeringWheel.transform.localEulerAngles = new Vector3(12,0,90 - (270 * Input.GetAxis("Horizontal")));
        wheelsPhysics[0].steerAngle = actualSteerAngle;
        wheelsPhysics[1].steerAngle = actualSteerAngle;
    }

    void Engine()
    {
        actualEngineTorque = Input.GetAxis("Vertical") * maxEngineTorque;
        currentSpeed = carRb.velocity.magnitude * 3.6f;
        wheelsPhysics[2].motorTorque = actualEngineTorque;
        wheelsPhysics[3].motorTorque = actualEngineTorque;
       
        engineSound.pitch = Mathf.Lerp(1.5f,4,currentSpeed/gearMaxSpeed);
    }

    void Gears()
    {
        if(currentSpeed > gearMaxSpeed && gear < 3)
        {
            gear++;
        }

        if(currentSpeed < gearMinSpeed && gear > 0)
        {
            gear--;
        }


        switch (gear)
        {
            case 0:
                gearMaxSpeed = 30;
                
            break;
            case 1:
                gearMaxSpeed = 60;
                gearMinSpeed = 25;
            break;
            case 2:
                gearMaxSpeed = 90;
                gearMinSpeed = 55;
            break;
            case 3:
                gearMaxSpeed = 120;
                gearMinSpeed = 85;
            break;
        }
    }

    void GForce()
    {
        GForceValue = (currentSpeed-lastFrameSpeed) / (Time.deltaTime * Physics.gravity.magnitude);
        lastFrameSpeed = currentSpeed;
    }

    void DownForce()
    {
        wheelsPhysics[0].attachedRigidbody.AddForce(Vector3.down * currentSpeed * 20);
        wheelsPhysics[1].attachedRigidbody.AddForce(Vector3.down * currentSpeed * 20);
        wheelsPhysics[2].attachedRigidbody.AddForce(Vector3.down * currentSpeed * 20);
        wheelsPhysics[3].attachedRigidbody.AddForce(Vector3.down * currentSpeed * 20);
    }

    void WheelsUpdate()
    {
        for (int i = 0; i < 4; i++)
		{
			Quaternion quat;
			Vector3 pos;
			wheelsPhysics[i].GetWorldPose(out pos, out quat);

			wheelsMesh[i].position = pos;
			wheelsMesh[i].rotation = quat;
		}
    }
}
