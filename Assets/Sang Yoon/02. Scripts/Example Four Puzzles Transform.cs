using System.Collections;
using UnityEngine;

public class ExampleFourPuzzlesTransform : MonoBehaviour
{
    private Transform puzzleExParent;

    IEnumerator Start()
    {
        yield return null;

        puzzleExParent = this.transform;
        RectTransform[] puzzleExChildren = puzzleExParent.GetComponentsInChildren<RectTransform>();
        foreach (Transform child in puzzleExParent)
        {
            RectTransform rt = child.GetComponent<RectTransform>();
            Debug.Log("퍼즐의 위치값 생성");

            if (rt != puzzleExParent.GetComponent<RectTransform>())
            {
                // anchoredPosition: UI에서 주로 쓰는 위치값
                Debug.Log("퍼즐 위치(anchoredPosition): " + rt.anchoredPosition);
            }
        }
    }
}
