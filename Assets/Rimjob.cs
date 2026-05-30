using System.Collections;

//using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rimjob : MonoBehaviour
{
    public GameObject rim1;
    private Vector2 rsc = new Vector2(0.67f,0.67f);
    public GameObject rim2;
    [SerializeField] private Image pauseMask;
    public GameObject rimJob;
    public BongBall bongBall;
    public Gamemanager gameManager;
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
    }

    private IEnumerator Expand()
    {
        rim1.transform.position = transform.position;
        rim2.transform.position = transform.position;
        rimJob.SetActive(true);
        yield return new WaitForSeconds(rate);
        rim1.transform.localScale = rsc;
        yield return new WaitForSeconds(rate);
        rim2.transform.localScale = rsc;

    }
    private IEnumerator Contract()
    {
        yield return new WaitForSeconds(rate*10);
        rim2.transform.localScale = Vector2.zero;
        yield return new WaitForSeconds(rate*10);
        rim1.transform.localScale = Vector2.zero;
        rimJob.SetActive(false);
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
