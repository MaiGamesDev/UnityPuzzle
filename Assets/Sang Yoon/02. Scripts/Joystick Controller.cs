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
