using UnityEngine;

public class BoatController : MonoBehaviour {
    public BucketController bucketController;
    public float thrustForce = 10f;
    public float maxThrustTime = 1f;
    public float recoveryTime = 1f;
    public float stamina = 100f;

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

    private void CheckInput() {
        if(bucketController.heldBucket != null){
            return;
        }

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

    private void UpdateTimers() {
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
