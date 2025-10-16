 using UnityEngine; 
using UnityEngine.InputSystem; 
public class Pad2 : MonoBehaviour
{ 
    public float padSped = 20; 
    public Vector3 padPos = new Vector3(8, 0, 0); 
    public Gamemanager gameManager;
    public float vertVel;
    public float lastY; 
    
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
    void Start() 
    {
        lastY = transform.position.y;
    } 
    // Update is called once per frame 
    void Update()
    { 
        float bound = 4.35f - (transform.localScale.y / 2);
        var keyboard = Keyboard.current;
        if (keyboard != null && gameManager != null && gameManager.gameProg)
        {
            float moveDir = 0f;
            if (keyboard.iKey.isPressed && transform.position.y < bound)
            {
                moveDir = 1f;
            }
            if (keyboard.kKey.isPressed && transform.position.y > -(bound))
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