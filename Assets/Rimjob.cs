using System.Collections;

//using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rimjob : MonoBehaviour
{
    public GameObject rim1;
    public GameObject rim2;
    public GameObject back;
    private Vector2[] bsc = {new Vector2(2f,2f),new Vector2(1.5f,1.5f),new Vector2(1f,1f)};
    private Vector2 rsc = new Vector2(0.67f,0.67f);
    [SerializeField] private Image pauseMask;
    public GameObject rimJob;
    public BongBall bongBall;
    public Gamemanager gameManager;
    public ScreenManager screenManager;
    public float rate = 0.025f;
    public float mult = 10f;
    private bool rimmed;
    private bool debounce;
    private Rigidbody2D rb2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rimJob.SetActive(false);
        pauseMask.enabled = false;
        rim1.transform.localScale = Vector2.zero;
        rim2.transform.localScale = Vector2.zero;
        back.transform.localScale = Vector2.zero;
    }

    private IEnumerator Expand()
    {
        screenManager.FaceVis(false);
        rim1.transform.position = transform.position;
        rim2.transform.position = transform.position;
        back.transform.position = transform.position;
        rimJob.SetActive(true);
        back.transform.localScale = Vector2.zero;
        yield return new WaitForSeconds(rate);
        rim1.transform.localScale = rsc;
        back.transform.localScale = bsc[2];
        yield return new WaitForSeconds(rate);
        rim2.transform.localScale = rsc;
        back.transform.localScale = bsc[0];

    }
    private IEnumerator Contract()
    {
        back.transform.localScale = bsc[1];
        yield return new WaitForSeconds(rate*10);
        rim2.transform.localScale = Vector2.zero;
        back.transform.localScale = bsc[2];
        yield return new WaitForSeconds(rate*10);
        rim1.transform.localScale = Vector2.zero;
        back.transform.localScale = Vector2.zero;
        rimJob.SetActive(false);
        screenManager.FaceVis(true);
    }
    // Update is called once per frame
    void Update()
    {
        //rimmed = (gameManager.gameHalt) ? true : false;
        if (gameManager.gameHalt)
        {
            if (!rimmed)
            {
                rimmed = true;
                StartCoroutine(Expand());
            }
        }
        else
        {
            if (rimmed) StartCoroutine(Contract());
            rimmed = false;
        }
    }
}
