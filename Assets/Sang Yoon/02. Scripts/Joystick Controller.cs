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

    public RectTransform puzzleHole;
    private GameObject[] correctPiece;

    private void Awake()
    {
        puzzleHole = GetComponent<RectTransform>();
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
        if (RectTransformUtility.RectangleContainsScreenPoint(puzzleHole, eventData.position, eventData.pressEventCamera))
        {
            
        }
        else
        {

        }
            isDragging = false;
    }
}
