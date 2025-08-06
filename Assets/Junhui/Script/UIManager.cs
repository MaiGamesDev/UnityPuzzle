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
    /// 클릭시 이펙트 생성
    /// </summary>
    public GameObject effectCanvas;
    public Sprite[] effectSprites;
    public float animSpeed = 0.1f;
    public Vector2 effectSize = new Vector2(300, 300);
    private bool touchProcessed= false; // 터치 처리

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

            // 터치 시작할 때만 이펙트 생성
            if (touch.phase == TouchPhase.Began && !touchProcessed)
            {
                touchProcessed = true;
                ScreenTouched(touch.position);
            }
            // 터치가 끝나면 리셋
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
        // UI 이미지 생성
        GameObject effect = new GameObject("TouchEffect");
        effect.transform.SetParent(effectCanvas.transform);

        Image img = effect.AddComponent<Image>();
        RectTransform rect = effect.GetComponent<RectTransform>();

        // 스크린 좌표를 캔버스 좌표로 변환
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            effectCanvas.GetComponent<RectTransform>(),
            screenPos, null, out localPos);
        rect.localPosition = localPos;

        // 크기 설정
        rect.sizeDelta = effectSize;
        img.raycastTarget = false;

        // 애니메이션 재생
        for (int i = 0; i < effectSprites.Length; i++)
        {
            img.sprite = effectSprites[i];
            yield return new WaitForSeconds(animSpeed);
        }

        // 삭제
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
