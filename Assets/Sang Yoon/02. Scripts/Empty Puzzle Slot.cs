//using UnityEngine;
//using UnityEngine.EventSystems;

//public class EmptyPuzzleSlot : MonoBehaviour, IDropHandler
//{
//    public string correctPieceID;

//    public void OnDrop(PointerEventData eventData)
//    {
//        Debug.Log(gameObject.name + "에 드롭되었습니다.");

//        GameObject droppedPiece = eventData.pointerDrag;
//        if (droppedPiece == null) return;

//        JoystickController puzzlePiece = droppedPiece.GetComponent<JoystickController>();
//        if (puzzlePiece != null && puzzlePiece.pieceID == this.correctPieceID)
//        {
//            // 정답!
//            Debug.Log("정답입니다: " + puzzlePiece.pieceID);

//            // 조각을 슬롯의 자식으로 만들고 위치를 고정합니다.
//            droppedPiece.transform.SetParent(this.transform);
//            droppedPiece.transform.position = this.transform.position;

//            // 더 이상 드래그되지 않도록 스크립트를 비활성화할 수 있습니다.
//            puzzlePiece.enabled = false;

//            // 다음 단계로 넘어가는 로직을 여기에 호출하세요.
//            // ex) GameManager.Instance.CheckForWin();
//        }
//        else
//        {
//            Debug.Log("오답입니다.");
//        }
//    }
//}


using System.Collections;
using UnityEngine;

public class EmptyPuzzleSlot : MonoBehaviour
{
    private Transform emptyPuzzle;
    public RectTransform emptyPuzzleRT;

    IEnumerator Start()
    {
        yield return null;

        emptyPuzzle = this.transform.GetChild(0);
        emptyPuzzleRT = emptyPuzzle.GetComponent<RectTransform>();

        //emptyPuzzleRT.sizeDelta = new Vector2(100, 100);

        if (emptyPuzzleRT != null)
        {
            Debug.Log($"퍼즐 구멍 위치 : {emptyPuzzleRT.anchoredPosition}");
            Debug.Log($"퍼즐 구멍 위치 : {emptyPuzzleRT.anchoredPosition.x}");
        }
    }
}
