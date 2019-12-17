using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractTo : MonoBehaviour
{
    Rigidbody myRb;
    public Transform attractedTo;
    public float strengthOfAttraction, maxMagnitude; 

    void Start()
    {
        myRb = GetComponent<Rigidbody>();

    }
    void Update()
    {
        if (attractedTo != null)
        {
            Vector3 direction = attractedTo.position - transform.position;
            myRb.AddForce(strengthOfAttraction * direction);
            if (myRb.velocity.magnitude > maxMagnitude)
            {
                myRb.velocity = myRb.velocity.normalized * maxMagnitude;

            }
        }


    }
}
