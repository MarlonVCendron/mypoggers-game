using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    // private float offset = 0f;

    [System.Serializable]
    public struct Wave {
        public float amplitude;
        public float frequency;
        public float phase;
        public Vector2 direction;
    }

    [SerializeField] private Wave[] waves;

    private void Awake() {
        if(instance == null){
            instance = this;
        } else if (instance != this) {
            Debug.Log("Instance already exists. Destroying");
            Destroy(this);
        }
    }

    // private void Update() {
    //     offset += Time.deltaTime;
    // }

    // public float GetWaveHeight(float x, float z, float time) {
    //     return GetWaveDisplacement(x, z, time).y;
    // }

    

    public float GetWaveHeight(float x, float z, float time)
    {
        return GetWaveDisplacement(x, z, time).y;
    }


    public Vector3 GetWaveDisplacement(float x, float z, float time)
    {
        Vector3 displacement = Vector3.zero;

        foreach (Wave wave in waves)
        {
            float k = 2 * Mathf.PI / wave.frequency;
            float dotProduct = Vector2.Dot(new Vector2(x, z), wave.direction.normalized);
            float phase = k * dotProduct + wave.phase + time;

            float sinPhase = Mathf.Sin(phase);
            float cosPhase = Mathf.Cos(phase);

            displacement.x += wave.amplitude * wave.direction.normalized.x * cosPhase;
            displacement.z += wave.amplitude * wave.direction.normalized.y * cosPhase;
            displacement.y += wave.amplitude * sinPhase;
        }

        return displacement;
    }

    // public float GetWaveHeight(float x, float z) {
    //     float waveHeight = 0f;

    //     foreach (Wave wave in waves) {
    //         float waveX = (x * wave.direction.x + offset) * wave.frequency;
    //         float waveZ = (z * wave.direction.y + offset) * wave.frequency;

    //         waveHeight += wave.amplitude * Mathf.Sin(waveX + waveZ);
    //     }

    //     return waveHeight;
    // }
}
