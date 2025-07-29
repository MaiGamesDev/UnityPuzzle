using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleDropZone : MonoBehaviour, IDropHandler
{
    public PuzzleExample puzzleExample;

    public void OnDrop(PointerEventData eventData)
    {
        // 드래그된 오브젝트에서 JoystickController 컴포넌트 가져오기
        var dropped = eventData.pointerDrag?.GetComponent<JoystickController>();
        if (dropped == null)
            return;

        // 레이캐스트 블록 복구
        dropped.canvasGroup.blocksRaycasts = true;

        // 올바른 인덱스인지 비교
        if (dropped.puzzleIndex == puzzleExample.correctIndex)
        {
            Debug.Log("정답입니다!");
            // 다음 레벨로
            puzzleExample.PuzzleOptions(puzzleExample.bgRandom);
        }
        else
        {
            Debug.Log("오답입니다!");
            dropped.ResetPosition();
        }
    }
}
