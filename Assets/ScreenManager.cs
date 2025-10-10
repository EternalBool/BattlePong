using Unity.VisualScripting; 
using UnityEngine; 
using UnityEngine.UI; 
using System.Collections; 
using TMPro; 
using System.Diagnostics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
public class ScreenManager : MonoBehaviour 
{ 
    public Canvas canvas; 
    public Gamemanager gameManager; 
    public GameObject GamePanel; 
    private bool endGame = false; 
    private float delay = 0.1f; 
    private string GOTxt = "Game Over luh bruh"; 
    private IEnumerator TypeText(bool set) 
    { 
        TextMeshProUGUI gameText = GamePanel.transform.Find("GameOver").GetComponent<TextMeshProUGUI>(); 
        if (!set) 
        { 
            gameText.text = ""; 
            yield break; 
        } 
        else 
        { 
            gameText.text = ""; 
            yield return new WaitForSeconds(1); 
            foreach (char c in GOTxt.ToCharArray()) 
            { 
                gameText.text += c; 
                yield return new WaitForSeconds(delay); 
            } 
        } 
    } 

    private IEnumerator ButtonScroll(bool set) 
    { 
        GameObject resetButton = GamePanel.transform.Find("Reset").gameObject; 
        Button btn = resetButton.GetComponent<Button>(); 
        if (set == true) 
        { 
            Vector3 targetPos = new Vector3(0, -175, 0); 
            Vector3 startPos = new Vector3(0, -1000, 0); 
            resetButton.transform.localPosition = startPos; 
            resetButton.SetActive(false); 
            float duration = 2f;
            float elapsed = 0f; 
            
            yield return new WaitForSeconds(3); 
            resetButton.SetActive(true); 
            btn.OnDeselect(null); 

            while (elapsed < duration) 
            { 
                elapsed += Time.deltaTime; 
                resetButton.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration); 
                yield return null; 
            } 
            resetButton.GetComponent<RectTransform>().anchoredPosition = targetPos; 
        } 
        else 
        { 
            resetButton.SetActive(false); 
            resetButton.transform.localPosition = new Vector3(0, -1000, 0); 
        } 
    } 
    public void GameOver() 
    { 
        gameManager.gameProg = false; 
        endGame = true; 
        GamePanel.SetActive(true); 
        StartCoroutine(TypeText(true)); 
        StartCoroutine(ButtonScroll(true)); 
    } 
    public void OverGame() 
    { 
        gameManager.p1score = 3; 
        gameManager.p2score = 3; 
        gameManager.gameProg = true; 
        gameManager.UpdateScore(); 
        GamePanel.SetActive(false); 
        endGame = false; 
        gameManager.bongBall.halt(); 
        StartCoroutine(TypeText(false)); 
        StartCoroutine(gameManager.centBong()); 
        StartCoroutine(ButtonScroll(false)); 
    } 
    void Start() 
    { 
        GameObject resetButton = GamePanel.transform.Find("Reset").gameObject; 
        Button btn = resetButton.GetComponent<Button>(); 
        btn.onClick.AddListener(OverGame); 
    } 
    void Update() 
    { 
        if (!gameManager.gameProg) 
        { 
            if (!endGame) 
            {
                GameOver(); 
            } 
        } 
    } 
} 