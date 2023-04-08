using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterChunkManager : MonoBehaviour
{
    public GameObject boat;
    public GameObject waterPrefab;
    public int waterSize = 10;
    public int gridSize = 16;

    private GameObject[,] waterGrid;
    private Vector2Int currentPlayerGridPosition;

    private void Start() {
        waterGrid = new GameObject[gridSize, gridSize];
        currentPlayerGridPosition = new Vector2Int(-1, -1); // Initialize to an invalid position
        SpawnWaters();
    }

    private void Update() {
        UpdateActiveWaters();
    }

    private void SpawnWaters() {
        for (int x = 0; x < gridSize; x++) {
            for (int z = 0; z < gridSize; z++) {
                Vector3 waterPosition = new Vector3(x * waterSize, 0, z * waterSize);
                GameObject newWater = Instantiate(waterPrefab, waterPosition, Quaternion.identity, transform);
                newWater.SetActive(false);
                waterGrid[x, z] = newWater;
            }
        }
    }

    private void UpdateActiveWaters() {
        Vector3 playerPosition = boat.transform.position;
        int gridX = Mathf.FloorToInt(playerPosition.x / waterSize);
        int gridZ = Mathf.FloorToInt(playerPosition.z / waterSize);

        if (currentPlayerGridPosition.x != gridX || currentPlayerGridPosition.y != gridZ) {
            currentPlayerGridPosition = new Vector2Int(gridX, gridZ);
            UpdateWaterGridActivation();
        }
    }

    private void UpdateWaterGridActivation() {
        for (int x = 0; x < gridSize; x++) {
            for (int z = 0; z < gridSize; z++) {
                bool shouldBeActive = Mathf.Abs(x - currentPlayerGridPosition.x) <= 2 && Mathf.Abs(z - currentPlayerGridPosition.y) <= 2;
                waterGrid[x, z].SetActive(shouldBeActive);
            }
        }
    }
}
