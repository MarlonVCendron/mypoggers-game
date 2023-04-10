using UnityEngine;

public class IslandController : MonoBehaviour
{
    public Transform boat;
    public float distance = 300f;
    private void Update() {
        transform.position = new Vector3(
            boat.position.x,
            transform.position.y,
            boat.position.z + distance
        );
    }
}

