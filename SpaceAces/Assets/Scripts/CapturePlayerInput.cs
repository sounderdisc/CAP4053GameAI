using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CapturePlayerInput : MonoBehaviour
{
    [SerializeField] FlightControl flightController;
    [SerializeField] PlayerControls controls;

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
    }

    void Awake()
    {
        controls = new PlayerControls();

        // toggle flight assist
        controls.ShipMovement.ToggleFA.performed += ctx => toggleFA = !toggleFA;

        // TRANSLATIONAL AXES
        controls.ShipMovement.SurgeAxis.performed += ctx => surgeInput = ctx.ReadValue<float>();
        controls.ShipMovement.SwayAxis.performed += ctx => swayInput = ctx.ReadValue<float>();
        controls.ShipMovement.HeaveAxis.performed += ctx => heaveInput = ctx.ReadValue<float>();

        controls.ShipMovement.SurgeAxis.canceled += ctx => surgeInput = 0f;
        controls.ShipMovement.SwayAxis.canceled += ctx => swayInput = 0f;
        controls.ShipMovement.HeaveAxis.canceled += ctx => heaveInput = 0f;

        // ROTATIONAL AXES
        controls.ShipMovement.RollAxis.performed += ctx => rollInput = ctx.ReadValue<float>();
        controls.ShipMovement.PitchAxis.performed += ctx => pitchInput = ctx.ReadValue<float>();
        controls.ShipMovement.YawAxis.performed += ctx => yawInput = ctx.ReadValue<float>();

        controls.ShipMovement.RollAxis.canceled += ctx => rollInput = 0f;
        controls.ShipMovement.PitchAxis.canceled += ctx => pitchInput = 0f;
        controls.ShipMovement.YawAxis.canceled += ctx => yawInput = 0f;

        // laser weapon
        controls.Weapons.Fire1.started += ctx => laser.Shoot();
    }

    void OnEnable()
    {
        controls.ShipMovement.Enable();
        controls.Weapons.Enable();
    }

    void OnDisable()
    {
        controls.ShipMovement.Disable();
        controls.Weapons.Disable();
    }
    
    // Update is called once per frame
    void Update()
    {
        flightController.ReceiveInput(toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, yawInput);
    }
}
