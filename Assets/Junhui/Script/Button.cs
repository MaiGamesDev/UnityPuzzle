using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool myBool;
    public AudioClip audioPressed;
    private Animation anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        myBool = true;
        Debug.Log("pointer down");

        SoundManager.Instance.PlaySound(audioPressed);
        anim.Play("Pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        myBool = false;
        Debug.Log("pointer up");

        anim.Play("Released");
    }

}
