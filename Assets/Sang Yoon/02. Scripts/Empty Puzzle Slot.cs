using System.Collections;
using UnityEngine;

public class EmptyPuzzleSlot : MonoBehaviour
{
    public Transform EmptyPuzzleT;

    IEnumerator Start()
    {
        yield return null;

        EmptyPuzzleT = this.transform;
        RectTransform EmptyPuzzleRT = GetComponentInChildren<RectTransform>();

        if (EmptyPuzzleRT != null)
        {
            Debug.Log($"퍼즐 구멍 위치{EmptyPuzzleRT.anchoredPosition}");
        }
    }
}
