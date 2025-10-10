using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine; 

 public class BongBall : MonoBehaviour
{
    public GameObject bong;
    public GameObject p1;
    public GameObject p2;
    public Rigidbody2D rb; 
    public float xVel; 
    public float yVel; 
    public float sped;
    public float yMult = 0f;
    public float maxVel;
    public Gamemanager gameManager; // Reference to GameManager script 

    void OnTriggerEnter2D(Collider2D other) 
    { 
        if (other.CompareTag("P2S")) 
        { 
            gameManager.PlayerScored("p2"); 
        } 
        else if (other.CompareTag("P1S")) 
        { 
            gameManager.PlayerScored("p1"); 
        } 
    }
    public void startVel()
    {
        rb.freezeRotation = true;
        bool isRight = UnityEngine.Random.value >= 0.5;
        xVel = -1f;

        if (isRight)
        {
            xVel = 1f;
        }

        yVel = UnityEngine.Random.Range(-(yMult), (yMult));
        rb.linearVelocity = new Vector2(xVel * sped, yVel * yMult);

        Debug.Log($"GameManager = {gameManager}, gameProg = {gameManager?.gameProg}");
    }

    public void maxBounce(bool set)
    {
        PhysicsMaterial2D poneMat = p1.GetComponent<Collider2D>().sharedMaterial;
        PhysicsMaterial2D pwoMat = p2.GetComponent<Collider2D>().sharedMaterial;
        if (set == false)
        {
            Debug.Log("+Velocity");
            poneMat.bounciness = 1.05f;
            pwoMat.bounciness = 1.05f;
        }
        else
        {
            poneMat.bounciness = 1f;
            pwoMat.bounciness = 1f;
        }        

    }

    public void halt() 
    { 
        bong.transform.position = Vector2.zero; 
        rb.linearVelocity = Vector2.zero;
    } 

    [System.Obsolete] 
    void Start()
    {
        /*
        maxBounce(false);
        Debug.Log("Warming Up!");
        if (gameManager != null && gameManager.gameProg)
        {
            Debug.Log("Game On!");
            sped = 7;
            startVel();
        } 
        */
    } 

    void Update()
    {
        if (math.abs(rb.linearVelocityX) > maxVel)
        {
            Debug.Log("MAX BOUNCINESS");
            rb.linearVelocityX = rb.linearVelocityX > 0 ? maxVel : -maxVel;
            maxBounce(true);
        }
    } 
} 