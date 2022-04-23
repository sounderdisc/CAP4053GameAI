using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Chaser! https://www.youtube.com/watch?v=TzGfjQSz6ow&ab_channel=CieloBernal
public class EnemyChaser : MonoBehaviour
{
    // location of the player's ship. for now, set in editor
    [SerializeField] private Transform targetShipTransform;
    [SerializeField] private Transform selfTransform;
    [SerializeField] private Vector3 selfToTarget;
    [SerializeField] private Vector3 steeringVector;
    [SerializeField] private bool wantToShoot;
    [SerializeField] private bool ignoreDamping;
    [SerializeField] private bool haltDebug;
    [SerializeField] private float angleToShoot;
    [SerializeField] private float targetLostAngle;
    [SerializeField] private float maneuverAgressiveness;
    [SerializeField] private float inputDamping;
    private Rigidbody rb;
    private FlightControl flightController;

    // These are all instance variables so we can set them in editor. Passed to FlightControl
    [SerializeField] float baseThrust;
    [SerializeField] float baseRotation;
    [SerializeField] float flightAssistStrength;
    [SerializeField] float maxSpeed, idealSpeed;
    [SerializeField] float rollMod, pitchMod, yawMod, surgeMod, swayMod, heaveMod;
    [SerializeField] private float surgeInput, swayInput, heaveInput;
    [SerializeField] private float rollInput, pitchInput, yawInput;
    [SerializeField] private bool toggleFA;
    private Laser laser;
    
    // Start is called before the first frame update
    void Start()
    {
        // PSA using a normal constructor for a monobehavior is a no-no. use add component instead
        flightController = gameObject.AddComponent<FlightControl>() as FlightControl;
        flightController.FakeConstructor(rollMod, pitchMod, yawMod, surgeMod, swayMod, heaveMod, baseThrust, baseRotation, flightAssistStrength, maxSpeed, idealSpeed);
        laser = GetComponent<Laser>();
        rb = GetComponent<Rigidbody>();
        wantToShoot = false;
        toggleFA = false; surgeInput = 0.0f; swayInput = 0.0f; heaveInput = 0.0f; rollInput = 0.0f; pitchInput = 0.0f; yawInput = 0.0f;
    }

    void DecideControllerState()
    {
        // https://upload.wikimedia.org/wikipedia/commons/thumb/5/5b/Vector_subtraction.png/640px-Vector_subtraction.png
        selfToTarget = Vector3.Normalize((targetShipTransform.position - selfTransform.position));
        // we need to compare our forward vector to the vector pointing to the target to get the steering vector
        steeringVector = Vector3.Normalize(selfToTarget - selfTransform.forward);
        // we end up only using the x and y components to tell how to rotate, and this means that a high Z component results
        // in weaker steering. so, lets eliminate distance from the target as a factor of steering
        float saveZ = steeringVector.z;
        steeringVector.z = 0.0f;
        steeringVector = Vector3.Normalize(steeringVector);
        // damping will help with our overshooting problem. you know how it sorta vibrates violently when locked on target?
        Vector3 normalizedAngularVelocity = rb.angularVelocity;
        normalizedAngularVelocity.z = 0.0f;
        normalizedAngularVelocity = Vector3.Normalize(normalizedAngularVelocity);
        inputDamping = 1.5f - Mathf.InverseLerp(-1, 1, Vector3.Dot(normalizedAngularVelocity, steeringVector));
        if (ignoreDamping)
            inputDamping = 1.0f;

        // set toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, and yawInput
        toggleFA = false;
        surgeInput = -selfToTarget.z;
        swayInput  = -selfToTarget.x;
        heaveInput = selfToTarget.y;
        rollInput = 0.0f; 
        pitchInput = Mathf.Clamp(steeringVector.x * maneuverAgressiveness * inputDamping, -1, 1); 
        yawInput = Mathf.Clamp(steeringVector.y * maneuverAgressiveness * inputDamping, -1, 1);

        // manage throttle
        // stop if you arent facing the target. move forward if you are
        // if (Vector3.Angle(selfToTarget, selfTransform.forward) > targetLostAngle)
        // {
        //     if (flightController.getCurrentSpeed() > 0)
        //     {
        //         surgeInput = 1.0f;
        //     }
        //     else
        //     {
        //         surgeInput = -1.0f;
        //     }
        // }
        // else
        // {
        //     if (flightController.getDesiredSpeed() < idealSpeed)
        //     {
        //         surgeInput = 1.0f;
        //     }
        //     else
        //     {
        //         surgeInput = -1.0f;
        //     }
        // }

        wantToShoot = (Vector3.Angle(selfToTarget, laser.GetMuzzleDirection()) < angleToShoot) ? true : false;
        if (wantToShoot)
        {
            laser.Shoot();
        }

        if (haltDebug)
        {
            surgeInput = 0.0f;
            swayInput = 0.0f;
            heaveInput = 0.0f;
            toggleFA = true;
        }
    }
    
    void FixedUpdate()
    {
        DecideControllerState();
        flightController.ReceiveInput(toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, yawInput);
    }
}
