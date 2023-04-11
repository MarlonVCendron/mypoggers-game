using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillingWater : MonoBehaviour
{
    public float fillSpeed = 0.0008f;
    public float bucketSize = 0.025f;

    private float maxY = -0.016f;
    private float minY = -0.27f;
    private bool isFilling = true;

    void Update() {
        if (isFilling) {
            IncreaseWaterLevel();
        }
    }

    void IncreaseWaterLevel() {
        Vector3 newPosition = transform.localPosition;
        newPosition.y += fillSpeed * Time.deltaTime;

        if (newPosition.y >= maxY) {
            newPosition.y = maxY;
            isFilling = false;
            Debug.Log("encheu");
        }

        transform.localPosition = newPosition;
    }

    public void RemoveBucketful() {
        Debug.Log("aaaaaaaaaaaaaaaaaa");
        Vector3 newPosition = transform.localPosition;
        newPosition.y -= bucketSize;

        if (newPosition.y < minY) {
            newPosition.y = minY;
        }

        transform.localPosition = newPosition;
    }
}
