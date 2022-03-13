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
    [SerializeField] private bool wantToShoot;
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
        rb = GetComponent<Rigidbody>(); // Unity is being a bitch and not acually fucking getting the damn object
        flightController.ReceiveInput(true, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
        wantToShoot = false;
    }

    void DecideControllerState()
    {
        selfToPlayer = Vector3.Normalize(playerShipTransform.position - selfTransform.position);

        // Debug.Log("laser null? " + (laser == null));
        // Debug.Log("rb null? " + (rb == null));
        // set toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, and yawInput
        if (rb.angularVelocity.magnitude > 0.2f)
        {
            Debug.Log("in if");
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
            pitchInput = 0.0f; // selfToPlayer[1];
            yawInput = (selfToPlayer[0] > 0) ? 1.0f : -1.0f; // selfToPlayer[0];

            Debug.Log("in else" + yawInput);
        }

        wantToShoot = (Vector3.Angle(selfToPlayer, laser.GetMuzzleDirection()) < 5) ? true : false;
        if (wantToShoot)
        {
            laser.Shoot();
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        DecideControllerState();
        flightController.ReceiveInput(toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, yawInput);
    }
}
