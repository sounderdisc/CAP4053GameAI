using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Chaser! https://www.youtube.com/watch?v=TzGfjQSz6ow&ab_channel=CieloBernal
public class EnemyChaser : MonoBehaviour
{
    // location of the player's ship. for now, set in editor
    [SerializeField] private Transform playerShipTransform;
    [SerializeField] private Transform selfTransform;
    [SerializeField] private Vector3 selfToPlayer;
    [SerializeField] private Vector3 targetManeuver;
    [SerializeField] private bool wantToShoot;
    [SerializeField] private float maneuverAgressiveness;
    private Rigidbody rb;
    private FlightControl flightController;

    // These are all instance variables so we can set them in editor. Passed to FlightControl
    [SerializeField] float baseThrust;
    [SerializeField] float baseRotation;
    [SerializeField] float flightAssistStrength;
    [SerializeField] float maxSpeed, idealSpeed;
    [SerializeField] float rollMod, pitchMod, yawMod, surgeMod, swayMod, heaveMod;
    private float surgeInput, swayInput, heaveInput;
    private float rollInput, pitchInput, yawInput;
    private bool toggleFA;
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
        selfToPlayer = Vector3.Normalize((playerShipTransform.position - selfTransform.position));
        targetManeuver = (selfTransform.forward - selfToPlayer) * maneuverAgressiveness;
        float manageableSpeed = Vector3.Angle(selfToPlayer, selfTransform.forward) / 10;
        // Debug.Log(targetManeuver);

        // set toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, and yawInput
        if (rb.angularVelocity.magnitude > manageableSpeed)
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
            pitchInput = -targetManeuver[1]; 
            yawInput = targetManeuver[0];
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


        wantToShoot = (Vector3.Angle(selfToPlayer, laser.GetMuzzleDirection()) < 5) ? true : false;
        if (wantToShoot)
        {
            // Debug.Log("selfTo Player: " + selfToPlayer + " | muzzle: " + laser.GetMuzzleDirection() + " | angle: " + Vector3.Angle(selfToPlayer, laser.GetMuzzleDirection()));
            // laser.Shoot();
            wantToShoot = true; // useless, except i think the compiler hates empty if's and i want to keep this if empty for now.
        }
    }
    
    void FixedUpdate()
    {
        DecideControllerState();
        flightController.ReceiveInput(toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, yawInput);
    }
}
