using System.Collections;
using System.Collections.Generic;
using System; // Math is in here. I use Math.abs()
using UnityEngine;

public class FlightControl : MonoBehaviour
{
    private float floatZeroTolerance = 0.000001f;
    private float baseThrust;
    private float baseRotation;
    private float rollMod, pitchMod, yawMod, surgeMod, swayMod, heaveMod;
    private float surgeInput, swayInput, heaveInput;
    private float rollInput, pitchInput, yawInput;
    private Rigidbody rb;
    private bool isActiveFA; // like auto break in nav meshes mentioned in class 

    void Start()
    {
        // Fetch the Rigidbody from the GameObject with this script attached
        rb = GetComponent<Rigidbody>(); // https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
    }

    // constructor
    public void FakeConstructor(float roll, float pitch, float yaw, float surge, float sway, float heave, float thrust, float rotation)
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

    //here's where you're gonna update thrust with the stick's position, or the AI pilot's commands
    public void ReceiveInput(bool toggleFA, float surge, float sway, float heave, float roll, float pitch, float yaw)
    {
        // flight assist toggle
        if (toggleFA != isActiveFA)
        {
            if (!isActiveFA)
            {
                isActiveFA = true;
                // rb.drag = baseThrust / 5;
                // rb.angularDrag = baseRotation * 1.5f;
            }
            else
            {
                isActiveFA = false;
                // rb.drag = 0;
                // rb.angularDrag = 0;
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
        
        // ROTATIONAL AXES
        rb.AddRelativeTorque(Vector3.back * rollMod * rollInput * baseRotation);
        rb.AddRelativeTorque(Vector3.left * pitchMod * pitchInput * baseRotation);
        rb.AddRelativeTorque(Vector3.up * yawMod * yawInput * baseRotation);

        // FLIGHT ASSIST
        if (isActiveFA)
        {
            Vector3 currentAngularVelocity = rb.angularVelocity; 
            if ((Math.Abs(rollInput) < floatZeroTolerance) && (Math.Abs(currentAngularVelocity.z) > floatZeroTolerance))
            {
                float rollSign = currentAngularVelocity.z / Math.Abs(currentAngularVelocity.z);
                rb.AddRelativeTorque(Vector3.back * rollMod * baseRotation * rollSign);
            }
            if ((Math.Abs(pitchInput) < floatZeroTolerance) && (Math.Abs(currentAngularVelocity.x) > floatZeroTolerance))
            {
                float pitchSign = currentAngularVelocity.x / Math.Abs(currentAngularVelocity.x);
                rb.AddRelativeTorque(Vector3.left * pitchMod * baseRotation * pitchSign);
            }
            if ((Math.Abs(yawInput) < floatZeroTolerance) && (Math.Abs(currentAngularVelocity.y) > floatZeroTolerance))
            {
                float yawSign = -1 * currentAngularVelocity.y / Math.Abs(currentAngularVelocity.y);
                rb.AddRelativeTorque(Vector3.up * yawMod * baseRotation * yawSign);
            }
            print(currentAngularVelocity);
        }
        else
        {
            print("FA OFF" + rb.angularVelocity);
        }
    }
}
