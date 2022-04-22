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
    [SerializeField] private float angleToShoot;
    [SerializeField] private float targetLostAngle;
    [SerializeField] private float maneuverAgressiveness;
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
    }

    void DecideControllerState()
    {
        // https://upload.wikimedia.org/wikipedia/commons/thumb/5/5b/Vector_subtraction.png/640px-Vector_subtraction.png
        selfToTarget = Vector3.Normalize((targetShipTransform.position - selfTransform.position));
        // we need to compare our forward vector to the vector pointing to the target to get the steering vector
        steeringVector = Vector3.Normalize(selfToTarget - selfTransform.forward);

        // set toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, and yawInput
        // if (steeringVector.z < 0) // the target is behind you
        if (false) // jank way to comment out one part of an if statement. very unclean code, but it works
        {
            toggleFA = true;
            surgeInput = 0.0f;
            swayInput  = 0.0f;
            heaveInput = 0.0f;
            rollInput = 0.0f; 
            pitchInput = 1.0f; 
            yawInput = Mathf.Clamp(steeringVector.y * maneuverAgressiveness, -1, 1);;
        }
        else // the target is in front of you
        {
            toggleFA = true;
            surgeInput = 0.0f;
            swayInput  = 0.0f;
            heaveInput = 0.0f;
            rollInput = 0.0f; 
            pitchInput = Mathf.Clamp(steeringVector.x * maneuverAgressiveness, -1, 1); 
            yawInput = Mathf.Clamp(steeringVector.y * maneuverAgressiveness, -1, 1);
        }

        // manage throttle
        // stop if you arent facing the target. move forward if you are
        if (Vector3.Angle(selfToTarget, selfTransform.forward) > targetLostAngle)
        {
            if (flightController.getCurrentSpeed() > 0)
            {
                surgeInput = 1.0f;
            }
            else
            {
                surgeInput = -1.0f;
            }
        }
        else
        {
            if (flightController.getDesiredSpeed() < idealSpeed)
            {
                surgeInput = 1.0f;
            }
            else
            {
                surgeInput = -1.0f;
            }

            // surgeInput = 1.0f;
        }

        wantToShoot = (Vector3.Angle(selfToTarget, laser.GetMuzzleDirection()) < angleToShoot) ? true : false;
        if (wantToShoot)
        {
            laser.Shoot();
        }

    }
    
    void FixedUpdate()
    {
        DecideControllerState();
        flightController.ReceiveInput(toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, yawInput);
    }
}
