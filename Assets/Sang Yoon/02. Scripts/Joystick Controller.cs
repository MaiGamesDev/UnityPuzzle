using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 현제 스크립트는 검정 퍼즐조각 프리팹 15개에 사용되어있음
/// </summary>

public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 offset;
    private bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;

    public EmptyPuzzleSlot emptyPuzzle;

    private void Awake()
    {
        emptyPuzzle = GetComponent<EmptyPuzzleSlot>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle // 월드 좌표를 스크린 좌표계로 변환
            (
            rectTransform.parent as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out mousePos
            );
        offset = rectTransform.anchoredPosition - mousePos;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle // 월드 좌표를 스크린 좌표계로 변환
            (
            rectTransform.parent as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out mousePos
            );
        rectTransform.anchoredPosition = mousePos + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if (emptyPuzzle == null) return;

        //if (IsRectOverlapping(emptyPuzzle.EmptyPuzzleRT, rectTransform))
        //{
            
        //}
        //else
        //{

        //}
    }

    public bool IsRectOverlapping(RectTransform rt1, RectTransform rt2)
    {
        Vector3[] corner1 = new Vector3[4];
        Vector3[] corner2 = new Vector3[4];
        rt1.GetWorldCorners(corner1);
        rt2.GetWorldCorners(corner2);

        float rt1_minX = corner1[0].x;
        float rt1_maxX = corner1[2].x;
        float rt1_minY = corner1[0].y;
        float rt1_maxY = corner1[2].y;

        float rt2_minX = corner1[0].x;
        float rt2_maxX = corner1[2].x;
        float rt2_minY = corner1[0].y;
        float rt2_maxY = corner1[2].y;

        bool isOverlapping =
            rt1_minX < rt2_minX &&
            rt1_maxX > rt2_minX &&
            rt1_minY < rt2_maxY &&
            rt1_maxY > rt2_minY;

        return isOverlapping;
    }
}
