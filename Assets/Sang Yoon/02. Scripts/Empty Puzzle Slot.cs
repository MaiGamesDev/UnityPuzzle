using UnityEngine;

public class EmptyPuzzleSlot : MonoBehaviour
{
    public RectTransform EmptyPuzzleRT;

    private void Awake()
    {
        EmptyPuzzleRT = GetComponent<RectTransform>();
    }
}
