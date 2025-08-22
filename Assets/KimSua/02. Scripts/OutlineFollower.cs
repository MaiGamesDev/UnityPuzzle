using UnityEngine;

public class OutlineFollower : MonoBehaviour
{
    public Transform target;

    private RectTransform rect;
    private RectTransform targetRect;

    private void Start()
    {
        rect = (RectTransform)transform;
        if (target != null)
        {
            targetRect = (RectTransform)target;
        }
    }

    void Update()
    {
        if (target != null && targetRect != null)
        {
            rect.position = targetRect.position;
            rect.rotation = targetRect.rotation;
        }
    }
}
