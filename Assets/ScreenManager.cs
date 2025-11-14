using Unity.VisualScripting; 
using UnityEngine; 
using UnityEngine.UI; 
using System.Collections; 
using TMPro; 
// using System.Diagnostics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine.InputSystem;
using System.IO.Compression;
using UnityEditor.U2D.Animation;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using NUnit.Framework;
using UnityEngine.PlayerLoop;
using UnityEditor.Rendering;
using Unity.Burst.Intrinsics;
//using UnityEngine.UIElements;
public class CharacterData
{
    public Vector3 Scale;
    public float Speed;
    public Color Color;
    public Dictionary<int, string> Expressions;
}
public class InputBuffer
{
    public bool space = false;
    public bool wasd = false;
    public bool ijkl = false;
}
public class ScreenManager : MonoBehaviour 
{ 
    public Canvas canvas; 
    public Gamemanager gameManager;
    public GameObject GamePanel;
    public GameObject IntroPanel;
    public GameObject p1;
    public GameObject p2;
    public TextMeshProUGUI p1express;
    public TextMeshProUGUI p2express;
    private bool endGame = false; 
    private string GOTxt = "Game Over luh bruh";
    private string INTxt = "Battle Pong";
    public InputBuffer inputBuffer = new InputBuffer();
    public bool p1db = false;
    private int p1ps = 0;
    public bool p2db = false;
    private int p2ps = 0;
    private bool pb1db = false;
    private bool pb2db = false;
    public Vector3 Scale;
    public Dictionary<int, string> Expressions;

    public static Dictionary<string, CharacterData> charData = new Dictionary<string, CharacterData>();
    private Dictionary<int, Dictionary<string, object>> pExpress;
    private static List<string> charNames = new(charData.Keys);

    void Awake()
    {
        pExpress = new Dictionary<int, Dictionary<string, object>>
        {
            [1] = new Dictionary<string, object>
            {
                { "Express", p1express },
                { "WinText", "P1 Wins!!!" },
                { "LossText", "P1 Loss..." },
            },
            [2] = new Dictionary<string, object>
            {
                { "Express", p2express },
                { "WinText", "P2 Wins!!!" },
                { "LossText", "P2 Loss..." },
            },
            [3] = new Dictionary<string, object>
            {
                { "Express", p1express },
                { "WinText", "Bot Wins!!!" },
                { "LossText", "Bot Loss..." },
            },
            [4] = new Dictionary<string, object>
            {
                { "Express", p2express },
                { "WinText", "Bot Wins!!!" },
                { "LossText", "Bot Loss..." },
            },
        };
        charData = new Dictionary<string, CharacterData>
        {
            ["Miles"] = new CharacterData
            {
                Speed = 15f,
                Scale = new Vector3(0.5f, 3f, 0.5f),
                Color = new Color(0.65f, 0.03f, 0.03f, 1f),
                Expressions = new Dictionary<int, string>
                {
                    { 0, "X(" }, { 1, "=(" }, { 2, "=/" }, { 3, "=)" }, { 4, "=D" }
                }
            },
            ["Brack"] = new CharacterData
            {
                Speed = 10f,
                Scale = new Vector3(0.75f, 4f, 0.75f),
                Color = new Color(0.5f, 0.5f, 0.5f, 1f),
                Expressions = new Dictionary<int, string>
                {
                    { 0, "X[" }, { 1, "=[" }, { 2, "=/" }, { 3, "=]" }, { 4, "=[]" }
                }
            },
            ["Curly"] = new CharacterData
            {
                Speed = 20f,
                Scale = new Vector3(0.35f, 2f, 0.35f),
                Color = new Color(0.47f, 0.3f, 0.2f, 1f),
                Expressions = new Dictionary<int, string>
                {
                    { 0, "X{" }, { 1, "={" }, { 2, "=/" }, { 3, "=}" }, { 4, "={}" }
                }
            },
            ["Botto"] = new CharacterData
            {
                Speed = 15f,
                Scale = new Vector3(0.5f, 3, 0.5f),
                Color = new Color(0f, 0f, 0f, 1f),
                Expressions = new Dictionary<int, string>
                {
                    { 0, "X()" }, { 1, "B(" }, { 2, "B/" }, { 3, "B)" }, { 4, "B()" }
                }
            }
        };
        charNames = new List<string>(charData.Keys);
    }


    public void UpdateScore(int poneScore, string poneFace, int pwoScore, string pwoFace)
    {
        p1express.text = charData[poneFace].Expressions[poneScore];
        p2express.text = charData[pwoFace].Expressions[pwoScore];
    }
    public void Initialize()
    {
        gameManager.p1score = 3;
        gameManager.p2score = 3;
        gameManager.SetSmile("P1", charNames[p1ps]);
        gameManager.SetSmile("P2", charNames[p2ps]);
        p1.GetComponent<Pad1>().padSped = charData[gameManager.p1face].Speed;
        p2.GetComponent<Pad2>().padSped = charData[gameManager.p2face].Speed;
        p1.transform.localScale = charData[gameManager.p1face].Scale;
        p2.transform.localScale = charData[gameManager.p2face].Scale;
    }
    public void Intro()
    {
        TextMeshProUGUI introText = IntroPanel.transform.Find("BattlePong").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI spacePrompt = IntroPanel.transform.Find("SpacePrompt").GetComponent<TextMeshProUGUI>();
        spacePrompt.enabled = false;
        IntroPanel.SetActive(true);
        StartCoroutine(Debounce(v => inputBuffer.space = v, 3f));
        StartCoroutine(TypeText(introText, INTxt, true, 0.1f, 0));
        StartCoroutine(TextPulse(spacePrompt, () => gameManager.playSel == true, 2f, 3f));
    }
    public void PlayerSelect()
    {
        TextMeshProUGUI introText = IntroPanel.transform.Find("BattlePong").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI spacePrompt = IntroPanel.transform.Find("SpacePrompt").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI fyft = IntroPanel.transform.Find("FYFT").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI fyfb = IntroPanel.transform.Find("FYFB").GetComponent<TextMeshProUGUI>();
        GameObject space = IntroPanel.transform.Find("SpaceBar").gameObject;
        GameObject vs = IntroPanel.transform.Find("Versus").gameObject;
        Transform p1p = IntroPanel.transform.Find("P1Panel");
        Transform p2p = IntroPanel.transform.Find("P2Panel");
        gameManager.playSel = true;
        StartCoroutine(TextPulse(spacePrompt, null, 15f, 0f, 1f));
        StartCoroutine(Debounce(v => inputBuffer.wasd = v, 3f));
        StartCoroutine(Debounce(v => inputBuffer.ijkl = v, 3f));
        StartCoroutine(Debounce(v => inputBuffer.space = v, 5f));
        StartCoroutine(Slide(introText.transform, introText.transform.localPosition, new Vector3(0, 1000, 0), 2f, 720f, 1f));
        StartCoroutine(Fade(IntroPanel, IntroPanel.GetComponent<Image>().color, new Color(1, 1, 1, 1), 1f, 1.5f));
        StartCoroutine(Slide(p1p.transform, new Vector3(-1000, 0, 0), new Vector3(-200, 0, 0), 1f, 0f, 2f));
        StartCoroutine(Slide(p2p.transform, new Vector3(1000, 0, 0), new Vector3(200, 0, 0), 1f, 0f, 2f));
        StartCoroutine(Fade(vs, vs.GetComponent<TextMeshProUGUI>().color, new Color(vs.GetComponent<TextMeshProUGUI>().color.r, vs.GetComponent<TextMeshProUGUI>().color.g, vs.GetComponent<TextMeshProUGUI>().color.b, 1), 1f, 2f));
        StartCoroutine(TextScroll(IntroPanel.GetComponent<RectTransform>(), fyft, 150f, () => gameManager.playSel == false, true, 3f));
        StartCoroutine(TextScroll(IntroPanel.GetComponent<RectTransform>(), fyfb, 150f, () => gameManager.playSel == false, false, 3f));
        StartCoroutine(Slide(space.transform, space.transform.localPosition, new Vector3(space.transform.localPosition.x, space.transform.localPosition.y + 200, space.transform.localPosition.z), 3f, -1f, 5f));
    }
    public void BottoBrain(string player, bool set, int brain)
    {
        if (set)
        {
            if (player == "P1")
            {
                Transform pb1p = IntroPanel.transform.Find("PB1Panel");
                StartCoroutine(Slide(pb1p, new Vector3(-500f, 0f, 0f), new Vector3(-335f, 0f, 0f), 0.5f));
            }
            if (player == "P2")
            {
                Transform pb2p = IntroPanel.transform.Find("PB2Panel");
                StartCoroutine(Slide(pb2p, new Vector3(500f, 0f, 0f), new Vector3(335f, 0f, 0f), 0.5f));
            }
        }
        else
        {
            if (player == "P1")
            {
                Transform pb1p = IntroPanel.transform.Find("PB1Panel");
                StartCoroutine(Slide(pb1p, new Vector3(-335f, 0f, 0f), new Vector3(-500f, 0f, 0f), 0.5f));
            }
            if (player == "P2")
            {
                Transform pb2p = IntroPanel.transform.Find("PB2Panel");
                StartCoroutine(Slide(pb2p, new Vector3(335f, 0f, 0f), new Vector3(500f, 0f, 0f), 0.5f));
            }
        }
    }
    private void FaceUp(string player)
    {
        //List<string> charNames = new List<string>(charData.Keys);
        if (player == "P1")
        {
            p1ps++;
            p1ps = (p1ps + 4) % 4;
            gameManager.SetSmile(player, charNames[p1ps]);
            TextMeshProUGUI poneFace = GameObject.FindGameObjectWithTag("P1PF").GetComponent<TextMeshProUGUI>();
            GameObject smileObj = Instantiate(poneFace.gameObject, new Vector2(poneFace.transform.localPosition.x, poneFace.transform.localPosition.y - 200f), poneFace.transform.localRotation);
            TextMeshProUGUI nextFace = smileObj.GetComponent<TextMeshProUGUI>();
            smileObj.transform.SetParent(poneFace.transform.parent, false);
            smileObj.name = poneFace.gameObject.name;
            nextFace.text = charData[gameManager.p1face].Expressions[3];
            nextFace.tag = "P1PF";
            nextFace.color = charData[gameManager.p1face].Color;
            StartCoroutine(Slide(poneFace.transform, poneFace.transform.localPosition, new Vector3(poneFace.transform.localPosition.x, poneFace.transform.localPosition.y + 200f, poneFace.transform.localPosition.z), 0.45f, -1f, -1f, true));
            StartCoroutine(Slide(nextFace.transform, nextFace.transform.localPosition, new Vector3(nextFace.transform.localPosition.x, nextFace.transform.localPosition.y + 200f, nextFace.transform.localPosition.z), 0.45f));
        }
        else if (player == "P2")
        {
            p2ps++;
            p2ps = (p2ps + 4) % 4;
            gameManager.SetSmile(player, charNames[p2ps]);
            TextMeshProUGUI pwoFace = GameObject.FindGameObjectWithTag("P2PF").GetComponent<TextMeshProUGUI>();
            GameObject smileObj = Instantiate(pwoFace.gameObject, new Vector2(pwoFace.transform.localPosition.x, pwoFace.transform.localPosition.y - 200f), pwoFace.transform.localRotation);
            TextMeshProUGUI nextFace = smileObj.GetComponent<TextMeshProUGUI>();
            smileObj.transform.SetParent(pwoFace.transform.parent, false);
            smileObj.name = pwoFace.gameObject.name;
            nextFace.text = charData[gameManager.p2face].Expressions[3];
            StartCoroutine(Slide(pwoFace.transform, pwoFace.transform.localPosition, new Vector3(pwoFace.transform.localPosition.x, pwoFace.transform.localPosition.y + 200f, pwoFace.transform.localPosition.z), 0.45f, -1f, -1f, true));
            nextFace.tag = "P2PF";
            nextFace.color = charData[gameManager.p2face].Color;
            StartCoroutine(Slide(nextFace.transform, nextFace.transform.localPosition, new Vector3(nextFace.transform.localPosition.x, nextFace.transform.localPosition.y + 200f, nextFace.transform.localPosition.z), 0.45f));
        }
    }
    private void FaceDown(string player)
    {
        //List<string> charNames = new List<string>(charData.Keys);
        if (player == "P1")
        {
            p1ps--;
            p1ps = (p1ps + 4) % 4;
            gameManager.SetSmile(player, charNames[p1ps]);
            TextMeshProUGUI poneFace = GameObject.FindGameObjectWithTag("P1PF").GetComponent<TextMeshProUGUI>();
            GameObject smileObj = Instantiate(poneFace.gameObject, new Vector2(poneFace.transform.localPosition.x, poneFace.transform.localPosition.y + 200f), poneFace.transform.localRotation);
            TextMeshProUGUI nextFace = smileObj.GetComponent<TextMeshProUGUI>();
            smileObj.transform.SetParent(poneFace.transform.parent, false);
            smileObj.name = poneFace.gameObject.name;
            nextFace.text = charData[gameManager.p1face].Expressions[3];
            StartCoroutine(Slide(poneFace.transform, poneFace.transform.localPosition, new Vector3(poneFace.transform.localPosition.x, poneFace.transform.localPosition.y - 200f, poneFace.transform.localPosition.z), 0.45f, -1f, -1f, true));
            nextFace.tag = "P1PF";
            nextFace.color = charData[gameManager.p1face].Color;
            StartCoroutine(Slide(nextFace.transform, nextFace.transform.localPosition, new Vector3(nextFace.transform.localPosition.x, nextFace.transform.localPosition.y - 200f, nextFace.transform.localPosition.z), 0.45f));
        }
        else if (player == "P2")
        {
            p2ps--;
            p2ps = (p2ps + 4) % 4;
            gameManager.SetSmile(player, charNames[p2ps]);
            TextMeshProUGUI pwoFace = GameObject.FindGameObjectWithTag("P2PF").GetComponent<TextMeshProUGUI>();
            GameObject smileObj = Instantiate(pwoFace.gameObject, new Vector2(pwoFace.transform.localPosition.x, pwoFace.transform.localPosition.y + 200f), pwoFace.transform.localRotation);
            TextMeshProUGUI nextFace = smileObj.GetComponent<TextMeshProUGUI>();
            smileObj.transform.SetParent(pwoFace.transform.parent, false);
            smileObj.name = pwoFace.gameObject.name;
            nextFace.text = charData[gameManager.p2face].Expressions[3];
            StartCoroutine(Slide(pwoFace.transform, pwoFace.transform.localPosition, new Vector3(pwoFace.transform.localPosition.x, pwoFace.transform.localPosition.y - 200f, pwoFace.transform.localPosition.z), 0.45f, -1f, -1f, true));
            nextFace.tag = "P2PF";
            nextFace.color = charData[gameManager.p2face].Color;
            StartCoroutine(Slide(nextFace.transform, nextFace.transform.localPosition, new Vector3(nextFace.transform.localPosition.x, nextFace.transform.localPosition.y - 200f, nextFace.transform.localPosition.z), 0.45f));
        }
    }
    private void FaceSwap(string player, string dir)
    {
        //List<string> charNames = new List<string>(charData.Keys);
        if (dir == "Up")
        {
            FaceUp(player);
            if (player == "P1")
            {
                StartCoroutine(Debounce(v => p1db = v, 0.5f));
            }
            else
            {
                StartCoroutine(Debounce(v => p2db = v, 0.5f));
            }
        }
        else if (dir == "Down")
        {
            FaceDown(player);
            if (player == "P1")
            {
                StartCoroutine(Debounce(v => p1db = v, 0.5f));
            }
            else
            {
                StartCoroutine(Debounce(v => p2db = v, 0.5f));
            }
        }
    }
    private void IntensitySwap(string player, string temp)
    {
        GameObject pb1p = IntroPanel.transform.Find("PB1Panel").gameObject;
        GameObject pb2p = IntroPanel.transform.Find("PB2Panel").gameObject;
        if (temp == "Heat")
        {
            if (player == "P1")
            {

                Animator bottonFire = pb1p.transform.Find("BottOnFire").GetComponent<Animator>();
                GameObject aKey = pb1p.transform.Find("aKey").gameObject;
                GameObject dKey = pb1p.transform.Find("dKey").gameObject;
                if (gameManager.PB1DIFF < 2)
                {
                    StartCoroutine(Slide(aKey.transform, aKey.transform.localPosition, new Vector3(aKey.transform.localPosition.x, aKey.transform.localPosition.y - 20, aKey.transform.localPosition.z), 0.25f));
                    StartCoroutine(Slide(dKey.transform, dKey.transform.localPosition, new Vector3(dKey.transform.localPosition.x, dKey.transform.localPosition.y - 20, dKey.transform.localPosition.z), 0.25f));
                    StartCoroutine(Morph(bottonFire.transform, bottonFire.transform.localScale, "Grow", 1.5f, 0.5f));
                } 
                gameManager.PB1DIFF++;
                gameManager.PB1DIFF = gameManager.PB1DIFF > 2 ? 2 : gameManager.PB1DIFF < 0 ? 0 : gameManager.PB1DIFF;
                bottonFire.SetInteger("BotDifficulty", gameManager.PB1DIFF);
                StartCoroutine(Debounce(v => pb1db = v, 0.5f));
            }
            else if (player == "P2")
            {
                Animator bottonFire = IntroPanel.transform.Find("PB2Panel").transform.Find("BottOnFire").GetComponent<Animator>();
                GameObject jKey = pb2p.transform.Find("jKey").gameObject;
                GameObject lKey = pb2p.transform.Find("lKey").gameObject;
                if (gameManager.PB2DIFF < 2)
                {
                    StartCoroutine(Slide(jKey.transform, jKey.transform.localPosition, new Vector3(jKey.transform.localPosition.x, jKey.transform.localPosition.y - 20, jKey.transform.localPosition.z), 0.25f));
                    StartCoroutine(Slide(lKey.transform, lKey.transform.localPosition, new Vector3(lKey.transform.localPosition.x, lKey.transform.localPosition.y - 20, lKey.transform.localPosition.z), 0.25f));
                    StartCoroutine(Morph(bottonFire.transform, bottonFire.transform.localScale, "Grow", 1.5f, 0.5f));
                } 
                gameManager.PB2DIFF++;
                gameManager.PB2DIFF = gameManager.PB2DIFF > 2 ? 2 : gameManager.PB2DIFF < 0 ? 0 : gameManager.PB2DIFF;
                bottonFire.SetInteger("BotDifficulty", gameManager.PB2DIFF);
                StartCoroutine(Debounce(v => pb2db = v, 0.5f));
            }
        }
        else if (temp == "Cool")
        {
            if (player == "P1")
            {
                Animator bottonFire = IntroPanel.transform.Find("PB1Panel").transform.Find("BottOnFire").GetComponent<Animator>();
                GameObject aKey = pb1p.transform.Find("aKey").gameObject;
                GameObject dKey = pb1p.transform.Find("dKey").gameObject;
                if (gameManager.PB1DIFF > 0)
                {
                    StartCoroutine(Slide(aKey.transform, aKey.transform.localPosition, new Vector3(aKey.transform.localPosition.x, aKey.transform.localPosition.y + 20, aKey.transform.localPosition.z), 0.25f));
                    StartCoroutine(Slide(dKey.transform, dKey.transform.localPosition, new Vector3(dKey.transform.localPosition.x, dKey.transform.localPosition.y + 20, dKey.transform.localPosition.z), 0.25f));
                    StartCoroutine(Morph(bottonFire.transform, bottonFire.transform.localScale, "Shrink", 1.5f, 0.5f));
                }
                gameManager.PB1DIFF--;
                gameManager.PB1DIFF = gameManager.PB1DIFF > 2 ? 2 : gameManager.PB1DIFF < 0 ? 0 : gameManager.PB1DIFF;
                bottonFire.SetInteger("BotDifficulty", gameManager.PB1DIFF);
                StartCoroutine(Debounce(v => pb1db = v, 0.5f));
            }
            else if (player == "P2")
            {
                Animator bottonFire = IntroPanel.transform.Find("PB2Panel").transform.Find("BottOnFire").GetComponent<Animator>();
                GameObject jKey = pb2p.transform.Find("jKey").gameObject;
                GameObject lKey = pb2p.transform.Find("lKey").gameObject;
                if (gameManager.PB2DIFF > 0)
                {
                    StartCoroutine(Slide(jKey.transform, jKey.transform.localPosition, new Vector3(jKey.transform.localPosition.x, jKey.transform.localPosition.y + 20, jKey.transform.localPosition.z), 0.25f));
                    StartCoroutine(Slide(lKey.transform, lKey.transform.localPosition, new Vector3(lKey.transform.localPosition.x, lKey.transform.localPosition.y + 20, lKey.transform.localPosition.z), 0.25f));
                    StartCoroutine(Morph(bottonFire.transform, bottonFire.transform.localScale, "Shrink", 1.5f, 0.5f));
                } 
                gameManager.PB2DIFF--;
                gameManager.PB2DIFF = gameManager.PB2DIFF > 2 ? 2 : gameManager.PB2DIFF < 0 ? 0 : gameManager.PB2DIFF;
                bottonFire.SetInteger("BotDifficulty", gameManager.PB2DIFF);
                StartCoroutine(Debounce(v => pb2db = v, 0.5f));
            }
        }
    }
    public IEnumerator Debounce(System.Action<bool> setter, float wait)
    {
        setter(true);
        yield return new WaitForSeconds(wait);
        setter(false);
    }
    private IEnumerator TextPulse(TextMeshProUGUI pulseText, Func<bool> condition, float pulseSpeed, float wait, float duration = -1f)
    {
        Color baseColor = new Color(pulseText.color.r, pulseText.color.g, pulseText.color.b, 1f);
        float alpha = 1f;

        yield return new WaitForSeconds(wait);
        pulseText.enabled = true;
        float elapsed = 0f;

        while (true)
        {
            alpha = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            pulseText.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            yield return null;
            elapsed += Time.deltaTime;
            if (condition != null && condition()) break;
            if (duration >= 0 && elapsed >= duration) break;
        }
        pulseText.color = baseColor;
    }
    private IEnumerator TypeText(TextMeshProUGUI typeText, string endText, bool set, float delay, float wait)
    {
        if (!set)
        {
            typeText.text = "";
            yield break;
        }
        else
        {
            typeText.text = "";
            yield return new WaitForSeconds(wait);
            foreach (char c in endText.ToCharArray())
            {
                typeText.text += c;
                yield return new WaitForSeconds(delay);
            }
        }
    }
    private IEnumerator Morph(Transform morphee, Vector3 size, string transform, float scale, float duration, float wait = -1f)
    {
        if (wait >= 0f) yield return new WaitForSeconds(wait);
        if (transform == "Shrink") scale = (1 / scale);
        Vector3 target = new Vector3(size.x * scale, size.y * scale, size.z * scale);
        morphee.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            morphee.localScale = Vector3.Lerp(morphee.localScale, target, elapsed / duration);
            yield return null;
        }
        if (morphee != null) morphee.localScale = target;
    }
    private IEnumerator Fade(GameObject fader, Color baseColor, Color goalColor, float duration, float wait = -1f)
    {
        Image img = fader.GetComponent<Image>();
        TextMeshProUGUI tmp = fader.GetComponent<TextMeshProUGUI>();
        float elapsed = 0f;
        if (wait >= 0) yield return new WaitForSeconds(wait);
        fader.SetActive(true);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            Color lerped = Color.Lerp(baseColor, goalColor, t);
            if (img != null) img.color = lerped;
            if (tmp != null) tmp.color = lerped;
            yield return null;
        }
        if (img != null) img.color = goalColor;
        if (tmp != null) tmp.color = goalColor;
    }
    private IEnumerator Slide(Transform slidee, Vector3 position, Vector3 destination, float duration, float rotation = -1f, float wait = -1f, bool termination = false)
    {
        float elapsed = 0f;
        Quaternion startRot = slidee.localRotation;
        if (wait >= 0f) yield return new WaitForSeconds(wait);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            slidee.localPosition = Vector3.Lerp(position, destination, t);
            if (rotation >= 0f)
            {
                float currentAngle = Mathf.Lerp(0f, rotation, t);
                slidee.localRotation = startRot * Quaternion.Euler(0f, 0f, currentAngle);
            }
            yield return null;
        }
        if (termination) Destroy(slidee.gameObject);
    }
    private IEnumerator Zoom(Transform panel, string dir, float duration, float wait = -1f)
    {
        GameObject zoomask = panel.transform.parent.gameObject;
        RectTransform zoomRect = zoomask.GetComponent<RectTransform>();
        if (wait >= 0) yield return new WaitForSeconds(wait);
        Vector2 target = new Vector2(400f, 820f);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            zoomRect.sizeDelta = Vector2.Lerp(zoomRect.sizeDelta, target, t);
            yield return null;
        }
        panel.gameObject.SetActive(false);
    }
    private IEnumerator TextScroll(RectTransform viewport, TextMeshProUGUI textMesh, float scrollSpeed, Func<bool> condition, bool scrollRight = false, float wait = -1f)
    {
        if (wait >= 0f) yield return new WaitForSeconds(wait);

        textMesh.enabled = true;
        textMesh.textWrappingMode = TextWrappingModes.NoWrap;
        textMesh.ForceMeshUpdate();

        float vpWidth = viewport.rect.width;
        float textWidth = textMesh.preferredWidth;
        string origin = textMesh.text;

        int repeat = Mathf.CeilToInt(vpWidth / textWidth) + 2;
        for (int i = 0; i < repeat; i++)
        {
            textMesh.text += " " + origin;
        }
        textMesh.ForceMeshUpdate();

        RectTransform rectTransform = textMesh.rectTransform;
        float totalWidth = textMesh.preferredWidth;
        rectTransform.sizeDelta = new Vector2(totalWidth, rectTransform.sizeDelta.y);

        while (!condition())
        {
            Vector2 pos = rectTransform.anchoredPosition;

            if (scrollRight)
            {
                pos.x += scrollSpeed * Time.deltaTime;
            }
            else
            {
              pos.x -= scrollSpeed * Time.deltaTime;  
            }
            if (!scrollRight && pos.x <= -textWidth)
            {
                pos.x += textWidth;
            }
            else if (scrollRight && pos.x >= 0)
            {
                pos.x -= textWidth;
            }
            
            rectTransform.anchoredPosition = pos;
            yield return null;
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
        Vector2 ponePlace = new Vector2(-135f, 231f);
        Vector2 pwoPlace = new Vector2(-26f, 231f);
        Vector2 poneFlexPos = new Vector2(-450f, 200f);
        Vector2 pwoFlexPos = new Vector2(-20f, 200f);
        Vector3 orgScale = new Vector3(1, 1, 1);
        float scale = 3f;
        float duration = 4f;
        float elapsed = 0f;
        Quaternion endRot = Quaternion.Euler(0, 0, -90);
        int winner;
        int loser;
        int bot = 2;

        if (gameManager.p1score == 0) { winner = 2; loser = 1; } else { winner = 1; loser = 2; }

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
                poneRes.text = gameManager.p1face != "Botto" ? (string)pExpress[winner]["WinText"] : (string)pExpress[winner + bot]["WinText"];
                pwoRes.text = gameManager.p2face != "Botto" ? (string)pExpress[loser]["LossText"] : (string)pExpress[loser + bot]["LossText"];
            }
            else
            {
                gameManager.p2score = 4;
                pwoRes.text = gameManager.p2face != "Botto" ? (string)pExpress[winner]["WinText"] : (string)pExpress[winner + bot]["WinText"];
                poneRes.text = gameManager.p1face != "Botto" ? (string)pExpress[loser]["LossText"] : (string)pExpress[loser + bot]["LossText"];
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
        else if (type == "In")
        {
            TextMeshProUGUI ponePsSmile = GameObject.FindGameObjectWithTag("P1PF").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI pwoPsSmile = GameObject.FindGameObjectWithTag("P2PF").GetComponent<TextMeshProUGUI>();
            RectTransform p1eRect = p1express.GetComponent<RectTransform>();
            RectTransform p1sRect = ponePsSmile.GetComponent<RectTransform>();
            RectTransform p2eRect = p2express.GetComponent<RectTransform>();
            RectTransform p2sRect = pwoPsSmile.GetComponent<RectTransform>();
            Transform p1p = IntroPanel.transform.Find("P1Panel").transform;
            Transform p2p = IntroPanel.transform.Find("P2Panel").transform;
            p1express.transform.localPosition = new Vector2(-290.9f, 74f);
            p2express.transform.localPosition = new Vector2(105.2f, 74f);
            p1eRect.sizeDelta = p1sRect.sizeDelta;
            p2eRect.sizeDelta = p2sRect.sizeDelta;
            p1express.fontSize = ponePsSmile.fontSize;
            p2express.fontSize = pwoPsSmile.fontSize;

            yield return new WaitForSeconds(1f);

            duration = 3f;
            //float rotation = 360f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                p1express.transform.localPosition = Vector3.Lerp(p1express.transform.localPosition, ponePlace, t);
                p2express.transform.localPosition = Vector3.Lerp(p2express.transform.localPosition, pwoPlace, t);
                p1express.fontSize = Mathf.Lerp(p1express.fontSize, 75f, t);
                p2express.fontSize = Mathf.Lerp(p2express.fontSize, 75f, t);
                //float currentAngle = Mathf.Lerp(0f, rotation, t);
                //p1express.transform.localRotation = p1express.transform.localRotation * Quaternion.Euler(0f, 0f, currentAngle);
                //p2express.transform.localRotation = p2express.transform.localRotation * Quaternion.Euler(0f, 0f, -currentAngle);
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
    public void ScreenIn()
    {
        Initialize();
        StartCoroutine(Flex("In"));
        UpdateScore(gameManager.p1score, gameManager.p1face, gameManager.p2score, gameManager.p2face);
        StartCoroutine(Zoom(IntroPanel.transform, "In", 2f));
        StartCoroutine(gameManager.GameIn());
    }
    public void OverScreen() 
    { 
        endGame = true;
        GamePanel.SetActive(true);
        StartCoroutine(Flex("Up"));
        StartCoroutine(TypeText(GamePanel.transform.Find("GameOver").GetComponent<TextMeshProUGUI>(), GOTxt, true, 0.1f, 3f)); 
        StartCoroutine(ButtonScroll(true)); 
    } 
    public void ScreenOver() 
    {
        Initialize();
        StartCoroutine(Flex("Out"));
        UpdateScore(gameManager.p1score, gameManager.p1face, gameManager.p2score, gameManager.p2face); 
        GamePanel.SetActive(false); 
        endGame = false;
        StartCoroutine(TypeText(GamePanel.transform.Find("GameOver").GetComponent<TextMeshProUGUI>(), null, false, 0, 0)); 
        StartCoroutine(ButtonScroll(false)); 
    } 
    void Start()
    {
        GameObject resetButton = GamePanel.transform.Find("Reset").gameObject; 
        Button btn = resetButton.GetComponent<Button>();
        btn.onClick.AddListener(gameManager.OverGame);
    } 
    void Update() 
    {
        if (!gameManager.gameProg)
        {
            if (!endGame && !gameManager.gameInit)
            {
                gameManager.GameOver();
            }
        }
        var keyboard = Keyboard.current;
        if (keyboard != null && gameManager != null && !gameManager.gameProg)
        {
            if (keyboard.rKey.wasPressedThisFrame)
            {
                gameManager.OverGame();
            }
            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && !gameManager.playSel && !inputBuffer.space)
                {
                    Debug.Log("Fix yo face!");
                    PlayerSelect();
                }
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.space)
                {
                    Debug.Log("Play Ball!");
                    ScreenIn();
                }
            }
            if (keyboard.wKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.wasd)
                {
                    if (!p1db) FaceSwap("P1", "Up");
                }
            }
            if (keyboard.sKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.wasd)
                {
                    if (!p1db) FaceSwap("P1", "Down");
                }
            }
            if (keyboard.iKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.ijkl)
                {
                    if (!p2db) FaceSwap("P2", "Up");
                }
            }
            if (keyboard.kKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.ijkl)
                {
                    if (!p2db) FaceSwap("P2", "Down");
                }
            }
            if (keyboard.aKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.wasd && gameManager.p1face == "Botto")
                {
                    if (!pb1db) IntensitySwap("P1", "Cool");
                }
            }
            if (keyboard.dKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.wasd && gameManager.p1face == "Botto")
                {
                    if (!pb1db) IntensitySwap("P1", "Heat");
                }
            }
            if (keyboard.jKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.ijkl && gameManager.p2face == "Botto")
                {
                    if (!pb2db) IntensitySwap("P2", "Cool");
                }
            }
            if (keyboard.lKey.wasPressedThisFrame)
            {
                if (gameManager.gameInit && gameManager.playSel && !inputBuffer.ijkl && gameManager.p2face == "Botto")
                {
                    if (!pb2db) IntensitySwap("P2", "Heat");
                }
            }
        }
    } 
} 