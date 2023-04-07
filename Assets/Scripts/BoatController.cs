using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float reverseSpeed = 3f;
    [SerializeField] private float steeringSpeed = 0.5f;
    [SerializeField] private float steeringDamping = 2f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput != 0) {
            float speed = verticalInput > 0 ? forwardSpeed : reverseSpeed;
            // Sim, é estranho usar transform.right. É que a bosta do modelo veio virada de lado e foda-sseeeeeeeeee
            rb.AddForce(-transform.right * speed * verticalInput, ForceMode.Acceleration);
        }

        if (Mathf.Abs(horizontalInput) > 0.1f) {
            rb.AddTorque(0, horizontalInput * steeringSpeed, 0);
        } else {
            float currentAngularVelocityY = rb.angularVelocity.y;
            rb.AddTorque(0, -currentAngularVelocityY * steeringDamping, 0);
        }
    }
}
