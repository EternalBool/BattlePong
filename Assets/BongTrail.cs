using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class BallGhostTrail : MonoBehaviour
{
    public GameObject ghostPrefab;      // assign your faded ghost prefab
    public BongBall bongBall;
    public float spawnInterval = 0.05f; // how often to spawn ghosts
    public float ghostLifetime = 0.4f;  // how long ghosts last before fading
    public float fadeSpeed = 2f;        // how fast ghosts fade
    public float minVelocity = 10f;     // minimum velocity required to spawn ghosts
    public string velMode = "Roaming";
    private float timer = 0f;
    private Rigidbody2D rb2d;
    private Dictionary<string, Dictionary<string, float>> trailVel;

    void Awake()
    {
        trailVel = new Dictionary<string, Dictionary<string, float>>
        {
            ["Roaming"] = new Dictionary<string, float>
            {
                {"r", 0.5f},
                {"g", 0.5f},
                {"b", 0.5f},
            },
            ["Dashing"] = new Dictionary<string, float>
            {
                {"r", 1f},
                {"g", 1f},
                {"b", 1f},
            },
            ["Maxxing"] = new Dictionary<string, float>
            {
                {"r", 1f},
                {"g", 0f},
                {"b", 0f},
            },
            ["Spin"] = new Dictionary<string, float>
            {
                {"r", 1f},
                {"g", 0.8f},
                {"b", 0f},
            },
        };
    }
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
            Debug.LogWarning("BallGhostTrail: No Rigidbody2D found on this object!");
    }

    void Update()
    {
        velMode = velMode == "Spin" ? "Spin" : rb2d.linearVelocity.magnitude < minVelocity ? "Roaming" : Mathf.Abs(rb2d.linearVelocity.x) >= bongBall.maxVel ? "Maxxing" : "Dashing";
        // Only emit ghosts if moving fast enough in any direction
        if (rb2d != null && (rb2d.linearVelocity.magnitude > minVelocity || velMode == "Spin"))
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
        SpriteRenderer mainSR = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = mainSR.sortingOrder - 1;
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

            sr.color = new Color(trailVel[velMode]["r"], trailVel[velMode]["g"], trailVel[velMode]["b"], 1 - t);
            ghost.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null;
        }

        Destroy(ghost);
    }
}
