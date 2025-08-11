using UnityEngine;

public class ResolutionScale : MonoBehaviour
{
    float w;
    float h;
    Vector2 multiple;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");
        w = canvas.GetComponent<RectTransform>().rect.width;
        h = canvas.GetComponent<RectTransform>().rect.height;
        multiple = new Vector2(w/720, h/1280);
        if (w < h) transform.localScale *= multiple.x;
        else transform.localScale *= multiple.y;
    }
}
