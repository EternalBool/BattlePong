using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine; 
using UnityEngine.InputSystem; 
public class Pad1 : MonoBehaviour 

{ 
    public float padSped = 15; 
    public Vector3 padPos = new Vector3(-8, 0, 0);
    public Gamemanager gameManager;
    private float lastY;
    public float vertVel;
     
     public void Regress(string face)
    {
        if (face == "Miles" || face == "Botto")
        {
            Vector3 scale = transform.localScale;
            scale.y -= 1f;
            transform.localScale = scale;
        }
        else if (face == "Brack")
        {
            padSped -= 3;
        }
    }
    void Start() 
    {
        lastY = transform.position.y;
    }

    void Update() 
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && gameManager != null && gameManager.gameProg)
        {
            float moveDir = 0f;
            if (keyboard.wKey.isPressed && transform.position.y < 2.85f)
            {
                moveDir = 1f;
            }
            else if (keyboard.sKey.isPressed && transform.position.y > -2.85f)
            {
                moveDir = -1f;
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
} 