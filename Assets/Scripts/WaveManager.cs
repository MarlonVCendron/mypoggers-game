using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [System.Serializable]
    public struct Wave {
        public float amplitude;
        public float length;
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

    public Vector3 GetWaveDisplacement(float x, float z, float time)
    {
        Vector3 displacement = Vector3.zero;

        foreach (Wave wave in waves)
        {
            float k = 2 * Mathf.PI / wave.length;
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

}
