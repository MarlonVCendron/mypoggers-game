// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BoatController : MonoBehaviour
// {
//     public float rowForce = 10f;
//     public float turnTorque = 5f;
//     public float maxVelocity = 5f;
//     public float rowCooldown = 1f;
//     private Rigidbody rb;
//     private float lastRowTime = -1f;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     void FixedUpdate()
//     {
//         if (Time.fixedTime >= lastRowTime + rowCooldown)
//         {
//             if (Input.GetKeyDown(KeyCode.A))
//             {
//                 Debug.Log("A");
//                 RowLeft();
//             }
//             if (Input.GetKeyDown(KeyCode.D))
//             {
//                 Debug.Log("D");
//                 RowRight();
//             }
//         }
//     }

//     private void RowLeft()
//     {
//         Vector3 forwardForce = transform.forward * rowForce;
//         Vector3 torque = transform.up * turnTorque;

//         ApplyForceAndTorque(forwardForce, torque);
//         lastRowTime = Time.time;
//     }

//     private void RowRight()
//     {
//         Vector3 forwardForce = transform.forward * rowForce;
//         Vector3 torque = -transform.up * turnTorque;

//         ApplyForceAndTorque(forwardForce, torque);
//         lastRowTime = Time.time;
//     }

//     private void ApplyForceAndTorque(Vector3 forwardForce, Vector3 torque)
//     {
//         rb.AddForce(forwardForce);
//         rb.AddTorque(torque);

//         if (rb.velocity.magnitude > maxVelocity)
//         {
//             rb.velocity = rb.velocity.normalized * maxVelocity;
//         }
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float rowForce = 10f;
    public float turnForce = 2f;

    private Rigidbody rb;
    private bool rowLeft;
    private bool rowRight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rowLeft = Input.GetKey(KeyCode.A);
        rowRight = Input.GetKey(KeyCode.D);
    }

    void FixedUpdate()
    {
        if (rowLeft && rowRight)
        {
            // Row both sides, moving the boat forward
            rb.AddForce(transform.forward * rowForce, ForceMode.Acceleration);
        }
        else if (rowLeft)
        {
            // Row only the left side, turning the boat right
            rb.AddForceAtPosition(transform.forward * rowForce, transform.position + transform.right, ForceMode.Acceleration);
            rb.AddTorque(0f, turnForce, 0f, ForceMode.Acceleration);
        }
        else if (rowRight)
        {
            // Row only the right side, turning the boat left
            rb.AddForceAtPosition(transform.forward * rowForce, transform.position - transform.right, ForceMode.Acceleration);
            rb.AddTorque(0f, -turnForce, 0f, ForceMode.Acceleration);
        }
    }
}
