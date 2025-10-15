using Unity.VisualScripting; 
using UnityEngine; 
using UnityEngine.UI; 
using System.Collections; 
using TMPro; 
using System.Diagnostics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine.InputSystem; 
public class ScreenManager : MonoBehaviour 
{ 
    public Canvas canvas; 
    public Gamemanager gameManager;
    public GameObject GamePanel;
    public TextMeshProUGUI p1express;
    public TextMeshProUGUI p2express;
    private bool endGame = false; 
    private float delay = 0.1f;
    private string GOTxt = "Game Over luh bruh";

    private Dictionary<string, Dictionary<int, string>> expressions;
    private Dictionary<int, Dictionary<string, object>> pExpress;

    void Awake()
    {
        pExpress = new Dictionary<int, Dictionary<string, object>>
        {
            [1] = new Dictionary<string, object>
            {
                {"Express", p1express },
                {"WinText", "P1 Wins!!!"},
                {"LossText", "P1 Loss..."},
            },
            [2] = new Dictionary<string, object>
            {
                {"Express", p2express },
                {"WinText", "P2 Wins!!!"},
                {"LossText", "P2 Loss..."},
            },
        };
        expressions = new Dictionary<string, Dictionary<int, string>>
        {
            ["Miles"] = new Dictionary<int, string>
            {
                {0, "X(" },
                {1, "=(" },
                {2, "=/" },
                {3, "=)" },
                {4, "=D" }
            },
            ["Brack"] = new Dictionary<int, string>
            {
                {0, "X[" },
                {1, "=[" },
                {2, "=/" },
                {3, "=]" },
                {4, "=[]" }
            },
            ["Curly"] = new Dictionary<int, string>
            {
                {0, "X{" },
                {1, "={" },
                {2, "=/" },
                {3, "=}" },
                {4, "={}" }
            },
            ["Botto"] = new Dictionary<int, string>
            {
                {0, "X()" },
                {1, "B)" },
                {2, "B/" },
                {3, "B)" },
                {4, "B()" }
            },
        };
    }

    public void UpdateScore(int poneScore, string poneFace, int pwoScore, string pwoFace)
    {
        p1express.text = expressions[poneFace][poneScore];
        p2express.text = expressions[pwoFace][pwoScore];
    }
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
            yield return new WaitForSeconds(3); 
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

            yield return new WaitForSeconds(5);
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
    private IEnumerator Flex(string type)
    {
        TextMeshProUGUI poneRes = GamePanel.transform.Find("P1Res").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI pwoRes = GamePanel.transform.Find("P2Res").GetComponent<TextMeshProUGUI>();
        Vector2 ponePlace = new Vector2(-43.2f, 96.5f);
        Vector2 pwoPlace = new Vector2(59.7f, 96.5f);
        Vector2 poneFlexPos = new Vector2(-247, -40);
        Vector2 pwoFlexPos = new Vector2(261, -40);
        Vector3 orgScale = new Vector3(1, 1, 1);
        float scale = 3f;
        float duration = 4f;
        float elapsed = 0f;
        Quaternion endRot = Quaternion.Euler(0,0,-90);
        int winner;
        int loser;

        if (gameManager.p1score == 0) {winner = 2; loser = 1; } else { winner = 1; loser = 2;}

        if (type == "Up")
        {
            p1express.enabled = false;
            p2express.enabled = false;
            p1express.transform.localScale *= scale;
            p2express.transform.localScale *= scale;
            p1express.transform.localPosition = poneFlexPos;
            p2express.transform.localPosition = pwoFlexPos;

            if (winner == 1)
            {
                gameManager.p1score = 4;
                poneRes.text = (string)pExpress[winner]["WinText"];
                pwoRes.text = (string)pExpress[loser]["LossText"];
            }
            else
            {
                gameManager.p2score = 4;
                pwoRes.text = (string)pExpress[winner]["WinText"];
                poneRes.text = (string)pExpress[loser]["LossText"];
            }

            UpdateScore(gameManager.p1score, gameManager.p1face, gameManager.p2score, gameManager.p2face);
            yield return new WaitForSeconds(0.5f);
            ((TextMeshProUGUI)pExpress[winner]["Express"]).enabled = true;
            yield return new WaitForSeconds(1f);
            ((TextMeshProUGUI)pExpress[loser]["Express"]).enabled = true;
            yield return new WaitForSeconds(1f);
            poneRes.enabled = true; pwoRes.enabled = true;

            while (!gameManager.gameProg)
            {
                p1express.transform.Rotate(0, 0, 100f * Time.deltaTime);
                p2express.transform.Rotate(0, 0, -100f * Time.deltaTime);
                yield return null;
            }
        }
        else if (type == "Out")
        {
            poneRes.enabled = false; pwoRes.enabled = false;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                p1express.transform.localScale = Vector3.Lerp(p1express.transform.localScale, orgScale, t);
                p2express.transform.localScale = Vector3.Lerp(p2express.transform.localScale, orgScale, t);
                p1express.transform.localRotation = Quaternion.Lerp(p1express.transform.localRotation, endRot, t);
                p2express.transform.localRotation = Quaternion.Lerp(p2express.transform.localRotation, endRot, t);
                p1express.transform.localPosition = Vector3.Lerp(p1express.transform.localPosition, ponePlace, t);
                p2express.transform.localPosition = Vector3.Lerp(p2express.transform.localPosition, pwoPlace, t);
                yield return null;
            }
        }
    }
    public void GameOver() 
    { 
        gameManager.gameProg = false; 
        endGame = true;
        GamePanel.SetActive(true);
        StartCoroutine(Flex("Up"));
        StartCoroutine(TypeText(true)); 
        StartCoroutine(ButtonScroll(true)); 
    } 
    public void OverGame() 
    { 
        gameManager.p1score = 3;
        gameManager.p2score = 3;
        GameObject pad1 = GameObject.FindGameObjectWithTag("Paddle1");
        GameObject pad2 = GameObject.FindGameObjectWithTag("Paddle2");
        Vector3 scale1 = pad1.transform.localScale;
        Vector3 scale2 = pad2.transform.localScale;
        scale1.y = 3f;
        scale2.y = 3f;
        gameManager.gameProg = true;
        StartCoroutine(Flex("Out"));
        UpdateScore(gameManager.p1score, gameManager.p1face, gameManager.p2score, gameManager.p2face); 
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
        var keyboard = Keyboard.current;
        if (keyboard != null && gameManager != null && !gameManager.gameProg)
        {
            if (keyboard.rKey.wasPressedThisFrame)
            {
                OverGame();
            }
        }
    } 
} 