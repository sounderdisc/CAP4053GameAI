using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightControl : MonoBehaviour
{
    // instance variables can be changed in inspector
    Rigidbody m_Rigidbody;
    public float m_Thrust; // recomended value in inspector: 20
    public float m_Torque; // recomended value in inspector: 5
    
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>(); // https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
    }

    // Update is called once per frame
    void Update()
    {
        // empty, for now
    }
    void FixedUpdate()
    {
        //------ translation axes ----------
        // surge, forwards and back
        if (Input.GetKey(KeyCode.W))
        {
            print("W key was pressed");
            m_Rigidbody.AddForce(transform.forward * m_Thrust);
        }

        if (Input.GetKey(KeyCode.S))
        {
            print("S key was pressed");
            m_Rigidbody.AddForce((-1*transform.forward) * m_Thrust);
        }
        
        // sway, left and right
        if (Input.GetKey(KeyCode.A))
        {
            print("A key was pressed");
            m_Rigidbody.AddForce((-1*transform.right) * m_Thrust);
        }

        if (Input.GetKey(KeyCode.D))
        {
            print("D key was pressed");
            m_Rigidbody.AddForce(transform.right * m_Thrust);
        }

        // Heave, up and down
        if (Input.GetKey(KeyCode.Q))
        {
            print("Q key was pressed");
            m_Rigidbody.AddForce(transform.up * m_Thrust);
        }

        if (Input.GetKey(KeyCode.E))
        {
            print("E key was pressed");
            m_Rigidbody.AddForce((-1*transform.up) * m_Thrust);
        }

        //------ rotational axes ----------
        
        // roll, side to side
        if (Input.GetKey(KeyCode.J))
        {
            print("J key was pressed");
            m_Rigidbody.AddTorque(transform.forward * m_Torque);
        }

        if (Input.GetKey(KeyCode.L))
        {
            print("L key was pressed");
            m_Rigidbody.AddTorque((-1*transform.forward) * m_Torque);
        }

        // pitch, up and down
        if (Input.GetKey(KeyCode.I))
        {
            print("I key was pressed");
            m_Rigidbody.AddTorque(transform.right * m_Torque);
        }

        if (Input.GetKey(KeyCode.K))
        {
            print("K key was pressed");
            m_Rigidbody.AddTorque((-1*transform.right) * m_Torque);
        }

        // yaw, left and right
        if (Input.GetKey(KeyCode.U))
        {
            print("U key was pressed");
            m_Rigidbody.AddTorque((-1*transform.up) * m_Torque);
        }

        if (Input.GetKey(KeyCode.O))
        {
            print("O key was pressed");
            m_Rigidbody.AddTorque(transform.up * m_Torque);
        }
    }
}
