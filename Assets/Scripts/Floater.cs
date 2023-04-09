using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public WaterChunkManager waterChunkManager;
    public Rigidbody rigidBody;
    public float displacementAmount = 3f;
    public int floaterCount = 4;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private void FixedUpdate() {
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        float waveHeight = waterChunkManager.GetMeshHeightAtPoint(transform.position.x, transform.position.z);
        if(transform.position.y < waveHeight) {
            float displacementMultiplier = Mathf.Clamp01(waveHeight - transform.position.y) * displacementAmount;
            rigidBody.AddForceAtPosition(
                new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),
                transform.position,
                ForceMode.Acceleration
            );
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
