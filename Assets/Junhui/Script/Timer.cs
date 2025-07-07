using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float time = 30;
    private float currTime = 0;

    public Image timeLeft;
    public TextMeshProUGUI timeText;

    private void Start()
    {
        currTime = time;
        SetTimer();
        StartCoroutine(TimePass());
    }

    void SetTimer()
    {
        timeLeft.fillAmount = currTime / time;
        timeText.text = currTime.ToString();
    }    

    IEnumerator TimePass()
    {
        yield return new WaitForSeconds(1f);
        currTime -= 1f;
        SetTimer();
        if (currTime > 0)
            StartCoroutine(TimePass());
    }
}
