using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

// 스크립트 이름을 PuzzlePiece로 바꾸는 것을 권장합니다.
public class JoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 startPosition;
    public Vector2 endPosition;
    private Transform startParent;
    private RectTransform rt;

    [HideInInspector] public CanvasGroup canvasGroup;

    public PuzzleExample puzzleExample;
    public int puzzleIndex;

    public bool isDraggable = true; // 드래그 가능 여부 추가(25-08-08)

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup = GetComponent<CanvasGroup>();
        puzzleExample = GetComponent<PuzzleExample>();
        rt = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        startPosition = rt.anchoredPosition;
        startParent = transform.parent;
        canvasGroup.blocksRaycasts = false;

        //transform.SetParent(GetComponentInParent<Canvas>().transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endPosition = rt.anchoredPosition;

        if (!isDraggable)
        {
            return;
        }

        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<PuzzleDropZone>() == null)
        {
            ResetPosition();
        }
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 시작할 때 기록해 둔 위치와 부모로 되돌리는 기능
    /// </summary>
    public void ResetPosition()
    {
        rt.DOAnchorPos(startPosition, 0.5f); // DOTween기능을 이용해서 틀렸을 때 되돌아 가는 애니메이션 구현
        transform.SetParent(startParent, true);
    }
}
