using UnityEngine;

public class RainController : MonoBehaviour
{
    public Transform boat;
    private void Update() {
        transform.position = new Vector3(boat.position.x, transform.position.y, boat.position.z);
    }
}
