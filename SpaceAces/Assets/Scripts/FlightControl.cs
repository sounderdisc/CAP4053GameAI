using System.Collections;
using System.Collections.Generic;
using System; // Math is in here. I use Math.abs()
using UnityEngine;

public class FlightControl : MonoBehaviour
{
    private float FLOAT_ZERO_TOLERANCE = 0.0001f;
    private float THROTLE_MOVE_SPEED = 25; // modifer to make the throttle move slower so a pilot can adjust speed accurately with FA on.
    private float baseThrust;
    private float baseRotation;
    private float flightAssistStrength;
    private float idealSpeedBonus;
    private float desiredSpeed, maxSpeed, idealSpeed;
    private float rollMod, pitchMod, yawMod, surgeMod, swayMod, heaveMod;
    private float surgeInput, swayInput, heaveInput;
    private float rollInput, pitchInput, yawInput;
    private Rigidbody rb;
    [SerializeField] private bool isActiveFA = false; // like auto break in nav meshes mentioned in class 

    void Start()
    {
        // Fetch the Rigidbody from the GameObject with this script attached
        rb = GetComponent<Rigidbody>(); // https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
        desiredSpeed = 0;
        idealSpeedBonus = 1;

    }

    // constructor
    public void FakeConstructor(float roll, float pitch, float yaw, float surge, float sway, float heave, float thrust, float rotation, float assist, float max, float ideal)
    {
        rollMod = roll;
        pitchMod = pitch;
        yawMod = yaw;

        surgeMod = surge;
        swayMod = sway;
        heaveMod = heave;

        baseThrust = thrust;
        baseRotation = rotation;
        flightAssistStrength = assist;
        maxSpeed = max;
        idealSpeed = ideal;

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
                desiredSpeed = Math.Min(rb.velocity.z, maxSpeed);
            }
            else
            {
                isActiveFA = false;
                desiredSpeed = 0;
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
        rb.AddRelativeForce(Vector3.right * swayMod * swayInput * baseThrust * idealSpeedBonus);
        rb.AddRelativeForce(Vector3.up * heaveMod * heaveInput * baseThrust * idealSpeedBonus);
        
        // ROTATIONAL AXES
        rb.AddRelativeTorque(Vector3.back * rollMod * rollInput * baseRotation * idealSpeedBonus);
        rb.AddRelativeTorque(Vector3.left * pitchMod * pitchInput * baseRotation * idealSpeedBonus);
        rb.AddRelativeTorque(Vector3.up * yawMod * yawInput * baseRotation * idealSpeedBonus);

        // FLIGHT ASSIST
        if (isActiveFA)
        {
            // stop spinning, unless the player is putting in input to that axis
            Vector3 currentAngularVelocity = rb.angularVelocity; 
            if ((Math.Abs(rollInput) < FLOAT_ZERO_TOLERANCE) && (Math.Abs(currentAngularVelocity.z) > FLOAT_ZERO_TOLERANCE))
            {
                rb.AddTorque(new Vector3(0, 0, currentAngularVelocity.z) * rollMod * baseRotation * -flightAssistStrength);
            }
            if ((Math.Abs(pitchInput) < FLOAT_ZERO_TOLERANCE) && (Math.Abs(currentAngularVelocity.x) > FLOAT_ZERO_TOLERANCE))
            {
                rb.AddTorque(new Vector3(currentAngularVelocity.x, 0, 0) * pitchMod * baseRotation * -flightAssistStrength);
            }
            if ((Math.Abs(yawInput) < FLOAT_ZERO_TOLERANCE) && (Math.Abs(currentAngularVelocity.y) > FLOAT_ZERO_TOLERANCE))
            {
                rb.AddTorque(new Vector3(0, currentAngularVelocity.y, 0) * yawMod * baseRotation * -flightAssistStrength);
            }
            
            // arrest the straifing and bobbing
            Vector3 currentVelocity = rb.velocity;
            if ((Math.Abs(heaveInput) < FLOAT_ZERO_TOLERANCE) && (Math.Abs(currentVelocity.y) > FLOAT_ZERO_TOLERANCE))
            {
                rb.AddForce(new Vector3(0, currentVelocity.y, 0) * heaveMod * baseThrust * -flightAssistStrength);
            }
            if ((Math.Abs(swayInput) < FLOAT_ZERO_TOLERANCE) && (Math.Abs(currentVelocity.x) > FLOAT_ZERO_TOLERANCE))
            {
                rb.AddForce(new Vector3(currentVelocity.x, 0, 0) * swayMod * baseThrust * -flightAssistStrength);
            }

            // forward and backwards, the z axis, is done differently when flight assist is on vs off.
            // first move the throttle according to the pilot's desires
            desiredSpeed += surgeInput / THROTLE_MOVE_SPEED;
            if (desiredSpeed > maxSpeed)
            {
                desiredSpeed = maxSpeed;
            }
            else if (desiredSpeed < -maxSpeed)
            {
                desiredSpeed = -maxSpeed;
            }
            // now, if the ship isnt going at the speed that the pilot wants, then apply force
            if (Math.Abs(desiredSpeed - currentVelocity.z) > FLOAT_ZERO_TOLERANCE)
            {
                float throttleDirection = (desiredSpeed-currentVelocity.z > 0) ? 1 : -1;
                rb.AddRelativeForce(Vector3.forward * surgeMod * throttleDirection * baseThrust * idealSpeedBonus);
            }

            // Debug.Log("FA ON: " + rb.velocity + " idealSpeedBonus: " + idealSpeedBonus);
        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * surgeMod * surgeInput * baseThrust * idealSpeedBonus);
            // Debug.Log("FA OFF: " + rb.velocity + " idealSpeedBonus: " + idealSpeedBonus);
        }

        // IDEAL SPEED MANUVERABLITY BONUS
        // max 2 min 0.8 log of the difference between current speed and idea speed
        idealSpeedBonus = (float) Math.Max( 0.8, Math.Min(2.0f, Math.Abs(Math.Log10(Math.Abs(rb.velocity.magnitude - idealSpeed)))));

        // MAX SPEED LIMIT
        // act like rubber band to pull back
        if (rb.velocity.magnitude > maxSpeed)
        {
            float rubberBandStrength = (rb.velocity.magnitude - maxSpeed) / 2;
            rb.AddForce(-rb.velocity * rubberBandStrength);
        }

        // Debug.Log(rb.angularVelocity);
    }
}
