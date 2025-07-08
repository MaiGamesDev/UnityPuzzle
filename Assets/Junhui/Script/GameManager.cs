using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject gameStartCanvas;
    public GameObject gameOverCanvas;

    private void Start()
    {
        canvas.SetActive(false);
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
        canvas.SetActive(true);
        gameStartCanvas.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
