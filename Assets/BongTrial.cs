using UnityEngine;
using System.Collections;

public class BallGhostTrail : MonoBehaviour
{
    public GameObject ghostPrefab;      // assign your faded ghost prefab
    public float spawnInterval = 0.05f; // how often to spawn ghosts
    public float ghostLifetime = 0.4f;  // how long ghosts last before fading
    public float fadeSpeed = 2f;        // how fast ghosts fade
    public float minVelocity = 10f;     // minimum velocity required to spawn ghosts

    private float timer = 0f;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
            Debug.LogWarning("BallGhostTrail: No Rigidbody2D found on this object!");
    }

    void Update()
    {
        // Only emit ghosts if moving fast enough in any direction
        if (rb2d != null && rb2d.linearVelocity.magnitude > minVelocity)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnGhost();
                timer = 0f;
            }
        }
    }

    void SpawnGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        if (sr != null)
            StartCoroutine(FadeAndShrink(ghost, sr));
    }

    private IEnumerator FadeAndShrink(GameObject ghost, SpriteRenderer sr)
    {
        float elapsed = 0f;
        Color startColor = sr.color;
        Vector3 startScale = ghost.transform.localScale;

        while (elapsed < ghostLifetime)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            float t = elapsed / ghostLifetime;

            sr.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);
            ghost.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null;
        }

        Destroy(ghost);
    }
}
