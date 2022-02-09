using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePlayerInput : MonoBehaviour
{
    public FlightControlV2 flightController;

    // These are all instance variables so we can set them in editor. Pls dont delete even though they arent used anywhere
    public float baseThrust;
    public float baseRotation;
    public float rollMod, pitchMod, yawMod, surgeMod, swayMod, heaveMod;
    
    // Start is called before the first frame update
    void Start()
    {
        // Fetch the FlightControllerV2 from the GameObject with this script attached
        flightController = new FlightControlV2(rollMod, pitchMod, yawMod, surgeMod, swayMod, heaveMod, baseThrust, baseRotation);
    }

    // Update is called once per frame
    void Update()
    {
        // TRANSLATIONAL AXES
        float surgeInput = Input.GetAxis("SurgeAxis");
        float swayInput = Input.GetAxis("SwayAxis");
        float heaveInput = Input.GetAxis("HeaveAxis");

        // ROTATIONAL AXES
        float rollInput = Input.GetAxis("RollAxis");
        float pitchInput = Input.GetAxis("PitchAxis");
        float yawInput = Input.GetAxis("YawAxis");

        // flight stabilization assist toggle
        bool toggleFA = false;
        if (Input.GetKeyDown("left shift"))
        {
            toggleFA = true;
        }

        flightController.ReceiveInput(toggleFA, surgeInput, swayInput, heaveInput, rollInput, pitchInput, yawInput);
    }
}
