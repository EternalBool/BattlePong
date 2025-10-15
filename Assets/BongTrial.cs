using UnityEngine;

public class BallTrail : MonoBehaviour
{
    public GameObject trailSquarePrefab;
    public int squareCount = 3;
    public float distanceBetween = 0.3f;
    public float shrinkFactor = 0.8f;
    public float fadeFactor = 0.5f;
    public float velocityThreshold = 10f; // 👈 Only emit above this speed

    private Transform[] squares;
    private Rigidbody2D rb;
    private bool emitting = false; // 👈 toggle for emission

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Create trail squares as children
        squares = new Transform[squareCount];
        for (int i = 0; i < squareCount; i++)
        {
            GameObject sq = Instantiate(trailSquarePrefab, transform.position, Quaternion.identity, transform);
            squares[i] = sq.transform;
            sq.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // start invisible
        }
    }

    void Update()
    {
        // 👇 check velocity to toggle emission
        float speed = rb.linearVelocity.magnitude;
        emitting = speed > velocityThreshold;

        for (int i = 0; i < squareCount; i++)
        {
            Transform sq = squares[i];
            SpriteRenderer sr = sq.GetComponent<SpriteRenderer>();

            // desired position behind the ball
            Vector3 targetPos = transform.position - (Vector3)rb.linearVelocity.normalized * distanceBetween * (i + 1);

            // smooth follow
            sq.position = Vector3.Lerp(sq.position, targetPos, Time.deltaTime * 8f);

            // scale down each subsequent square
            float scale = Mathf.Pow(shrinkFactor, i + 1);
            sq.localScale = Vector3.Lerp(sq.localScale, Vector3.one * scale, Time.deltaTime * 8f);

            // fade based on emission state and order
            float targetAlpha = emitting ? Mathf.Pow(fadeFactor, i + 1) : 0f;
            Color c = sr.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * 6f);
            sr.color = c;
        }
    }
}
