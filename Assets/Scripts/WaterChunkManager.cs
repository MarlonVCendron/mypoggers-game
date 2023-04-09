using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterChunkManager : MonoBehaviour
{
    public GameObject boat;
    public GameObject waterPrefab;
    public Camera mainCamera;
    public int waterSize = 10;
    public int gridSize = 16;

    private GameObject[,] waterGrid;
    private Vector2Int currentPlayerGridPosition;
    private int activeTilesRange = 2;

    private void Awake() {
        waterGrid = new GameObject[gridSize, gridSize];
        currentPlayerGridPosition = new Vector2Int(-1, -1); // Initialize to an invalid position
        SpawnWaters();
        UpdateActiveWaters();
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
        int gridX = Mathf.RoundToInt(playerPosition.x / waterSize);
        int gridZ = Mathf.RoundToInt(playerPosition.z / waterSize);

        if (currentPlayerGridPosition.x != gridX || currentPlayerGridPosition.y != gridZ) {
            currentPlayerGridPosition = new Vector2Int(gridX, gridZ);

            // for (int i = 0; i < boat.transform.childCount; i++) {
            //     GameObject child = boat.transform.GetChild(i).gameObject;
            //     Floater floater = child.GetComponent<Floater>();
            //     if (floater != null) {
            //         int floaterGridX = Mathf.RoundToInt(floater.transform.position.x / waterSize);
            //         int floaterGridZ = Mathf.RoundToInt(floater.transform.position.z / waterSize);
            //         floater.waterManager = waterGrid[floaterGridX, floaterGridZ].GetComponent<WaterManager>();
            //     }
            // }
            
            UpdateWaterGridActivationPosition();
        }
        UpdateWaterGridActivationCamera();
    }

    private void UpdateWaterGridActivationPosition() {
        int startX = Mathf.Max(currentPlayerGridPosition.x - activeTilesRange, 0);
        int endX = Mathf.Min(currentPlayerGridPosition.x + activeTilesRange + 1, gridSize);
        int startZ = Mathf.Max(currentPlayerGridPosition.y - activeTilesRange, 0);
        int endZ = Mathf.Min(currentPlayerGridPosition.y + activeTilesRange + 1, gridSize);

        for (int x = startX; x < endX; x++) {
            for (int z = startZ; z < endZ; z++) {
                waterGrid[x, z].SetActive(true);
            }
        }
    }

    private void UpdateWaterGridActivationCamera() {
        int startX = Mathf.Max(currentPlayerGridPosition.x - activeTilesRange, 0);
        int endX = Mathf.Min(currentPlayerGridPosition.x + activeTilesRange + 1, gridSize);
        int startZ = Mathf.Max(currentPlayerGridPosition.y - activeTilesRange, 0);
        int endZ = Mathf.Min(currentPlayerGridPosition.y + activeTilesRange + 1, gridSize);

        for (int x = startX; x < endX; x++) {
            for (int z = startZ; z < endZ; z++) {
                bool currentTile = x ==  currentPlayerGridPosition.x && z == currentPlayerGridPosition.y;
                if(!currentTile){
                    bool shouldBeActive = IsTileInCameraView(waterGrid[x, z]);
                    waterGrid[x, z].SetActive(shouldBeActive);
                }
            }
        }
    }

    private bool IsTileInCameraView(GameObject tile) {
        float halfSize = waterSize / 2f;
        Vector3 center = tile.transform.position + new Vector3(halfSize, 0, halfSize);
        float padding = waterSize;
        Bounds tileBounds = new Bounds(center, new Vector3(waterSize + padding, 1, waterSize + padding));

        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, tileBounds);
    }

    public float GetMeshHeightAtPoint(float _x, float _z) {
        float maxHeight = 5f;
        int meshRange = 1;
        int startX = Mathf.Max(currentPlayerGridPosition.x - meshRange, 0);
        int endX = Mathf.Min(currentPlayerGridPosition.x + meshRange + 1, gridSize);
        int startZ = Mathf.Max(currentPlayerGridPosition.y - meshRange, 0);
        int endZ = Mathf.Min(currentPlayerGridPosition.y + meshRange + 1, gridSize);

        for (int x = startX; x < endX; x++) {
            for (int z = startZ; z < endZ; z++) {
                Vector3 rayOrigin = new Vector3(_x, maxHeight, _z);
                Ray ray = new Ray(rayOrigin, Vector3.down);


                MeshCollider meshCollider = waterGrid[x, z].GetComponent<MeshCollider>();
                if (meshCollider.Raycast(ray, out RaycastHit hitInfo, maxHeight * 2)){
                    return hitInfo.point.y;
                }
            }
        }
        Debug.LogError("No intersection found with the mesh collider.");
        return 0;
    }
}
