using System;
using System.Collections.Specialized;
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
    public float yMult = 2f;
    public float maxVel;
    public float minYVel = 1.5f;
    public bool maxBouncyBool = false;

    //public float spinSped = 5f; //EXPERIMENT
    //private float currSpin; //EXPERIMENT
    public Gamemanager gameManager;

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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle1") || collision.gameObject.CompareTag("Paddle1"))
        {
            Vector2 v = rb.linearVelocity;
            float padVelY = 0f;
            if (collision.gameObject == p1)
            {
                var pad = collision.gameObject.GetComponent<Pad1>();
                if (pad != null) padVelY = pad.vertVel;
            }
            else if (collision.gameObject == p2)
            {
                var pad = collision.gameObject.GetComponent<Pad2>();
                if (pad != null) padVelY = pad.vertVel;
            }

            v.y += padVelY * 0.15f;
            v.y = Mathf.Clamp(v.y, -maxVel, maxVel);
            float xDir = Mathf.Sign(v.x);
            v.x = xDir * Mathf.Abs(rb.linearVelocity.x);
            rb.linearVelocity = v;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 v = rb.linearVelocity;
            if (Mathf.Abs(v.y) < minYVel)
            {
                float sign = Mathf.Sign(v.y);
                if (sign == 0) sign = 1f;
                v.y = sign * minYVel;
            }
            rb.linearVelocity = v;
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

        yVel = UnityEngine.Random.Range(-(yMult + 1.5f), (yMult + 1.5f));
        rb.linearVelocity = new Vector2(xVel * sped, yVel * yMult);

        Debug.Log($"GameManager = {gameManager}, gameProg = {gameManager?.gameProg}");
    }

    public void maxBounce(bool set)
    {
        PhysicsMaterial2D poneMat = p1.GetComponent<Collider2D>().sharedMaterial;
        PhysicsMaterial2D pwoMat = p2.GetComponent<Collider2D>().sharedMaterial;
        if (!set)
        {
            maxBouncyBool = false;
            Debug.Log("+Velocity");
            poneMat.bounciness = 1.05f;
            pwoMat.bounciness = 1.05f;
        }
        else
        {
            maxBouncyBool = true;
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
        if (gameManager.gameProg && maxBouncyBool)
        {
            if (rb.linearVelocity.magnitude < maxVel)
            {
                maxBounce(false);
            }
        }

        /*
        // *** EXIRIMENTO *** // *** EXIRIMENTO *** // *** EXIRIMENTO *** // *** EXIRIMENTO *** // *** EXIRIMENTO *** //
        if (Mathf.Abs(currSpin) > 0.01f)
        {
            // Rotate the sprite only (visual effect)
            transform.Rotate(Vector3.forward, currSpin * spinSped * Time.deltaTime * Mathf.Sign(rb.linearVelocity.x));

            // Optional: slowly reduce spin over time (spin decay)
            currSpin = Mathf.Lerp(currSpin, 0f, Time.deltaTime * 2f);
        }
        */
    } 
} 