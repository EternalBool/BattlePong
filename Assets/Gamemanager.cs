using Unity.VisualScripting; 
using UnityEngine; 
using System.Collections; 
using TMPro; 
using System.Timers; 

public class Gamemanager : MonoBehaviour 
{ 
     // Start is called once before the first execution of Update after the MonoBehaviour is created 

    public int p1score = 3; 
    public int p2score = 3; 
    public bool gameProg = false; 
    public string p1face = "Miles";
    public string p2face = "Botto";

    public BongBall bongBall; // Reference to BongBall script
    public ScreenManager screenManager; // Reference to ScreenManager script
    public GameObject bong;
    public GameObject p1;
    public GameObject p2;

    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI p1express; 
    public TextMeshProUGUI p2express; 

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
    
    public IEnumerator centBong() 
    {
        yield return new WaitForSeconds(1); 

        bong.transform.position = Vector2.zero; 

        Rigidbody2D rb = bong.GetComponent<Rigidbody2D>(); 
        float xVel = bong.GetComponent<BongBall>().xVel; 
        float yVel = bong.GetComponent<BongBall>().yVel; 
        float sped = bong.GetComponent<BongBall>().sped;
        float yMult = bong.GetComponent<BongBall>().yMult; 
        
        rb.linearVelocity = Vector2.zero; 

        yield return new WaitForSeconds(1); 

        if (gameProg) 
        { 
            bongBall.startVel(); 
        }   
    } 

    private void gameOver() 
    { 
        gameProg = false; 
        bongBall.halt(); 
        Debug.Log("Game Over!"); 
    } 

    void Start() 
    { 
        gameProg = true; // ***UPDATE LATER*** 
        p1score = 3; 
        p2score = 3; 

        screenManager.UpdateScore(p1score, p1face, p2score, p2face); 

        GameObject leftWall= GameObject.FindGameObjectWithTag("P2S");
        GameObject rightWall = GameObject.FindGameObjectWithTag("P1S");

        if (leftWall != null && rightWall != null)
        {
            leftWall.GetComponent<Renderer>().enabled = false;
            rightWall.GetComponent<Renderer>().enabled = false;
        } 

        if (bongBall != null)
        {
            bongBall.sped = 7;
            bongBall.startVel();
            bongBall.maxBounce(false);
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
                gameOver(); 
            } 
            else if (p2score <= 0) 
            { 
                Debug.Log("Player 1 Wins!"); 
                gameOver(); 
            } 
        } 
    } 
} 