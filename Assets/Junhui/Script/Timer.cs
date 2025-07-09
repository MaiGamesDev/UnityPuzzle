using System.Collections;
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

    private void Start()
    {
        currTime = time;
        StartTimer(12f);
    }

    private void Update()
    {
        TimePass();
    }

    void SetTimer()
    {
        timeLeft.fillAmount = currTime / time;
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
    }

    void StartTimer(float value)
    {
        isTimeout = false;
        time = value;
        currTime = time;
        SetTimer();
    }
    void TimeEnd()
    {
        manager.Gameover();
    }    
}
