using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyCheater : MonoBehaviour
{
    // location of the player's ship. for now, set in editor
    [SerializeField] private Transform targetShipTransform;
    // location of self. set in editor
    [SerializeField] private Transform selfTransform;
    // Calculated on fixed update
    [SerializeField] private Vector3 selfToTarget;
    // Calculated on fixed update
    [SerializeField] private Vector3 steeringVector;
    // Calculated on fixed update
    [SerializeField] private bool wantToShoot;
    // set in editor
    [SerializeField] private float angleToShoot;
    [SerializeField] private float maneuverAgressiveness;
    private int randomManuver = 0;
    private int ticksToHoldRandomManuver = 1000; // 1000/50=20 seconds
    private int tickCount = 0;
    private bool hasShotThisCycle;
    [SerializeField] private bool waiting = false;
    [SerializeField] private float secondsDelayBeforeStoppingWaiting = 8f;
    private Rigidbody rb;
    private Laser laser;
    
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<Laser>();
        rb = GetComponent<Rigidbody>();
        wantToShoot = false;
        hasShotThisCycle = false;
        tickCount = 0;
        randomManuver = 0;
        if (waiting)
            Invoke("StopWaiting", secondsDelayBeforeStoppingWaiting);
    }

    void StopWaiting()
    {
        waiting = false;
    }

    void EvaluateTargetLocation()
    {
        // https://upload.wikimedia.org/wikipedia/commons/thumb/5/5b/Vector_subtraction.png/640px-Vector_subtraction.png
        selfToTarget = Vector3.Normalize((targetShipTransform.position - selfTransform.position));
        // we need to compare our forward vector to the vector pointing to the target to get the steering vector
        steeringVector = Vector3.Normalize(selfToTarget - selfTransform.forward);
        // we end up only using the x and y components to tell how to rotate, and this means that a high Z component results
        // in weaker steering. so, lets eliminate distance from the target as a factor of steering
        // float saveZ = steeringVector.z;
        // steeringVector.z = 0.0f;
        steeringVector = Vector3.Normalize(steeringVector);
    }
    
    void ManuverShip()
    {
        rb.AddForce(selfToTarget * maneuverAgressiveness);
        rb.AddTorque(new Vector3(steeringVector.y, -steeringVector.x, 0.0f) * maneuverAgressiveness, ForceMode.Acceleration);

        if (randomManuver == 0)
        {
            rb.AddRelativeForce(Vector3.up);
        }
        else if (randomManuver == 1)
        {
            rb.AddRelativeForce(Vector3.down);
        }
        else if (randomManuver == 2)
        {
            rb.AddRelativeForce(Vector3.left);
        }
        else if (randomManuver == 3)
        {
            rb.AddRelativeForce(Vector3.right);
        }
        else
        {
            Debug.Log("Impossible random manuver in ManuverShip() in EnemyCheater.cs");
        }

        tickCount++;
        if (tickCount >= ticksToHoldRandomManuver)
        {
            randomManuver = Random.Range(0, 4);
            DampenMovement();
            if (!hasShotThisCycle)
            {
                rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f); // now here's the real cheat, lol
                angleToShoot = 1;
                Debug.Log("CHEATER E BRAKE");
            }
            tickCount = 0;
            hasShotThisCycle = false;
        }

        // dampen movement every 0.4 seconds. Dampen movement again if close to target to avoid overshooting
        // if (tickCount % 20 == 0)
        //     DampenMovement();
        if (Vector3.Angle(selfToTarget, laser.GetMuzzleDirection()) < 90)
            DampenMovement();

        // stop rolling like an idiot, only spin a little bit, lol
        // if (rb.angularVelocity.z > 0.55f)
        //     rb.angularVelocity = new Vector3(rb.angularVelocity.x, rb.angularVelocity.y, 0.55f);
        // if (rb.angularVelocity.z < -0.55f)
        //     rb.angularVelocity = new Vector3(rb.angularVelocity.x, rb.angularVelocity.y, -0.55f);
    }
    
    void DampenMovement()
    {
        rb.AddTorque(-rb.angularVelocity * maneuverAgressiveness * 0.5f, ForceMode.Acceleration);
    }

    void FireControl()
    {
        wantToShoot = (Vector3.Angle(selfToTarget, laser.GetMuzzleDirection()) < angleToShoot) ? true : false;
        if (wantToShoot)
        {
            laser.Shoot();
            angleToShoot = Random.Range(3, 15);
            hasShotThisCycle = true;
        }
    }
    
    // 50 calls per second
    void FixedUpdate()
    {
        if (waiting || targetShipTransform == null)
            return;
        EvaluateTargetLocation();
        ManuverShip();
        FireControl();
    }
}
