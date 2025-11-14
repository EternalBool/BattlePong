using Unity.VisualScripting; 
using UnityEngine; 
using System.Collections; 
using TMPro; 
using System.Timers;
using UnityEditor.Rendering.Universal;

public class Gamemanager : MonoBehaviour 
{ 
     // Start is called once before the first execution of Update after the MonoBehaviour is created 

    public int p1score = 3;
    public int p2score = 3;
    public bool gameInit = false;
    public bool gameProg = false;
    public bool playSel = false; 
    public string _p1face = "";
    public int PB1DIFF = 1;
    public string _p2face = "";
    public int PB2DIFF = 1;

    public BongBall bongBall; // Reference to BongBall script
    public ScreenManager screenManager; // Reference to ScreenManager script
    public GameObject bong;
    public GameObject p1;
    public GameObject p2;

    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI p1express;
    public TextMeshProUGUI p2express;
    public string p1face
    {
        get => _p1face;
        set
        {
            if (_p1face != value)
            {
                string oldValue = _p1face;
                _p1face = value;
                OnP1FaceChange(oldValue, _p1face);
            }
        }
    }
    public string p2face
    {
        get => _p2face;
        set
        {
            if (_p2face != value)
            {
                string oldValue = _p2face;
                _p2face = value;
                OnP2FaceChange(oldValue, _p2face);
            }
        }
    }
    private void OnP1FaceChange(string oldValue, string currValue)
    {
        if (currValue == "Botto")
        {
            screenManager.BottoBrain("P1", true, PB1DIFF);
        }
        else if (oldValue == "Botto")
        {
            screenManager.BottoBrain("P1", false, PB1DIFF);
        }
    }
    private void OnP2FaceChange(string oldValue, string currValue)
    {
        if (currValue == "Botto")
        {
            screenManager.BottoBrain("P2", true, PB1DIFF);
        }
        else if (oldValue == "Botto")
        {
            screenManager.BottoBrain("P2", false, PB1DIFF);
        }
    }
    public void PlayerScored(string player)
    {
        if (player == "p1")
        {
            p2score--;
            p2.GetComponent<Pad2>().Regress(p2face);
            Debug.Log("Player 1 Scored! Player 1: " + p1score + " Player 2: " + p2score);
        }
        else if (player == "p2")
        {
            p1score--;
            p1.GetComponent<Pad1>().Regress(p1face);
            Debug.Log("Player 2 Scored! Player 1: " + p1score + " Player 2: " + p2score);
        }
        screenManager.UpdateScore(p1score, p1face, p2score, p2face);
        if (gameProg)
        {
            StartCoroutine(centBong());
        }
    }
    public void SetSmile(string player, string smile)
    {
        if (player == "P1")
        {
            p1face = smile;
            //Debug.Log("P1 Face: " + p1face);
        }
        else if (player == "P2")
        {
            p2face = smile;
            //Debug.Log("P2 Face: " + p2face);
        }
    }

    public IEnumerator centBong()
    {
        yield return new WaitForSeconds(1f);

        bong.transform.position = Vector2.zero;

        Rigidbody2D rb = bong.GetComponent<Rigidbody2D>();
        float xVel = bong.GetComponent<BongBall>().xVel;
        float yVel = bong.GetComponent<BongBall>().yVel;
        float sped = bong.GetComponent<BongBall>().sped;
        float yMult = bong.GetComponent<BongBall>().yMult;

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1f);

        if (gameProg)
        {
            bongBall.startVel();
        }
    }
    public IEnumerator GameIn()
    {
        gameProg = true;
        gameInit = false;
        playSel = false;
        yield return new WaitForSeconds(1f);
        bongBall.maxBounce(false);
        StartCoroutine(centBong());
    }
    public void GameOver()
    {
        gameProg = false;
        bongBall.halt();
        screenManager.OverScreen();
        Debug.Log("Game Over!");
    } 
    public void OverGame()
    {
        p1score = 3; 
        p2score = 3;
        screenManager.ScreenOver();
        gameProg = true;
        gameInit = false;

        if (bongBall != null)
        {
            bongBall.sped = 7;
            StartCoroutine(centBong());
            bongBall.maxBounce(false);
        }
    }
    void Start()
    {
        gameInit = true;
        gameProg = false;
        screenManager.Intro();

        GameObject leftWall= GameObject.FindGameObjectWithTag("P2S");
        GameObject rightWall = GameObject.FindGameObjectWithTag("P1S");

        if (leftWall != null && rightWall != null)
        {
            leftWall.GetComponent<Renderer>().enabled = false;
            rightWall.GetComponent<Renderer>().enabled = false;
        } 
    } 
    // Update is called once per frame 

    void Update() 
    { 
        if (gameProg) 
        { 
                if (p1score <= 0) 
            { 
                Debug.Log("Player 2 Wins!"); 
                GameOver(); 
            } 
            else if (p2score <= 0) 
            { 
                Debug.Log("Player 1 Wins!"); 
                GameOver(); 
            } 
        } 
    } 
} 