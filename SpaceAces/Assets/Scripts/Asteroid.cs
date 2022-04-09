using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    Transform trans;
    Vector3 rotation;

    [SerializeField]float scaling_min = 10.0f;
    [SerializeField]float scaling_max = 20.0f;
    [SerializeField]float rotationSpeed = 10f;

    // Initialize variables before game starts.
    void Awake()
    {
        trans = transform;
    }

    // Randomly generate size and rotation direction initially.
    void Start()
    {
        // Initialize scaling to prevent errors.
        Vector3 scaling = Vector3.one;

        // Randomly generate shape
        scaling.x = Random.Range(scaling_min, scaling_max);
        scaling.y = Random.Range(scaling_min, scaling_max);
        scaling.z = Random.Range(scaling_min, scaling_max);

        // Set the 
        trans.localScale = scaling;

        // Randomly generate rotation
        rotation.x = Random.Range(-rotationSpeed, rotationSpeed);
        rotation.y = Random.Range(-rotationSpeed, rotationSpeed);
        rotation.z = Random.Range(-rotationSpeed, rotationSpeed);
    }

    void Update()
    {
        trans.Rotate(rotation * Time.deltaTime);
    }
}
