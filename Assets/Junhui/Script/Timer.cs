using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float time = 30;
    private float currTime = 0;

    private bool isTimeout = false;

    public Image timeLeft;
    public UIManager manager;

    [SerializeField] private PuzzleExample puzzleEx;
    private bool hintTrigger = false;

    private void Start()
    {
        currTime = time;
        StartTimer(15f);
    }

    private void Update()
    {
        TimePass();
    }

    void SetTimer()
    {
        timeLeft.fillAmount = currTime / time;
        if (currTime < 5f) timeLeft.color = Color.red;
        else
        {
            Color color;
            ColorUtility.TryParseHtmlString("#FFB8FF", out color);
            timeLeft.color = color;
        }
    }

    void TimePass()
    {
        if (currTime <= 0)
        { 
            if (!isTimeout)
            {
                isTimeout = true;
                TimeEnd();
            }
            return;
        }
        currTime -= Time.deltaTime;
        SetTimer();

        if (!hintTrigger && currTime <= 5f)
        {
            hintTrigger = true;
            puzzleEx.ShowHint();
            puzzleEx.StartBlinking();
        }

        if (hintTrigger && currTime <= 4.5f)
        {
            puzzleEx.ClearOutlines();
        }
    }

    public void StartTimer(float value)
    {
        isTimeout = false;
        time = value;
        currTime = time;
        SetTimer();

        hintTrigger = false;
    }

    void TimeEnd()
    {
        manager.Gameover();
    }    
}
