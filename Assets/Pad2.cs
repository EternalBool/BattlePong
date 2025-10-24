using System.Collections;
using UnityEngine; 
using UnityEngine.InputSystem;
public class Pad2 : MonoBehaviour
{
    public float padSped = 15;
    public Vector3 padPos = new Vector3(8, 0, 0);
    public GameObject bong;
    public Gamemanager gameManager;
    public float vertVel;
    public float lastY;
    public bool pc;
    private float botDateRate = 1f;
    private float viewDist = 4f;
    private float t;

    public void Regress(string face)
    {
        if (face == "Miles" || face == "Botto")
        {
            Vector3 scale = transform.localScale;
            scale.y -= 0.5f;
            transform.localScale = scale;
        }
        else if (face == "Brack")
        {
            padSped -= 3;
        }
    }
    // /* *** AI CONFIG *** // *** AI CONFIG *** // *** AI CONFIG *** //
    void Awake()
    {
        pc = gameManager.p2face == "Botto" ? false : true;
    }
    // */
    void Start()
    {
        lastY = transform.position.y;
        botDateRate = gameManager.PB2DIFF > 1 ? 0.05f : (gameManager.PB2DIFF < 1 ? 0.25f : 0.15f);
    }
    // Update is called once per frame 
    void Update()
    {
        float bound = 4.35f - (transform.localScale.y / 2);
        if (pc)
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && gameManager != null && gameManager.gameProg)
            {
                float moveDir = 0f;
                if (keyboard.iKey.isPressed && transform.position.y < bound)
                {
                    moveDir = 1f;
                }
                if (keyboard.kKey.isPressed && transform.position.y > -bound)
                {
                    moveDir = -1f;
                }
                if (transform.position.y > bound)
                {
                    transform.position = new Vector3(transform.position.x, bound, transform.position.z);
                }
                if (transform.position.y < -bound)
                {
                    transform.position = new Vector3(transform.position.x, -bound, transform.position.z);
                }
                transform.Translate(Vector2.up * moveDir * Time.deltaTime * padSped);
            }
            else
            {
                transform.position = padPos;
            }
            vertVel = (transform.position.y - lastY) / Time.deltaTime;
            lastY = transform.position.y;
        }
        else
        {
            
            if (bong != null && gameManager.gameProg)
            {
                t += Time.deltaTime;
                if (t >= botDateRate && bong.transform.position.x >= -viewDist)
                {
                    StartCoroutine(BallerP(bound));
                    t = 0f;
                }
            }
        }
    }
    private IEnumerator BallerP(float bound)
    {
        if (Mathf.Abs(bong.transform.position.y) < bound)
        {
            float elapsed = 0f;
            float duration = botDateRate * 0.9f;
            Vector3 startPos = transform.position;
            Vector3 targetPos = new Vector3(transform.position.x, bong.transform.position.y, transform.position.z);
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                yield return null;
            }
        }
       // else Debug.Log("out of bound");
    }
} 
