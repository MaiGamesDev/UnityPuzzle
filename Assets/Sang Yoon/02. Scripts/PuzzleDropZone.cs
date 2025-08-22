using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PuzzleDropZone : MonoBehaviour, IDropHandler
{
    public PuzzleExample puzzleExample;
    public ScoreManager scoreManager;
    public Timer timer;

    private JoystickController joystickController;
    // private ExampleFourPuzzlesTransform exampleFourPuzzlesTransform;

    private RectTransform rt;

    void Awake()
    {
        if (timer == null)
            timer = Object.FindFirstObjectByType<Timer>();

        scoreManager = GetComponent<ScoreManager>();
        // exampleFourPuzzlesTransform = GetComponent<ExampleFourPuzzlesTransform>();
        joystickController = GetComponent<JoystickController>();

        rt = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 드래그된 오브젝트에서 JoystickController 컴포넌트 가져오기
        var dropped = eventData.pointerDrag?.GetComponent<JoystickController>();
        if (dropped == null)
            return;
        // 제시된 4개의 퍼즐 Z축 회전값을 가지고 오기위함
        float pieceZ = dropped.GetComponent<RectTransform>().localEulerAngles.z;

        // 레이캐스트 블록 복구
        dropped.canvasGroup.blocksRaycasts = true;

        bool isIndexCor = dropped.puzzleIndex == puzzleExample.correctIndex;
        bool isRotationCor = pieceZ >= -0.01 && pieceZ <= 0.01;


        // 올바른 인덱스인지 비교
        if (isIndexCor && isRotationCor)
        {
            // 성공 사운드 재생
            SoundManager.Instance.PlaySuccess();

            Destroy(dropped.gameObject);
            puzzleExample.DestroyChildren();
            // 다음 레벨
            puzzleExample.PuzzleReset();

            ScoreManager.instance.AddScore(1);
            timer.StartTimer(15f);
        }
        else
        {
            // 실패 사운드 재생
            SoundManager.Instance.PlayFail();

            dropped.ResetPosition();
        }
    }
}
