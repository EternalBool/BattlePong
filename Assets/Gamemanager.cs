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
    private string p1face = "Miles";
    private string p2face = "Brack";
    
    public BongBall bongBall; // Reference to BongBall script 
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI p1express; 
    public TextMeshProUGUI p2express; 

    public void PlayerScored(string player) 
    {    
        if (player == "p1") 
        { 
            p2score--; 
            Debug.Log("Player 1 Scored! Player 1: " + p1score + " Player 2: " + p2score); 
        } 
        else if (player == "p2") 
        { 
            p1score--; 
            Debug.Log("Player 2 Scored! Player 1: " + p1score + " Player 2: " + p2score); 
        } 
        UpdateScore(); 
        if (gameProg) 
        { 
            StartCoroutine(centBong()); 
        } 
    } 
    public void UpdateScore() 
    { 
        if (p1face == "Miles") 
            if (p1score == 3) 
                p1express.text = "=)"; 
            else if (p1score == 2) 
                p1express.text = "=/"; 
            else if (p1score == 1) 
                p1express.text = "=("; 
            else if (p1score <= 0) 
                p1express.text = "X("; 
            else if (p1score >= 4) 
                p1express.text = "=D"; 
        if (p1face == "Brack") 
            if (p1score == 3) 
                p1express.text = "=]"; 
            else if (p1score == 2) 
                p1express.text = "=/"; 
            else if (p1score == 1) 
                p1express.text = "=["; 
            else if (p1score <= 0) 
                p1express.text = "X["; 
            else if (p1score >= 4) 
                p1express.text = "=[]"; 
        if (p1face == "Curly") 
            if (p1score == 3) 
                p1express.text = "=}"; 
            else if (p1score == 2) 
                p1express.text = "=/"; 
            else if (p1score == 1) 
                p1express.text = "={"; 
            else if (p1score <= 0) 
                p1express.text = "X{"; 
            else if (p1score >= 4) 
                p1express.text = "={}"; 
         if (p2face == "Miles") 
            if (p2score == 3) 
                p2express.text = "=)"; 
            else if (p2score == 2) 
                p2express.text = "=/"; 
            else if (p2score == 1) 
                p2express.text = "=("; 
            else if (p2score <= 0) 
                p2express.text = "X("; 
            else if (p2score >= 4) 
                p2express.text = "=D"; 
        if (p2face == "Brack") 
            if (p2score == 3) 
                p2express.text = "=]"; 
            else if (p2score == 2) 
                p2express.text = "=/"; 
            else if (p2score == 1) 
                p2express.text = "=["; 
            else if (p2score <= 0) 
                p2express.text = "X["; 
            else if (p2score >= 4) 
                p2express.text = "=[]"; 
        if (p2face == "Curly") 
            if (p2score == 3) 
                p2express.text = "=}"; 
            else if (p2score == 2) 
                p2express.text = "=/"; 
            else if (p2score == 1) 
                p2express.text = "={"; 
            else if (p2score <= 0) 
                p2express.text = "X{"; 
            else if (p2score >= 4) 
                p2express.text = "={}"; 
    } 

    public IEnumerator centBong() 
    {
        yield return new WaitForSeconds(1); 

        GameObject bong = GameObject.FindGameObjectWithTag("Bong"); 
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

        UpdateScore(); 

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