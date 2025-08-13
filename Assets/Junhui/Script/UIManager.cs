using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject gameStartCanvas;
    public GameObject gameOverCanvas;

    public AudioClip audioGameStart;
    public AudioClip audioGameOver;

    [SerializeField] private TextMeshProUGUI scoreText;


    /// <summary>
    /// Ŭ���� ����Ʈ ����
    /// </summary>
    public GameObject effectCanvas;
    public Sprite[] effectSprites;
    public float animSpeed = 0.1f;
    public Vector2 effectSize = new Vector2(300, 300);
    private bool touchProcessed= false; // ��ġ ó��

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
            Touch touch = Input.GetTouch(0);

            // ��ġ ������ ���� ����Ʈ ����
            if (touch.phase == TouchPhase.Began && !touchProcessed)
            {
                touchProcessed = true;
                ScreenTouched(touch.position);
            }
            // ��ġ�� ������ ����
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                touchProcessed = false;
            }            
        }
        else
        {
            touchProcessed = false;
        }
    }

    void ScreenTouched(Vector2 screenPos)
    {
        if (!isStart)
        {
            SoundManager.Instance.PlaySound(audioGameStart);
            ResetCanvas();
            canvas.SetActive(true);
            isStart = true;
        }

            StartCoroutine(CreateEffect(screenPos));
    }

    IEnumerator CreateEffect(Vector2 screenPos)
    {
        // UI �̹��� ����
        GameObject effect = new GameObject("TouchEffect");
        effect.transform.SetParent(effectCanvas.transform);

        Image img = effect.AddComponent<Image>();
        RectTransform rect = effect.GetComponent<RectTransform>();

        // ��ũ�� ��ǥ�� ĵ���� ��ǥ�� ��ȯ
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            effectCanvas.GetComponent<RectTransform>(),
            screenPos, null, out localPos);
        rect.localPosition = localPos;

        // ũ�� ����
        rect.sizeDelta = effectSize;
        img.raycastTarget = false;

        // �ִϸ��̼� ���
        for (int i = 0; i < effectSprites.Length; i++)
        {
            img.sprite = effectSprites[i];
            yield return new WaitForSeconds(animSpeed);
        }

        // ����
        Destroy(effect);
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
