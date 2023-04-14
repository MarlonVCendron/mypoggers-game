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
//------------------------------------------------------------------------------------------------
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BoatController : MonoBehaviour
// {
//     public float rowForce = 10f;
//     public float turnForce = 2f;

//     private Rigidbody rb;
//     private bool rowLeft;
//     private bool rowRight;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     void Update()
//     {
//         rowLeft = Input.GetKey(KeyCode.A);
//         rowRight = Input.GetKey(KeyCode.D);
//     }

//     void FixedUpdate()
//     {
//         if (rowLeft && rowRight)
//         {
//             // Row both sides, moving the boat forward
//             rb.AddForce(transform.forward * rowForce, ForceMode.Acceleration);
//         }
//         else if (rowLeft)
//         {
//             // Row only the left side, turning the boat right
//             rb.AddForceAtPosition(transform.forward * rowForce, transform.position + transform.right, ForceMode.Acceleration);
//             rb.AddTorque(0f, turnForce, 0f, ForceMode.Acceleration);
//         }
//         else if (rowRight)
//         {
//             // Row only the right side, turning the boat left
//             rb.AddForceAtPosition(transform.forward * rowForce, transform.position - transform.right, ForceMode.Acceleration);
//             rb.AddTorque(0f, -turnForce, 0f, ForceMode.Acceleration);
//         }
//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BoatController : MonoBehaviour
// {
//     public GameObject leftPaddle;
//     public GameObject rightPaddle;
//     public float paddlePower = 25f;
//     public float paddleCooldown = 1.5f;

//     private Rigidbody boatRigidbody;
//     private float leftPaddleCooldownTimer;
//     private float rightPaddleCooldownTimer;

//     void Start() {
//         boatRigidbody = GetComponent<Rigidbody>();
//         leftPaddleCooldownTimer = 0f;
//         rightPaddleCooldownTimer = 0f;
//     }

//     void Update() {
//         if (Input.GetKey(KeyCode.A) && leftPaddleCooldownTimer <= 0f) {
//             Row(leftPaddle.transform);
//             leftPaddleCooldownTimer = paddleCooldown;
//         }

//         if (Input.GetKey(KeyCode.D) && rightPaddleCooldownTimer <= 0f) {
//             Row(rightPaddle.transform);
//             rightPaddleCooldownTimer = paddleCooldown;
//         }

//         if (leftPaddleCooldownTimer > 0f) {
//             leftPaddleCooldownTimer -= Time.deltaTime;
//         }

//         if (rightPaddleCooldownTimer > 0f) {
//             rightPaddleCooldownTimer -= Time.deltaTime;
//         }
//     }

//     private void Row(Transform paddleTransform)
//     {
//         Vector3 forceDirection = paddleTransform.forward * paddlePower;
//         Vector3 forcePosition = paddleTransform.position;

//         boatRigidbody.AddForceAtPosition(forceDirection, forcePosition);
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float thrustForce = 10f;
    public float maxThrustTime = 1f;
    public float recoveryTime = 1f;

    private Rigidbody rb;
    private float leftThrustTimer;
    private float rightThrustTimer;
    private bool leftPaddleCooldown;
    private bool rightPaddleCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        leftThrustTimer = 0f;
        rightThrustTimer = 0f;
        leftPaddleCooldown = false;
        rightPaddleCooldown = false;
    }

    void Update()
    {
        CheckInput();
        UpdateTimers();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.A) && !leftPaddleCooldown) {
            leftThrustTimer += Time.deltaTime;
            if (leftThrustTimer <= maxThrustTime) {
                ApplyThrust(-1);
            } else {
                leftPaddleCooldown = true;
            }
        }

        if (Input.GetKey(KeyCode.D) && !rightPaddleCooldown) {
            rightThrustTimer += Time.deltaTime;
            if (rightThrustTimer <= maxThrustTime) {
                ApplyThrust(1);
            } else {
                rightPaddleCooldown = true;
            }
        }
    }

    private void ApplyThrust(int direction) {
        Vector3 force = transform.forward * thrustForce * Time.deltaTime;
        Vector3 torque = new Vector3(0, -direction * thrustForce * 0.4f * Time.deltaTime, 0);
        rb.AddForce(force);
        rb.AddTorque(torque);
    }

    private void UpdateTimers()
    {
        if (leftPaddleCooldown) {
            leftThrustTimer -= Time.deltaTime;
            if (leftThrustTimer <= 0f)
            {
                leftPaddleCooldown = false;
                leftThrustTimer = 0f;
            }
        }

        if (rightPaddleCooldown) {
            rightThrustTimer -= Time.deltaTime;
            if (rightThrustTimer <= 0f)
            {
                rightPaddleCooldown = false;
                rightThrustTimer = 0f;
            }
        }
    }
}
