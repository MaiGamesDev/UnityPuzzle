using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject gameStartCanvas;
    public GameObject gameOverCanvas;

    public AudioClip audioGameStart;
    public AudioClip audioGameOver;

    [SerializeField] private TextMeshProUGUI scoreText;

    private bool isStart = false;

    private void Start()
    {
        ResetCanvas();
        gameStartCanvas.SetActive(true);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            ScreenTouched();
        }
    }
    void ScreenTouched()
    {
        if (!isStart)
        { 
            SoundManager.Instance.PlaySound(audioGameStart);
            ResetCanvas();
            canvas.SetActive(true);
            isStart = true;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Gameover()
    {
        SoundManager.Instance.PlaySound(audioGameOver);
        ResetCanvas();
        scoreText.text = ScoreManager.instance.totalScore.ToString();
        gameOverCanvas.SetActive(true);
    }

    private void ResetCanvas()
    {
        canvas.SetActive(false);
        gameStartCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }
}
