using UnityEngine;

public class Resolution : MonoBehaviour
{
    float w;
    float h;
    Vector2 multiple;
    public bool forceY = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");
        w = canvas.GetComponent<RectTransform>().rect.width;
        h = canvas.GetComponent<RectTransform>().rect.height;
        multiple = new Vector2(w/720, h/1280);
        if (0.45 < (w / h) && (w / h) < 0.65)
            return;
        if (forceY )
            GetComponent<RectTransform>().anchoredPosition *= new Vector2(1, multiple.y);
        else
            GetComponent<RectTransform>().anchoredPosition *= multiple.x >= multiple.y ? new Vector2(multiple.x, 1) : new Vector2(1, multiple.y);
    }
}
