using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Chaser! https://www.youtube.com/watch?v=TzGfjQSz6ow&ab_channel=CieloBernal
public class EnemyChaser : MonoBehaviour
{
    // location of the player's ship. for now, set in editor
    [SerializeField] private Transform playerShipTransform;
    [SerializeField] private Transform selfTransform;
    [SerializeField] private Vector3 steeringVector;
    [SerializeField] private Vector3 targetManeuver;
    [SerializeField] private bool wantToShoot;
    [SerializeField] private float angleToShoot;
    [SerializeField] private float handsOffAngle;
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
        steeringVector = Vector3.Normalize((playerShipTransform.position - selfTransform.position));

        // set toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, and yawInput
        if (Vector3.Angle(steeringVector, selfTransform.forward) < handsOffAngle)
        {
            toggleFA = true;
            surgeInput = 0.0f;
            swayInput  = 0.0f;
            heaveInput = 0.0f;
            rollInput = 0.0f; 
            pitchInput = 0.0f; 
            yawInput = 0.0f;
        }
        else
        {
            toggleFA = true;
            surgeInput = 0.0f;
            swayInput  = 0.0f;
            heaveInput = 0.0f;
            rollInput = 0.0f; 
            pitchInput = Mathf.Clamp(-steeringVector[1] * maneuverAgressiveness, -1, 1); 
            yawInput = Mathf.Clamp(steeringVector[0] * maneuverAgressiveness, -1, 1);
        }

        // manage throttle
        if (flightController.getDesiredSpeed() < idealSpeed)
        {
            surgeInput = 1.0f;
        }
        else
        {
            surgeInput = -1.0f;
        }


        wantToShoot = (Vector3.Angle(steeringVector, laser.GetMuzzleDirection()) < angleToShoot) ? true : false;
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
