 using UnityEngine; 
using UnityEngine.InputSystem; 
public class Pad2 : MonoBehaviour
{ 
    public float padSped = 15; 
    public Vector3 padPos = new Vector3(8, 0, 0); 
    public Gamemanager gameManager; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    void Start() 
    { 

    } 
    // Update is called once per frame 
    void Update() 
    { 
        var keyboard = Keyboard.current; 
        if (keyboard != null && gameManager != null && gameManager.gameProg) 
        { 
            if (keyboard.upArrowKey.isPressed && transform.position.y < 2.85f) 
            { 
                transform.Translate(Vector2.up * Time.deltaTime * padSped); 
            } 
            if (keyboard.downArrowKey.isPressed && transform.position.y > -2.85f) 
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