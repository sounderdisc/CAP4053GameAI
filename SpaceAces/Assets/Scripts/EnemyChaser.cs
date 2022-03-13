using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Chaser! https://www.youtube.com/watch?v=TzGfjQSz6ow&ab_channel=CieloBernal
public class EnemyChaser : MonoBehaviour
{
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
        flightController.ReceiveInput(true, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    }

    void DecideControllerState()
    {
        // set toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, and yawInput
        toggleFA = true;
        surgeInput = 1.0f;
        swayInput  = 0.0f;
        heaveInput = 0.0f;
        rollInput = 0.0f;
        pitchInput = 0.05f;
        yawInput = 0.0f;

        bool wantToShoot = false;
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
