using UnityEngine; 
using UnityEngine.InputSystem; 
public class Pad1 : MonoBehaviour 

{ 
    public float padSped = 15; 
    public Vector3 padPos = new Vector3(-8, 0, 0); 
    public Gamemanager gameManager; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    void Start() 
    {    
        /* 
        if (gameManager != null) 
        { 
            gameProg = gameManager.gameProg; 
        } 
        */ 
    }
    // Update is called once per frame 

    void Update() 
    {
        var keyboard = Keyboard.current; 
        if (keyboard != null && gameManager != null && gameManager.gameProg) 
        { 
            if (keyboard.wKey.isPressed && transform.position.y < 2.85f) 
            { 
                transform.Translate(Vector2.up * Time.deltaTime * padSped); 
            } 
            if (keyboard.sKey.isPressed && transform.position.y > -2.85f) 
            { 

                transform.Translate(Vector2.down * Time.deltaTime * padSped); 
            } 
        } 
        else 
        { 
            transform.position = padPos; 
        } 
    } 
} 