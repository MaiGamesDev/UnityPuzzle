using UnityEngine;
using UnityEngine.EventSystems;

// 스크립트 이름을 PuzzlePiece로 바꾸는 것을 권장합니다.
public class JoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform startParent;
    [HideInInspector] public CanvasGroup canvasGroup;

    public PuzzleExample puzzleExample;
    public int puzzleIndex;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        canvasGroup.blocksRaycasts = false;

        transform.SetParent(GetComponentInParent<Canvas>().transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<PuzzleDropZone>() == null)
        {
            ResetPosition();
            canvasGroup.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// 시작할 때 기록해 둔 위치와 부모로 되돌리는 기능
    /// </summary>
    public void ResetPosition()
    {
        transform.SetParent(startParent, true);
        transform.position = startPosition;
    }
}

    //public bool IsRectOverlapping(RectTransform rt1, RectTransform rt2)
    //{
    //    Vector3[] corners1 = new Vector3[4];
    //    Vector3[] corners2 = new Vector3[4];
    //    rt1.GetWorldCorners(corners1);
    //    rt2.GetWorldCorners(corners2);

    //    float minX1 = corners1[0].x, maxX1 = corners1[0].x;
    //    float minY1 = corners1[0].y, maxY1 = corners1[0].y;

    //    for (int i = 0; i < 4; i++)
    //    {
    //        minX1 = Mathf.Min(minX1, corners1[i].x);
    //        maxX1 = Mathf.Max(maxX1, corners1[i].x);
    //        minY1 = Mathf.Min(minY1, corners1[i].y);
    //        maxY1 = Mathf.Max(maxY1, corners1[i].y);
    //    }

    //    float minX2 = corners2[0].x, maxX2 = corners2[0].x;
    //    float minY2 = corners2[0].y, maxY2 = corners2[0].y;

    //    for (int i = 0; i < 4; i++)
    //    {
    //        minX2 = Mathf.Min(minX1, corners2[i].x);
    //        maxX2 = Mathf.Max(maxX1, corners2[i].x);
    //        minY2 = Mathf.Min(minY1, corners2[i].y);
    //        maxY2 = Mathf.Max(maxY1, corners2[i].y);
    //    }

    //    return
    //        minX1 < maxX2 &&
    //        maxX1 > minX2 &&
    //        minY1 < maxY2 &&
    //        maxY1 > minY2;
    //}

//using Unity.VisualScripting;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.U2D;

///// <summary>
///// 현제 스크립트는 검정 퍼즐조각 프리팹 15개에 사용되어있음
///// </summary>

//public class JoystickController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
//{
//    private Vector2 offset;
//    private bool isDragging = false;
//    private RectTransform rectTransform;
//    private Canvas canvas;

//    private EmptyPuzzleSlot emptyPuzzle;
//    private ExampleFourPuzzlesTransform fourPuzzles;

//    private PuzzleExample puzzleExample;


//    private void Awake()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        canvas = GetComponentInParent<Canvas>();

//        emptyPuzzle = GetComponent<EmptyPuzzleSlot>();
//        fourPuzzles = GetComponent<ExampleFourPuzzlesTransform>();
//        puzzleExample = GetComponent<PuzzleExample>();
//    }

//    private void Start()
//    {
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        Vector2 mousePos;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle // 월드 좌표를 스크린 좌표계로 변환
//            (
//            rectTransform.parent as RectTransform,
//            eventData.position,
//            canvas.worldCamera,
//            out mousePos
//            );
//        offset = rectTransform.anchoredPosition - mousePos;
//        isDragging = true;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        if (!isDragging) return;

//        Vector2 mousePos;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle // 월드 좌표를 스크린 좌표계로 변환
//            (
//            rectTransform.parent as RectTransform,
//            eventData.position,
//            canvas.worldCamera,
//            out mousePos
//            );
//        rectTransform.anchoredPosition = mousePos + offset;
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        isDragging = false;
//        if (emptyPuzzle == null) return;
//    }

//    /// <summary>
//    /// 틀렸을 때 퍼즐을 다시 돌려보내는 기능
//    /// </summary>
//    private void ReturnPuzzleToOriginPos() 
//    {
//        for (int i = 0; i < fourPuzzles.originPos.Length; i++)
//        {
//            fourPuzzles.puzzleExRTs[i].anchoredPosition = fourPuzzles.originPos[i];
//        }
//    }

//    public void PuzzleAreaExpansion()
//    {

//    }










//}
