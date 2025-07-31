using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleDropZone : MonoBehaviour, IDropHandler
{
    public PuzzleExample puzzleExample;
    public Timer timer;

    public float plusTime = 6f;
    public float minusTime = 3f;

    void Awake()
    {
        if (timer == null)
            timer = Object.FindFirstObjectByType<Timer>();

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

        bool indexCor = dropped.puzzleIndex == puzzleExample.correctIndex;
        bool rotationCor = pieceZ == 0;

        // 올바른 인덱스인지 비교
        if (indexCor && rotationCor)
        {
            Destroy(dropped.gameObject);
            puzzleExample.DestroyChildren();
            // 다음 레벨
            puzzleExample.PuzzleReset();



            //if (timer.time > 15)
            //{
            //    Debug.Log("6초추가");
            //    timer.time = 15;
            //}
            //else
            //{
            //    Debug.Log("6초추가");
            //    timer.time = timer.time + plusTime;
            //}

        }
        else
        {
            Debug.Log("3초 감소");

            dropped.ResetPosition();
            //timer.time = timer.time - minusTime;
        }
    }
}
