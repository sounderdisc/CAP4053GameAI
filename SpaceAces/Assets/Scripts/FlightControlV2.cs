using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightControlV2 : MonoBehaviour
{
    public float baseThrust;
    public float baseRotation;
    public float rollMod, pitchMod, yawMod, surgeMod, swayMod, heaveMod;
    public float surgeInput, heaveInput, swayInput;
    public float rollInput, pitchInput, yawInput;
    public Rigidbody rb;
    public bool isActiveFA; // like auto break in nav meshes mentioned in class

    void Start()
    {
        // Fetch the Rigidbody from the GameObject with this script attached
        rb = GetComponent<Rigidbody>(); // https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
    }

    // constructor
    public FlightControlV2(float roll, float pitch, float yaw, float surge, float sway, float heave, float thrust, float rotation)
    {
        rollMod = roll;
        pitchMod = pitch;
        yawMod = yaw;

        surgeMod = surge;
        swayMod = sway;
        heaveMod = heave;

        baseThrust = thrust;
        baseRotation = rotation;
    }

    public void ReceiveInput(bool toggleFA, float surge, float sway, float heave, float roll, float pitch, float yaw) //here's where you're gonna update thrust with the stick's position, or the AI pilot's commands
    {
    	// flight stabilization assist toggle
        if (toggleFA)
        {
            if (!isActiveFA)
            {
                isActiveFA = true;
                rb.drag = baseThrust / 5;
                rb.angularDrag = baseRotation * 1.5f;
            }
            else
            {
                isActiveFA = false;
                rb.drag = 0;
                rb.angularDrag = 0;
            }
        }

        // TRANSLATIONAL AXES
        surgeInput = surge;
        swayInput = sway;
        heaveInput = heave;

        // ROTATIONAL AXES
        rollInput = roll;
        pitchInput = pitch;
        yawInput = yaw;
    }

    void FixedUpdate()
    {
        // TRANSLATIONAL AXES
        rb.AddRelativeForce(Vector3.forward * surgeMod * surgeInput * baseThrust);
        rb.AddRelativeForce(Vector3.right * swayMod * swayInput * baseThrust);
        rb.AddRelativeForce(Vector3.up * heaveMod * heaveInput * baseThrust);
        
        //ROTATIONAL AXES
        rb.AddRelativeTorque(Vector3.back * rollMod * rollInput * baseRotation);
        rb.AddRelativeTorque(Vector3.left * pitchMod * pitchInput * baseRotation);
        rb.AddRelativeTorque(Vector3.up * yawMod * yawInput * baseRotation);
    }
}
