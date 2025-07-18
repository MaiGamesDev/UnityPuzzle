using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Image art;
    public AudioClip audioClear;
    public Timer timer;
    public Sprite[] arts = new Sprite[30];
    private int currIndex = 0;

    private void Start()
    {
        art.sprite = arts[currIndex];
    }
    public void LevelClear()
    {
        SoundManager.Instance.PlaySound(audioClear);
        timer.StartTimer(15f);
        currIndex++;
        art.sprite = arts[currIndex];
    }
}
