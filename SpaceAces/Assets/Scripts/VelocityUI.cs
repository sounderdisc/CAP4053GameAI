using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityUI : MonoBehaviour
{

    private Rigidbody Rigidbody;
    [SerializeField] private Text VelocityText;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        VelocityText.text = Rigidbody.velocity.magnitude.ToString("0");
    }
}
