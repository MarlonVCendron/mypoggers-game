using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LeviathanController : MonoBehaviour
{
    public GameObject player;
    public Vector3[] positions;
    public float[] times;

    public AudioClip[] audioClips;
    public AudioClip audioJumpscare;
    public AudioSource audioSource;


    private int currentPositionIndex = 0;
    private float elapsedTime = 0f;

    private void Start()
    {
        StartCoroutine(SurfaceAndSubmerge());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    private IEnumerator SurfaceAndSubmerge()
    {
        for (int i = 0; i < times.Length; i++)
        {
            yield return new WaitForSeconds(times[i] - elapsedTime);
            elapsedTime = 0f;

            PlayRandomAudio();
            StartCoroutine(MoveToPosition(positions[currentPositionIndex], positions[currentPositionIndex+1], true, 5f));

            currentPositionIndex+=2;
        }

        yield return new WaitForSeconds(3f);
        MoveTowardsPlayer();
    }

    private IEnumerator MoveToPosition(Vector3 startPosition, Vector3 targetPosition, bool relative, float duration, bool death = false)
    {
        if(relative) {
            startPosition = player.transform.position + startPosition;
            targetPosition = player.transform.position + targetPosition;
        }

        transform.position = startPosition;

        // Vector3 midPoint = (startPosition + endPosition) / 2 + Vector3.up * (startPosition - endPosition).magnitude / 2;
        Vector3 midPoint = (startPosition + targetPosition) / 2 + Vector3.up * 20;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            Vector3 currentPos = Vector3.Lerp(Vector3.Lerp(startPosition, midPoint, t), Vector3.Lerp(midPoint, targetPosition, t), t);
            transform.position = currentPos;

            yield return null;
        }

        transform.position = targetPosition;

        if(death){
            SceneManager.LoadScene("GameOver");
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 startPosition = new Vector3(playerPosition.x, -150, playerPosition.z + 600);
            Vector3 targetPosition = new Vector3(playerPosition.x, 20, playerPosition.z + 30);
            StartCoroutine(MoveToPosition(startPosition, targetPosition, false, 3f, true));

            audioSource.clip = audioJumpscare;
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void PlayRandomAudio()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the monster.");
            return;
        }

        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomIndex];
        audioSource.PlayOneShot(audioSource.clip);
    }
}
