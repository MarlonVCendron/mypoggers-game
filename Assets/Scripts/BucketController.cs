using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 10.0f;
    public string bucketTagName = "Bucket";
    public float holdDistance = 0.3f;
    public LayerMask layerMask;

    private GameObject heldBucket;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (heldBucket == null) {
                PickupBucket();
            } else {
                DropBucket();
            }
        }

        if (heldBucket != null) {
            UpdateHeldBucketPosition();
        }
    }

    void PickupBucket()
    {
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        // Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask)) {
            // Debug.Log(hit.collider.gameObject);
            if (hit.collider.CompareTag(bucketTagName)) {
                heldBucket = hit.collider.gameObject;
            }
        }
    }

    void DropBucket() {
        // devolver
        heldBucket = null;
    }

    void UpdateHeldBucketPosition() {
        heldBucket.transform.position = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
        // heldBucket.transform.rotation = Quaternion.identity;
    }
}
