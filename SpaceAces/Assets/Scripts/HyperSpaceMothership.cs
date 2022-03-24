using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperSpaceMothership : MonoBehaviour
{
    public Transform hyperSpaceFinalPosition;
    public Transform slowFinalPosition;
    public bool jumpedFromHyperSpace = false;
    public float hyperSpaceSpeed = 100f;
    public float slowSpeed = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        // Delay system. Thinking of adding 5 seconds or so before it jumps from hyperspace.
    }

    // FixedUpdate is based on frame rate. Hence, this function focuses on the ship's movement
    // to give a realistic effect.
    void FixedUpdate()
    {
        Vector3 original = transform.position;
        Vector3 destination = hyperSpaceFinalPosition.position;

        // Ship has already reached initial destination from hyper space, switch to slow speed.
        if (jumpedFromHyperSpace || CompareVector3(original, destination))
        {
            destination = slowFinalPosition.position;
            // Move the ship from it's original location to a new one at a particular speed.
            transform.position = Vector3.MoveTowards(original, destination, slowSpeed);
        }

        // Ship hasn't reached initial destination, keep it's speed at hyperspace speed.
        else
        {
            transform.position = Vector3.MoveTowards(original, destination, hyperSpaceSpeed);
        }
    }

    // Function is equivalent to 'if (a == b)' for Vector3 but accounts for float errors.
    public bool CompareVector3(Vector3 a, Vector3 b)
    {
        bool retval = Vector3.SqrMagnitude(a - b) < 0.0001;
        
        // Flag we reached the initial destination from hyper space. This will toggle slow speeds.
        if (retval == true)
            jumpedFromHyperSpace = true;
        
        return retval;
    }
}
