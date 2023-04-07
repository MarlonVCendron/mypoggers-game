using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private Vector3[] baseVertices;
    private Mesh mesh;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    // private void Update() {
    //     Vector3[] vertices = meshFilter.mesh.vertices;
    //     for (int i = 0; i < vertices.Length; i++) {
    //         // vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x, transform.position.z + vertices[i].z);
            
    //         Vector3 worldPos = transform.TransformPoint(vertices[i]);
    //         vertices[i].y = WaveManager.instance.GetWaveHeight(worldPos.x, worldPos.z);
    //         meshFilter.mesh.vertices = vertices;
    //         meshFilter.mesh.RecalculateNormals();
    //     }
    // }

    // private void Update() {
    //     Vector3[] vertices = meshFilter.mesh.vertices;
    //     for (int i = 0; i < vertices.Length; i++) {
    //         Vector3 worldPos = transform.TransformPoint(vertices[i]);
    //         Vector3 displacement = WaveManager.instance.GetWaveDisplacement(worldPos.x, worldPos.z, Time.time);
    //         Vector3 displacedWorldPos = worldPos + displacement;
    //         vertices[i] = transform.InverseTransformPoint(displacedWorldPos);
    //     }
    //     meshFilter.mesh.vertices = vertices;
    //     meshFilter.mesh.RecalculateNormals();
    // }

    private void Start() {
        mesh = meshFilter.mesh;
        baseVertices = mesh.vertices;
    }

    private void Update() {
        Vector3[] vertices = new Vector3[baseVertices.Length];
        for (int i = 0; i < baseVertices.Length; i++) {
            Vector3 worldPos = transform.TransformPoint(baseVertices[i]);
            Vector3 displacement = WaveManager.instance.GetWaveDisplacement(worldPos.x, worldPos.z, Time.time);
            Vector3 displacedWorldPos = worldPos + displacement;
            vertices[i] = transform.InverseTransformPoint(displacedWorldPos);
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    public float GetMeshHeightAtPoint(float x, float z) {
        float maxHeight = 10f;
        Vector3 rayOrigin = new Vector3(x, maxHeight, z);
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (meshCollider.Raycast(ray, out RaycastHit hitInfo, maxHeight * 2)){
            return hitInfo.point.y;
        } else {
            Debug.LogError("No intersection found with the mesh collider.");
            return 0f;
        }
    }
}
