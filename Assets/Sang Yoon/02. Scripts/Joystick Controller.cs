using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour
{
    public RectTransform joystickHandel;
    public Vector2 touchPos;

    public bool isTouch = true;


    void Joystick()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isTouch)
        {
            Debug.Log("터치 발생");

            joystickHandel.gameObject.SetActive(true);
            eventData.position = joystickHandel.position;
            touchPos = eventData.position;

            Debug.Log("터치 끝");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 발생");

        joystickHandel.position = eventData.position;
        touchPos = eventData.position;

        isTouch = false;

        Debug.Log("드래그 끝");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isTouch)
        {
            Debug.Log("터치 업 발생");


            joystickHandel.position = Vector2.zero;
            joystickHandel.gameObject.SetActive(false);

            isTouch = true;

            Debug.Log("터치 업 끝");

        }
    }
}
