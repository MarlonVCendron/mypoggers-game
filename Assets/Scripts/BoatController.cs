using UnityEngine;
using UnityEngine.UI;

public class BoatController : MonoBehaviour {
    public BucketController bucketController;
    public float thrustForce = 10f;
    public float maxThrustTime = 1f;
    public float recoveryTime = 1f;
    public Image staminaBarFill;
    public float stamina = 100f;

    private Rigidbody rb;
    private float leftThrustTimer;
    private float rightThrustTimer;
    private bool leftPaddleCooldown;
    private bool rightPaddleCooldown;
    private float staminaRecoveryRate = 5f;
    private float staminaDecreaseRate = 10f;
    private float minStaminaToRow = 5f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        leftThrustTimer = 0f;
        rightThrustTimer = 0f;
        leftPaddleCooldown = false;
        rightPaddleCooldown = false;
    }

    void Update() {
        CheckInput();
        UpdateTimers();
    }

    private void CheckInput() {
        if(bucketController.heldBucket != null){
            return;
        }

        bool rowing = false;

        if (stamina >= minStaminaToRow) {
            if (Input.GetKey(KeyCode.A) && !leftPaddleCooldown) {
                rowing = true;
                leftThrustTimer += Time.deltaTime;
                if (leftThrustTimer <= maxThrustTime) {
                    ApplyThrust(-1);
                } else {
                    leftPaddleCooldown = true;
                }
            }

            if (Input.GetKey(KeyCode.D) && !rightPaddleCooldown) {
                rowing = true;
                rightThrustTimer += Time.deltaTime;
                if (rightThrustTimer <= maxThrustTime) {
                    ApplyThrust(1);
                } else {
                    rightPaddleCooldown = true;
                }
            }
        }

        UpdateStamina(rowing);
    }

    private void UpdateStamina(bool rowing) {
        if (rowing) {
            stamina -= staminaDecreaseRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, 100);
        } else {
            stamina += staminaRecoveryRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, 100);
        }

        UpdateStaminaBar();
    }

    private void UpdateStaminaBar() {
        float staminaPercentage = stamina / 100f;

        RectTransform rectTransform = staminaBarFill.GetComponent<RectTransform>();
        Vector2 newSizeDelta = rectTransform.sizeDelta;
        newSizeDelta.x = staminaPercentage * rectTransform.parent.GetComponent<RectTransform>().sizeDelta.x;
        rectTransform.sizeDelta = newSizeDelta;
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
