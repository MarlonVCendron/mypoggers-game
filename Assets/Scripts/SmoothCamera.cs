using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform boat;
    public Vector3 cameraOffset;
    public float positionDampening = 0.1f;
    public float rotationDampening = 0.1f;
    public float mouseSensitivity = 100f;
    public float mouseRotationDampening = 0.1f;

    private float mouseX;
    private float mouseY;
    private Quaternion targetRotation;

    private void Start()
    {
        targetRotation = transform.rotation;

        // Lock the mouse cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // float horizontalRotationLimit = 110f;
        float verticalRotationLimit = 70f;
        // mouseX = Mathf.Clamp(mouseX, -horizontalRotationLimit, horizontalRotationLimit);
        mouseY = Mathf.Clamp(mouseY, -verticalRotationLimit, verticalRotationLimit);

        targetRotation = Quaternion.Euler(mouseY, mouseX, 0f);
    }

    private void FixedUpdate()
    {
        if (boat != null)
        {
            // Calculate the desired position of the camera relative to the boat
            Vector3 desiredPosition = boat.TransformPoint(cameraOffset);

            // Smoothly interpolate the camera's position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, positionDampening);

            // Calculate the desired rotation of the camera based on the boat's rotation
            Quaternion desiredBoatRotation = Quaternion.LookRotation(boat.forward, boat.up);

            // Smoothly interpolate the camera's rotation according to the boat's movement
            Quaternion boatRotation = Quaternion.Slerp(transform.rotation, desiredBoatRotation, rotationDampening);

            // Apply the boat's dampened rotation to the target rotation based on the mouse input
            Quaternion combinedRotation = boatRotation * Quaternion.Inverse(transform.rotation) * targetRotation;

            // Smoothly interpolate the camera's rotation according to the combined rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, combinedRotation, mouseRotationDampening);
        }
    }
}
