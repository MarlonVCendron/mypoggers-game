using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject boat;
    public FillingWater fillingWater;
    public float maxDistance = 10.0f;
    public string bucketTagName = "Bucket";
    public string waterTagName = "FillingWater";
    public float holdDistance = 3.5f;
    public LayerMask layerMask;

    public GameObject heldBucket;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleClick();
        }
    }

    void HandleClick() {
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask)) {
            if (heldBucket == null) {
                if (hit.collider.CompareTag(bucketTagName)) {
                    heldBucket = hit.collider.gameObject;
                    heldBucket.transform.SetParent(playerCamera.transform);
                    heldBucket.transform.localPosition = new Vector3(0, -3.4f, holdDistance);
                    heldBucket.transform.localRotation = Quaternion.identity;
                }
            } else {
                if (hit.collider.CompareTag(waterTagName)) {
                    fillingWater.RemoveBucketful();
                } else {
                    DropBucket();
                }
            }
        }
    }

    void DropBucket() {
        heldBucket.transform.SetParent(boat.transform);
        heldBucket.transform.localPosition = new Vector3(-0.079f, -0.23f, 0.896f);
        heldBucket.transform.localRotation = Quaternion.identity;
        heldBucket = null;
    }
}
