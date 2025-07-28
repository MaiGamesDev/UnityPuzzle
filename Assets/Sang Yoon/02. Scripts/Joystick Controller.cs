using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 현제 스크립트는 투명한 배경의 UI의 컴포넌트로 사용되어있음
/// </summary>
public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    //private Vector2 offset;
    //private bool isDragging = false;

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
    //    offset = (Vector2)transform.position - mousePos;
    //    isDragging = true;
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    if(!isDragging) return;

    //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
    //    transform.position = mousePos + offset;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    isDragging = false;
    //}



    public GameObject puzzlePieces;
    public Transform parentTransform;

    public RectTransform joystickHandel;
    public Vector2 touchPos;
    public Vector2 localPoint;
    public Vector2 puzzlePos;

    public float moveSpeed = 10f;

    public bool isTouch = true;

    private void Start()
    {
        joystickHandel.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData) // 터치했을 때
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);

        localPoint = eventData.position;
        joystickHandel.position = localPoint;
        touchPos = localPoint;
        joystickHandel.gameObject.SetActive(true);

        if (puzzlePieces != null)
        {
            puzzlePos = puzzlePieces.transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData) // 터치해서 드래그 했을 때
    {
        joystickHandel.position = eventData.position;
        touchPos = eventData.position;

        if (puzzlePieces != null)
        {
            Vector2 dragOffset = eventData.position - localPoint;

            puzzlePieces.transform.position = puzzlePos + dragOffset * moveSpeed;
        }
    }

    public void OnPointerUp(PointerEventData eventData) // 손을 폰에서 떼었을 때
    {
        joystickHandel.position = Vector2.zero;
        joystickHandel.gameObject.SetActive(false);
    }
}
